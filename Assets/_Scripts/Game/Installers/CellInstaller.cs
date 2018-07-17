using System;
using Fight2048.Game.Model;
using Fight2048.Game.Presenter;
using UnityEngine;
using Zenject;
using Random = System.Random;

namespace Fight2048.Game.Installers
{
    public class CellInstaller : Installer<CellInstaller>
    {
        readonly int _row;
        readonly int _col;
        public CellInstaller(
            [InjectOptional]
            int row, int col)
        {
            _row = row;
            _col = col;
        }
        
        public override void InstallBindings()
        {     
            Container.Bind<Cell>().AsSingle().WithArguments(_row, _col);
        }
    }
}
