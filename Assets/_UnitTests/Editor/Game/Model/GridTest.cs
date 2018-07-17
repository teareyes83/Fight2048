using System.Linq;
using Fight2048.Game;
using Fight2048.Game.Model;
using NUnit.Framework;
using Zenject;

namespace Fight2048.Tests.Game.Model
{
    public class GridTest : ZenjectUnitTestFixture
    {
        [Test]
        public void ConstructorTest()
        {
            SignalBusInstaller.Install(Container);
            Container.BindFactory<int, Cell, Tile, Tile.Factory>();
            Container.Bind<TileRegistry>().AsSingle();
            Container.BindFactory<int, int, Cell, Cell.Factory>();
            var gridSize = 2;
            Container.Bind<Grid>().AsSingle().WithArguments(gridSize).NonLazy();

            var grid = Container.Resolve<Grid>();
            
            Assert.AreEqual(grid.Size, 2);
            Assert.AreEqual(grid.EmptyCells.Count(), 4);

            var cellR0C0 = grid.GetCell(0, 0);
            Assert.AreEqual(cellR0C0.Coord, new Coord(0, 0));
            
            var cellR1C1 = grid.GetCell(1, 1);
            Assert.AreEqual(cellR1C1.Coord, new Coord(1, 1));
            
        } 
        
        [Test]
        public void MoveTileTest()
        {
            SignalBusInstaller.Install(Container);
            Container.BindFactory<int, Cell, Tile, Tile.Factory>();
            Container.Bind<TileRegistry>().AsSingle();
            Container.BindFactory<int, int, Cell, Cell.Factory>();
            var gridSize = 2;
            Container.Bind<Grid>().AsSingle().WithArguments(gridSize).NonLazy();

            var grid = Container.Resolve<Grid>();

            Assert.AreEqual(grid.EmptyCells.Count(), 4);
            
            var cellR0C0 = grid.GetCell(0, 0);
            var tile = grid.SpawnTile(cellR0C0, 0);

            Assert.AreEqual(grid.EmptyCells.Count(), 3);
            
            Assert.AreEqual(tile.Cell, cellR0C0);
            Assert.AreEqual(cellR0C0.Tile, tile);
            
            var cellR0C1 = grid.GetCell(0, 1);
            grid.MoveTile(tile, cellR0C1);
            
            Assert.AreEqual(grid.EmptyCells.Count(), 3);
            
            Assert.AreEqual(cellR0C0.Tile, null);
            Assert.AreEqual(tile.Cell, cellR0C1);
            Assert.AreEqual(cellR0C1.Tile, tile);
        }
    }
}