using System;
using Fight2048.Game.Model;
using UnityEngine;
using Zenject;
using Random = System.Random;

namespace Fight2048.Game.Installers
{
    public class TileInstaller : Installer<TileInstaller>
    {
        readonly int _number;
        readonly Cell _cell;
       
        public TileInstaller(
            [InjectOptional]
            int number, Cell cell)
        {
            _number = number;
            _cell = cell;
        }
        
        public override void InstallBindings()
        {
            Container.Bind<Tile>().AsSingle().WithArguments(_number, _cell);
        }
    }
}
