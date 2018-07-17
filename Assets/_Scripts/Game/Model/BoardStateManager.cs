
using System.Collections.Generic;
using UniRx;
using Zenject;

namespace Fight2048.Game.Model
{
    public class BoardStateManager
    {
        public ReactiveProperty<BoardState> CurrentState = new ReactiveProperty<BoardState>();

        private IBoardState _currentState;
        private List<IBoardState> _states;
        
        [Inject]
        public void Construct(GameStart gameStart,
            WaitForMove waitForMove,
            ProcessMove processMove,
            AfterMove afterMove,
            GameEnd gameEnd)
        {
            _states = new List<IBoardState>()
            {
                gameStart,
                waitForMove,
                processMove,
                afterMove,
                gameEnd,
            };
            
            ChangeState(BoardState.GameEnd);
        }

        public void ChangeState(BoardState state)
        {
            _currentState?.OnExitState();
            _currentState = _states[(int)state];
            CurrentState.Value = state;
            _currentState.OnEnterState();
        }

        public void StateComplete()
        {
            _currentState.Complete();
        }

        public void CommandMove(Direction direction)
        {
            _currentState.CommandMove(direction);
        }
    }


    public enum BoardState
    {
        GameStart,
        WaitForMove,
        ProcessMove,
        AfterMove, 
        GameEnd,
    }
    
    public interface IBoardState
    {
        void OnEnterState();
        void OnExitState();
        void CommandMove(Direction direction);
        void Complete();
    }

    public abstract class StateBase
    {
        protected BoardStateManager _manager;
        public StateBase(BoardStateManager manager)
        {
            _manager = manager;
        }
    }

    public class GameStart : StateBase, IBoardState
    {
        private TileSpawnHandler _tileSpawnHandler;
        private BoardScoreManager _scoreManager;
        
        public GameStart(TileSpawnHandler tileSpawnHandler, BoardScoreManager scoreManager, BoardStateManager manager): base(manager)
        {
            _tileSpawnHandler = tileSpawnHandler;
            _scoreManager = scoreManager;
        }

        void IBoardState.OnEnterState()
        {
            _tileSpawnHandler.Clear();
            _scoreManager.ResetScore();
            _tileSpawnHandler.SpawnNewTile();
            _manager.ChangeState(BoardState.WaitForMove);
        }

        void IBoardState.OnExitState()
        {
        }
        
        void IBoardState.CommandMove(Direction direction)
        {
        }

        void IBoardState.Complete()
        {
            
        }
    }
    
    public class WaitForMove : StateBase, IBoardState
    {
        private TileMoveHandler _tileMoveHandler;
        public WaitForMove(BoardStateManager manager, TileMoveHandler tileMoveHandler) : base(manager)
        {
            _tileMoveHandler = tileMoveHandler;
        }
        
        void IBoardState.OnEnterState()
        {
        }

        void IBoardState.OnExitState()
        {
        }
        
        void IBoardState.CommandMove(Direction direction)
        {
            if (!_tileMoveHandler.CanMoveAny())
            {
                _manager.ChangeState(BoardState.GameEnd);
                return;
            }

            if (!_tileMoveHandler.CanMoveAny(direction))
            {
                return;
            }

            _manager.ChangeState(BoardState.ProcessMove);
            _tileMoveHandler.MoveTiles(direction);
        }

        void IBoardState.Complete()
        {
        }  
    }
    
    public class ProcessMove : StateBase, IBoardState 
    {
        private TileMoveHandler _tileMoveHandler;
        
        public ProcessMove(BoardStateManager manager, TileMoveHandler tileMoveHandler) : base(manager)
        {
            _tileMoveHandler = tileMoveHandler;
        }

        void IBoardState.OnEnterState()
        {
        }

        void IBoardState.OnExitState()
        {
            _tileMoveHandler.UnlockAllCell();
        }
        
        void IBoardState.CommandMove(Direction direction)
        {
        }

        void IBoardState.Complete()
        {
            _manager.ChangeState(BoardState.AfterMove);
        }  
    }
    
    public class AfterMove : StateBase, IBoardState 
    {
        private TileSpawnHandler _tileSpawnHandler;
        
        
        public AfterMove(TileSpawnHandler tileSpawnHandler, BoardStateManager manager): base(manager)
        {
            _tileSpawnHandler = tileSpawnHandler;
        }
       
        void IBoardState.OnEnterState()
        {
            _tileSpawnHandler.SpawnNewTile();
            _manager.ChangeState(BoardState.WaitForMove);
        }

        void IBoardState.OnExitState()
        {
        }
        
        void IBoardState.CommandMove(Direction direction)
        {
        }

        void IBoardState.Complete()
        {
        }  
    }
    
    public class GameEnd : StateBase, IBoardState 
    {
        public GameEnd(BoardStateManager manager, TileRegistry tileRegistry): base(manager)
        {
        }
       
        void IBoardState.OnEnterState()
        {
        }

        void IBoardState.OnExitState()
        {
        }
        
        void IBoardState.CommandMove(Direction direction)
        {
        }

        void IBoardState.Complete()
        {
            _manager.ChangeState(BoardState.GameStart);
        }  
    }
}