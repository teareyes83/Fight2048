using System;
using Fight2048.Game.Misc;
using Fight2048.Game.Model;
using Fight2048.Game.Presenter;
using UnityEngine;
using Zenject;
using Random = System.Random;

namespace Fight2048.Game.Installers
{
    public class GameMonoInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
            Container.DeclareSignal<MoveSignal>();
            
            GameInstaller.Install(Container);

            InstallPresenters();
        }

        private void InstallPresenters()
        {
            InstallRecord();
            InstallPresenterMisc();
            InstallCellPresenters();
            InstallTilePresenters();
        }

        private void InstallRecord()
        {
            Container.BindInterfacesTo<PlayerPrefsRecord>().AsSingle();
        }

        private void InstallPresenterMisc()
        {
            Container.Bind<CoordToPosition>().AsSingle();
        }

        [SerializeField] 
        private GameObject cellPrefab;
        private void InstallCellPresenters()
        {
            if (!Container.HasBinding<Cell.Factory>()) return;
            
            Container.BindFactory<Cell, CellPresenter, CellPresenter.Factory>().FromPoolableMemoryPool<CellPresenter>(
                x => x.WithInitialSize(16).FromComponentInNewPrefab(cellPrefab).UnderTransformGroup("CellPool"));
            
            var cellPresenterFactory =  Container.Resolve<CellPresenter.Factory>();
                
            var cellFactory = Container.Resolve<Cell.Factory>();
            cellFactory.OnCreate += (cell)=>
            {
                cellPresenterFactory.Create(cell);
            };
        }
        
        
        [SerializeField] 
        private GameObject tilePrefab;
        private void InstallTilePresenters()
        {
            if (!Container.HasBinding<Tile.Factory>()) return;
            
            Container.BindFactory<Tile, TilePresenter, TilePresenter.Factory>().FromPoolableMemoryPool<TilePresenter>(
                x => x.WithInitialSize(16).FromComponentInNewPrefab(tilePrefab).UnderTransformGroup("TilePool"));
                        
            var tilePresenterFactory =  Container.Resolve<TilePresenter.Factory>();
                
            var tileFactory = Container.Resolve<Tile.Factory>();
            tileFactory.OnCreate += (tile)=>
            {
                tilePresenterFactory.Create(tile);
            };
        }
    }
}
