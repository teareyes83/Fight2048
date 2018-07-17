using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;
using UnityEngine;
using Random = System.Random;

namespace Fight2048.Game.Model
{
    public class TileSpawnHandler
    {
        private readonly Grid _grid;
        
        private readonly Setting _setting;
        private readonly Random _tileValueRandom;
        
        public TileSpawnHandler(Setting setting, Grid grid, Random tileValueRandom)
        {
            _grid = grid;
            _setting = setting;
            _tileValueRandom = tileValueRandom;
        }

        public void Clear()
        {
            _grid.Clear();
        }

        public Tile SpawnTile(Cell cell, int value)
        {    
            var tile = _grid.SpawnTile(cell, value);
            return tile;
        }

        public Tile SpawnNewTile()
        {
            var cell = PickEmptyCell();
            if (cell == null)
            {
                return null;
            }

            var value = _setting.BaseTileValue * ((_tileValueRandom.Next() % 10 == 0) ? 2 : 1);
            return SpawnTile(cell, value);
        }

        private readonly Random _random = new Random();
        private Cell PickEmptyCell()
        {
            var emptyCells = _grid.EmptyCells;
            var count = emptyCells.Count();
            if (count == 0)
                return null;
            
            var pickedCellIndex = _random.Next() % count;

            var index = 0;
            foreach (var cell in emptyCells)
            {
                if (pickedCellIndex == index)
                {
                    return cell;
                }

                ++index;
            }

            return null;
        }

        [Serializable]
        public class Setting
        {
            public int BaseTileValue;
        }
    }
}