using System.Linq;
using Fight2048.Game;
using Fight2048.Game.Model;
using NUnit.Framework;
using Zenject;
using Moq;
using Random = System.Random;

namespace Fight2048.Tests.Game.Model
{
    public class TileSpawnHandlerTest : ZenjectUnitTestFixture
    {
        [SetUp]
        public void SetUp()
        {
            SignalBusInstaller.Install(Container);
            Container.DeclareSignal<TileMerged>();
            Container.Bind<Random>().AsTransient();
            Container.BindFactory<int, Cell, Tile, Tile.Factory>();
            Container.Bind<TileRegistry>().AsSingle();
            Container.BindFactory<int, int, Cell, Cell.Factory>();
        }

        [Test]
        public void SpawnNewTileTest()
        {
            var gridSize = 1;
            Container.Bind<Grid>().AsSingle().WithArguments(gridSize);
            
            Container.Bind<TileSpawnHandler>().AsSingle().WithArguments(new TileSpawnHandler.Setting()).NonLazy();
            
            var tileSpawner = Container.Resolve<TileSpawnHandler>();
            var grid = Container.Resolve<Grid>();
            
            Assert.AreEqual(grid.EmptyCells.Count(), 1);
            
            var tile = tileSpawner.SpawnNewTile();
            
            Assert.AreEqual(grid.EmptyCells.Count(), 0);
            Assert.AreEqual(grid.GetCell(0, 0).Tile, tile);
        }  
        
        [Test]
        public void SpawnNewTileValueTest()
        {
            var gridSize = 2;
            Container.Bind<Grid>().AsSingle().WithArguments(gridSize);
            
            var randomMock = new Mock<Random>();
            randomMock.Setup((m) => m.Next()).Returns(1);
            Container.Rebind<Random>().FromInstance(randomMock.Object);

            var setting = new TileSpawnHandler.Setting(){ BaseTileValue = 2 };
            Container.Bind<TileSpawnHandler>().AsSingle().WithArguments(setting).NonLazy();
            
            var tileSpawner = Container.Resolve<TileSpawnHandler>();
            
            var tile = tileSpawner.SpawnNewTile();
                        
            Assert.AreEqual(tile.Number, 2);
            
            randomMock.Setup((m) => m.Next()).Returns(0);
            
            var tile2 = tileSpawner.SpawnNewTile();
            
            Assert.AreEqual(tile2.Number, 4);
        }          
    }
}