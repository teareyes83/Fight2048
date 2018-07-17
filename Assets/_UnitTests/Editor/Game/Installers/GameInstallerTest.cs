using System.Collections;
using Fight2048.Game;
using Fight2048.Game.Installers;
using Fight2048.Game.Model;
using NUnit.Framework;
using UnityEditor;
using UnityEngine.TestTools;
using Zenject;

namespace Fight2048.Tests.Game.Installers
{
    public class GameInstallerTest : ZenjectUnitTestFixture
    {
        [Test]
        public void InstallTest()
        {
            SignalBusInstaller.Install(Container);
            Container.Bind<IRecord>().FromMock();
            Container.Bind<Board.Setting>().AsSingle();
            Container.Bind<TileSpawnHandler.Setting>().AsSingle();
            
            GameInstaller.Install(Container);

            var board = Container.Resolve<Board>();
            
            Assert.NotNull(board);
        }
    }
}