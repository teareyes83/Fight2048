using System.Collections;
using Fight2048.Game;
using Fight2048.Game.Installers;
using Fight2048.Game.Misc;
using Fight2048.Game.Model;
using Fight2048.Game.Presenter;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using Zenject;

namespace Fight2048.IntegrationTests.Game.Installers
{
    public class TileInstallerTest : ZenjectIntegrationTestFixture
    {
        [UnityTest]
        public IEnumerator CreatePresenterTest()
        {
            PreInstall();
            
            SignalBusInstaller.Install(Container);
            Container.DeclareSignal<MoveSignal>();
            
            Container.Bind<Board.Setting>().AsSingle();
            Container.Bind<TilePresenter.Setting>().AsSingle();
            Container.Bind<CoordToPosition.Setting>().AsSingle();
            Container.Bind<CoordToPosition>().AsSingle();
            Container.BindFactory<int, Cell, Tile, Tile.Factory>().FromSubContainerResolve().ByInstaller<TileInstaller>();
            var tileFactory = Container.Resolve<Tile.Factory>();
            
            Container.BindFactory<Tile, TilePresenter, TilePresenter.Factory>().FromPoolableMemoryPool<TilePresenter>(
                x => x.WithInitialSize(1).FromNewComponentOnNewGameObject().UnderTransformGroup("TilePool"));
            var tilePresenterFactory =  Container.Resolve<TilePresenter.Factory>();
                
            tileFactory.OnCreate += (_tile)=>
            {
                tilePresenterFactory.Create(_tile);
            };
            
            PostInstall();
            
            var tile = tileFactory.Create(0, new Cell(0, 0));
            Assert.NotNull(tile);

            Assert.NotNull(GameObject.FindObjectOfType<TilePresenter>());
            
            yield return null;
        }

    }
}