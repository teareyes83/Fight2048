using System;
using System.Collections.Generic;
using ModestTree;
using UnityEngine;
using Zenject;

namespace Fight2048.Game.Model
{
    public class Grid
    {
        private readonly Cell[,] _cells;
        
        private readonly int _size;
        public int Size => _size;
        
        private readonly TileRegistry _tileRegistry;
        
        private HashSet<Cell> _emptyCells = new HashSet<Cell>();
        public IEnumerable<Cell> EmptyCells => _emptyCells;

        private SignalBus _signalBus;
        
        public Grid(int size, Cell.Factory cellFactory, TileRegistry tileRegistry, SignalBus signalBus)
        {
            _cells = new Cell[size, size];
            _size = size;
            for (var row = 0; row < size; ++row)
            {
                for (var col = 0; col < size; ++col)
                {
                    Cell cell = cellFactory.Create(row, col);
                    _cells[row, col] = cell;
                    _emptyCells.Add(cell);
                }
            }

            _tileRegistry = tileRegistry;
            _signalBus = signalBus;
        }

        public Cell GetCell(int row, int col)
        {
            return _cells[row, col];
        }

        public Cell GetNextCell(Cell cell, Direction direction)
        {
            int nextCellRow = cell.Coord.Row;
            int nextCellCol = cell.Coord.Col;
            
            switch (direction)
            {
                case Direction.Up:
                    --nextCellRow;
                    break;
                case Direction.Right:
                    ++nextCellCol;
                    break;
                case Direction.Down:
                    ++nextCellRow;
                    break;
                case Direction.Left:
                    --nextCellCol;
                    break;
            }

            if (IsOutOfGrid(nextCellRow, nextCellCol))
                return null;

            return GetCell(nextCellRow, nextCellCol);
        }

        bool IsOutOfGrid(int row, int col)
        {
            if (row < 0)
                return true;
            if (row >= _size)
                return true;
            
            if (col < 0)
                return true;
            return col >= _size;
        }

        public void MoveTile(Tile tile, Cell cellTo)
        {
            Cell cellFrom = tile.Cell;
            cellFrom.Tile = null;
            _emptyCells.Add(cellFrom);

            var distance = 0;
            if (cellTo != null)
            {
                distance = Coord.Distance(cellFrom.Coord, cellTo.Coord);
                tile.PendingDistance = distance;
            }
            else
            {
                _tileRegistry.DespawnTile(tile);
                return;
            }

            var previousTile = cellTo.Tile;
            cellTo.Tile = tile;
            
            if (previousTile != null)
            {
                tile.Number += previousTile.Number;
                
                _signalBus.Fire(new TileMerged(){ value  = tile.Number});
                
                previousTile.PendingDistance = distance;
                _tileRegistry.DespawnTile(previousTile);
            }
            else
            {
                _emptyCells.Remove(cellTo);
            }
            
            tile.Cell = cellTo;
            tile.PendingDistance = distance;
        }
        
        public Tile SpawnTile(Cell cell, int value)
        {
            var tile = _tileRegistry.SpawnTile(value, cell);
            _emptyCells.Remove(cell);
            cell.Tile = tile;
            return tile;
        }
        
        public void RemoveTile(Tile tile)
        {
            MoveTile(tile, null);
        }

        public void Clear()
        {
            var removeList = new List<Tile>(_tileRegistry.Tiles);
            foreach (var each in removeList)
            {
                RemoveTile(each);
            }
        }
    }
}