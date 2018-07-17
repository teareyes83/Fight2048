using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using ModestTree;

namespace Fight2048.Game.Model
{
    public class TileMoveHandler
    {
        private TileRegistry _tileRegistry;
        private Grid _grid;
        
        public TileMoveHandler(TileRegistry tileRegistry, Grid grid)
        {
            _tileRegistry = tileRegistry;
            _grid = grid;
        }

        private readonly List<Tile> _moveOrderedTiles = new List<Tile>();
        public void MoveTiles(Direction direction) 
        {
            _moveOrderedTiles.Clear();
            _moveOrderedTiles.AddRange(_tileRegistry.Tiles);
            
            SortByMoveOrder(_moveOrderedTiles, direction);
            
            MoveTiles(_moveOrderedTiles, direction);
        }
        
        void MoveTiles(List<Tile> moveOrderedTiles, Direction direction) 
        {
            foreach (var tile in moveOrderedTiles)
            {
                MoveTile(tile, direction);
            }
        }

        public void SortByMoveOrder(List<Tile> tiles, Direction direction)
        {
            tiles.Sort(GetComparison(direction));
        }

        private Comparison<Tile> GetComparison(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    return (a, b) => a.Cell.Coord.Row - b.Cell.Coord.Row; 
                case Direction.Right:
                    return (a, b) => b.Cell.Coord.Col - a.Cell.Coord.Col;
                case Direction.Down:
                    return (a, b) => b.Cell.Coord.Row - a.Cell.Coord.Row;
                case Direction.Left:
                    return (a, b) => a.Cell.Coord.Col - b.Cell.Coord.Col;               
            }

            return null;
        }

        public bool CanMoveAny()
        {
            if (_grid.EmptyCells.Any())
            {
                return true;
            }
            
            var directions = Enum.GetValues(typeof(Direction));
            foreach (Direction direction in directions)
            {
                if (CanMoveAny(direction))
                    return true;
            }

            return false;
        }

        public bool CanMoveAny(Direction direction)
        {
            foreach (var tile in _tileRegistry.Tiles)
            {
                if (CanMoveTile(tile, direction))
                    return true;
            }

            return false;
        }

        bool CanMoveTile(Tile tile, Direction direction)
        {
            var nextCell = _grid.GetNextCell(tile.Cell, direction);
            if (nextCell == null)
                return false;
            
            if (nextCell.IsLocked())
                return false;
            
            if(nextCell.HasTile())
            {
                return nextCell.Tile.Number == tile.Number;
            }

            return true;
        }

        public void MoveTile(Tile tile, Direction direction)
        {
            var moveCount = 0;
            Cell cellTo = tile.Cell;
            while (moveCount < _grid.Size)
            {
                var nextCell = _grid.GetNextCell(cellTo, direction);
                if(nextCell == null)
                    break;
                
                if (nextCell.IsLocked())
                    break;
                
                if (nextCell.HasTile())
                {
                    if (nextCell.Tile.Number == tile.Number)
                    {
                        _grid.MoveTile(tile, nextCell);
                        nextCell.Lock();
                        ++moveCount;
                        return;
                    }
                    else
                    {
                        break;
                    }
                }

                cellTo = nextCell;
                ++moveCount;
            }

            if (moveCount > 0)
            {
                _grid.MoveTile(tile, cellTo);
            }
        }

        public void UnlockAllCell()
        {
            foreach (var tile in _tileRegistry.Tiles)
            {
                tile.Cell.Unlock();
            }
        }
    }
}