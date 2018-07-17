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
    public class CellInstallerTest : ZenjectIntegrationTestFixture
    {
        [UnityTest]
        public IEnumerator CreatePresenterTest()
        {
            PreInstall();

            SignalBusInstaller.Install(Container);
            Container.DeclareSignal<MoveSignal>();
            
            Container.Bind<Board.Setting>().AsSingle();
            Container.Bind<CoordToPosition.Setting>().AsSingle();
            Container.Bind<CoordToPosition>().AsSingle();
            Container.BindFactory<int, int, Cell, Cell.Factory>().FromSubContainerResolve().ByInstaller<CellInstaller>();
            var cellFactory = Container.Resolve<Cell.Factory>();
            
            Container.BindFactory<Cell, CellPresenter, CellPresenter.Factory>().FromPoolableMemoryPool<CellPresenter>(
                x => x.WithInitialSize(1).FromNewComponentOnNewGameObject().UnderTransformGroup("CellPool"));
            var cellPresenterFactory =  Container.Resolve<CellPresenter.Factory>();
                
            cellFactory.OnCreate += (_cell)=>
            {
                cellPresenterFactory.Create(_cell);
            };
            
            PostInstall();
            
            var cell = cellFactory.Create(1, 1);
            Assert.NotNull(cell);

            Assert.NotNull(GameObject.FindObjectOfType<CellPresenter>());

            yield return null;
        }

    }
}