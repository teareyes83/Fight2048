using UniRx;
using Zenject;

namespace Fight2048.Game.Model
{
    public class BoardScoreManager
    {
        private SignalBus _signalBus;
        private IRecord _record;

        private readonly ReactiveProperty<int> _score = new IntReactiveProperty();
        public ReactiveProperty<int> ScoreRP => _score;

        public int Score
        {
            get { return _score.Value; }
            set { _score.Value = value; }
        }
        
        private readonly ReactiveProperty<int> _highScore = new IntReactiveProperty();
        public ReactiveProperty<int> HighScoreRP => _highScore;

        public int HighScore
        {
            get { return _highScore.Value; }
            set { _highScore.Value = value; }
        }
        
        public BoardScoreManager(SignalBus signalBus, IRecord record)
        {
            _signalBus = signalBus;
            _signalBus.Subscribe<TileMerged>(OnTileMerged);
            _record = record;
            HighScore = _record.Load("hs", 0);
        }

        void OnTileMerged(TileMerged tileMerged)
        {
            Score += tileMerged.value;
            if (HighScore < Score)
            {
                HighScore = Score;
                _record.Save("hs", HighScore);
            }
        }

        public void ResetScore()
        {
            Score = 0;
        }
    }
}