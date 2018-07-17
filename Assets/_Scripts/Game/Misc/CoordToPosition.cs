using System;
using Fight2048.Game.Model;
using UnityEngine;

namespace Fight2048.Game.Misc
{
    public class CoordToPosition
    {
        private Setting _setting;
        private Board.Setting _boardSetting;
        public CoordToPosition(Setting setting, Board.Setting boardSetting)
        {
            _setting = setting;
            _boardSetting = boardSetting;
        }

        public Vector3 ToCellPosition(Coord coord)
        {
            return ToPosition(coord, 0);
        }
        
        public Vector3 ToTilePosition(Coord coord)
        {
            return ToPosition(coord, _setting.TileOffsetY);
        }
        
        Vector3 ToPosition(Coord coord, float y)
        {
            var offset = (_setting.Unit * _boardSetting.Size / 2) - _setting.Unit / 2;
            var offsetX = _setting.OffsetX - offset;
            var offsetZ = _setting.OffsetZ - offset;
            return new Vector3(coord.Col * _setting.Unit + offsetX, y ,
                -(coord.Row * _setting.Unit + offsetZ));
        }

        [Serializable]
        public class Setting
        {
            public float OffsetX;
            public float OffsetZ;
            public float TileOffsetY = 1.0f;
            public float Unit = 1.1f;
        }
    }
}