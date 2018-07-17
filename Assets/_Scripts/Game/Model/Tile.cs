using System;
using ModestTree;
using UniRx;
using UnityEngine;
using Zenject;

namespace Fight2048.Game.Model
{
    public class Tile : IDisposable
    {
        public Tile(int number, Cell cell)
        {
            Number = number;
            Cell = cell;
        }

        private readonly ReactiveProperty<Cell> _cell = new ReactiveProperty<Cell>();
        public ReactiveProperty<Cell> CellRP
        {
            get { return _cell; }
        }

        public Cell Cell
        {
            get { return _cell.Value; }
            set 
            {
                if (value != null)
                {
                    Assert.That(this._cell.Value != value);    
                }
                
                this._cell.Value = value;
            }
        }

        private readonly ReactiveProperty<int> _number = new IntReactiveProperty();
        public ReactiveProperty<int> NumberRP
        {
            get { return _number; }
        }

        public int Number
        {
            get { return _number.Value; }
            set { _number.Value = value; }
        }

        private int _pendingDistance;
        public int PendingDistance
        {
            get { return _pendingDistance; }
            set { _pendingDistance = value; }
        }

        void Reset()
        {
            Cell = null;
        }
        
        public void Dispose()
        {
            Reset();
        }

        public class Factory : PlaceholderFactory<int, Cell, Tile>
        {
            public delegate void CreateEvent(Tile cell);

            public event CreateEvent OnCreate;
            
            public override Tile Create(int param1, Cell param2)
            {
                var ret = base.Create(param1, param2);
                
                
                OnCreate?.Invoke(ret);
                
                return ret;
            }
        }
    }
    
    public interface ITilePresenter
    {
    }
}