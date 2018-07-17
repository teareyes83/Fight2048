using System.Collections.Generic;
using System.Linq;
using ModestTree;
using UnityEngine;
using Random = System.Random;

namespace Fight2048.Game.Model
{
    public class TileRegistry
    {
        private readonly Tile.Factory _tileFactory;
        
        private readonly HashSet<Tile> _tiles = new HashSet<Tile>();
        public IEnumerable<Tile> Tiles => _tiles;
        
        public TileRegistry(Tile.Factory tileFactory)
        {
            _tileFactory = tileFactory;
        }
        
        public Tile SpawnTile(int value, Cell cell)
        {
            Tile tile = _tileFactory.Create(value, cell);
            _tiles.Add(tile);
            return tile;
        }
        
        public void DespawnTile(Tile tile)
        {
            Assert.That(_tiles.Remove(tile));
            tile.Dispose();
        }
    }
}