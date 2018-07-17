using System.Collections.Generic;
using Fight2048.Game.Presenter;
using ModestTree;
using UniRx;
using UnityEngine;
using Zenject;

namespace Fight2048.Game.Model
{
    public class Cell
    {
        private readonly ReactiveProperty<Coord> _coord = new ReactiveProperty<Coord>();

        public ReactiveProperty<Coord> CoordRP
        {
            get { return _coord; }
        }

        public Coord Coord 
        {
            get { return _coord.Value; }
        }
        
        public Cell(int row, int col)
        {
            _coord.Value = new Coord(row, col);
        }
        
        private Tile _tile;
        public Tile Tile 
        {
            get { return _tile; }
            set
            {
                _tile = value;
            }
        }

        private bool _locked = false;

        public bool IsLocked()
        {
            return _locked;
        }

        public void Lock()
        {
            _locked = true;
        }
        
        public void Unlock()
        {
            _locked = false;
        }

        public bool HasTile()
        {
            return _tile != null;
        }
        
        public class Factory : PlaceholderFactory<int, int, Cell>
        {
            public delegate void CreateEvent(Cell cell);

            public event CreateEvent OnCreate;
            
            public override Cell Create(int param1, int param2)
            {
                var ret = base.Create(param1, param2);
                
                OnCreate?.Invoke(ret);
                
                return ret;
            }
        }
    }

    public interface ICellPresenter : IPresenter
    {
    }
}