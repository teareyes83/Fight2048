using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using ModestTree;

namespace Fight2048.Game.Model
{
    public class Board 
    {
        private Setting _setting;
        private BoardStateManager _boardStateManager;
        private BoardScoreManager _boardScoreManager;
        
        public Board(Setting setting, BoardStateManager boardStateManager, BoardScoreManager boardScoreManager)
        {           
            _setting = setting;
            _boardStateManager = boardStateManager;
            _boardScoreManager = boardScoreManager;
        }

        public BoardStateManager StateManager { get { return _boardStateManager; }}
        public BoardScoreManager ScoreManager { get { return _boardScoreManager; }}

        public void MoveTiles(Direction direction)
        {
            _boardStateManager.CommandMove(direction);
        }

        public void StateComplete()
        {
            _boardStateManager.StateComplete();
        }

        [Serializable]
        public class Setting
        {
            public int Size = 1;
        }
    }
}