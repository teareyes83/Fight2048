using System;
using Fight2048.Game.Model;
using UnityEngine;
using Zenject;
using Random = System.Random;

namespace Fight2048.Game.Installers
{
    public class GameInstaller : Installer<GameInstaller>
    {
        public override void InstallBindings()
        {
            Container.DeclareSignal<TileMerged>();
            Container.Bind<Board>().FromSubContainerResolve().ByInstaller<BoardInstaller>().AsSingle();
            
            Container.BindFactory<int, Cell, Tile, Tile.Factory>().FromSubContainerResolve().ByInstaller<TileInstaller>();
            Container.BindFactory<int, int, Cell, Cell.Factory>().FromSubContainerResolve().ByInstaller<CellInstaller>();            
        }
    }
}
