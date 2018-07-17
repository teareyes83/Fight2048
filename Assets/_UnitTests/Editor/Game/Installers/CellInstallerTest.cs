using System.Collections;
using Fight2048.Game;
using Fight2048.Game.Installers;
using Fight2048.Game.Model;
using Fight2048.Game.Presenter;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using Zenject;

namespace Fight2048.Tests.Game.Installers
{
    public class CellInstallerTest : ZenjectUnitTestFixture
    {
        [Test]
         public void CreateCellWithParametersTest()
        { 
           Container.BindFactory<int, int, Cell, Cell.Factory>().FromSubContainerResolve().ByInstaller<CellInstaller>();
        
           var cellFactory = Container.Resolve<Cell.Factory>();
        
           var cell = cellFactory.Create(1, 1);
           Assert.NotNull(cell);
           Assert.AreEqual(cell.Coord, new Coord(1, 1));
        }
    }
}