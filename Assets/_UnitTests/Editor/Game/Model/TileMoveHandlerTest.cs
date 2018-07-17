using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Fight2048.Game;
using Fight2048.Game.Model;
using Moq;
using NUnit.Framework;
using UnityEditorInternal;
using UnityEngine.TestTools;
using Zenject;

namespace Fight2048.Tests.Game.Model
{
    public class TileMoveHandlerTest : ZenjectUnitTestFixture
    {
        private Grid _grid;
        private TileMoveHandler _tileMoveHandler;
        private int _baseTileValue = 2;
        private readonly int _gridSize = 3;
        
        [SetUp]
        public void SetUp()
        {
            SignalBusInstaller.Install(Container);
            Container.DeclareSignal<TileMerged>();
            Container.BindFactory<int, Cell, Tile, Tile.Factory>();
            Container.Bind<TileRegistry>().AsSingle();
            Container.BindFactory<int, int, Cell, Cell.Factory>();
            Container.Bind<Grid>().AsSingle().WithArguments(_gridSize);            
            
            Container.Bind<TileMoveHandler>().AsSingle().NonLazy();

            _grid = Container.Resolve<Grid>();
            _tileMoveHandler = Container.Resolve<TileMoveHandler>();
        }

        [TearDown]
        public void TearDown()
        {
            Container.UnbindAll();
            _grid = null;
            _tileMoveHandler = null;
        }

        [Test]
        public void MoveTileSimpleTest()
        {
            var cellR0C0 =_grid.GetCell(0, 0);
            var tile = _grid.SpawnTile(cellR0C0, _baseTileValue);

            var largestIndex = _gridSize - 1;
            
            Assert.AreEqual(tile.Cell.Coord, new Coord(0, 0));
            
            _tileMoveHandler.MoveTile(tile, Direction.Right);
            
            Assert.AreEqual(tile.Cell.Coord, new Coord(0, largestIndex));
            
            _tileMoveHandler.MoveTile(tile, Direction.Down);
            
            Assert.AreEqual(tile.Cell.Coord, new Coord(largestIndex, largestIndex));
            
            _tileMoveHandler.MoveTile(tile, Direction.Left);
            
            Assert.AreEqual(tile.Cell.Coord, new Coord(largestIndex, 0));
            
            _tileMoveHandler.MoveTile(tile, Direction.Up);
            
            Assert.AreEqual(tile.Cell.Coord, new Coord(0, 0));
        }
        
        [Test]
        public void SortByMoveOrderTest()
        {
            var tileR0C0 = new Tile(0, new Cell(0, 0));
            var tileR1C1 = new Tile(0, new Cell(1, 1));
            var tileR2C2 = new Tile(0, new Cell(2, 2));
            var tileR3C3 = new Tile(0, new Cell(3, 3));
            
            var list = new List<Tile>(){ tileR1C1, tileR0C0, tileR3C3, tileR2C2 };
            
            var orderedByUp = new List<Tile>(list);
            _tileMoveHandler.SortByMoveOrder(orderedByUp, Direction.Up);
            
            Assert.AreEqual(orderedByUp[0], tileR0C0);
            Assert.AreEqual(orderedByUp[1], tileR1C1);
            Assert.AreEqual(orderedByUp[2], tileR2C2);
            Assert.AreEqual(orderedByUp[3], tileR3C3);
            
            var orderedByRight = new List<Tile>(list);
            _tileMoveHandler.SortByMoveOrder(orderedByRight, Direction.Right);
            
            Assert.AreEqual(orderedByRight[0], tileR3C3);
            Assert.AreEqual(orderedByRight[1], tileR2C2);
            Assert.AreEqual(orderedByRight[2], tileR1C1);
            Assert.AreEqual(orderedByRight[3], tileR0C0);
        }
        
        [Test]
        public void MoveTilesMergeTest()
        {
            var cellR0C0 =_grid.GetCell(0, 0);
            var tileR0C0 = _grid.SpawnTile(cellR0C0, _baseTileValue);
            
            var cellR0C1 =_grid.GetCell(0, 1);
            _grid.SpawnTile(cellR0C1, _baseTileValue);
            
            var cellR0C2 =_grid.GetCell(0, 2);
            _grid.SpawnTile(cellR0C2, _baseTileValue);
            
            _tileMoveHandler.MoveTiles(Direction.Right);
            
            Assert.AreEqual(cellR0C0.HasTile(), false);
            Assert.AreEqual(cellR0C1.Tile, tileR0C0);
            Assert.AreEqual(cellR0C2.Tile.Number, _baseTileValue * 2);
        }
    }
}