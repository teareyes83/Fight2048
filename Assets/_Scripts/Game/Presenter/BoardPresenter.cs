using System;
using System.Collections;
using System.Collections.Generic;
using Fight2048.Game.Model;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Fight2048.Game.Presenter
{
    public class BoardPresenter : MonoBehaviour
    {
        [Inject] public Board Board;
        [Inject] public SignalBus Signal;

        private float lastMoveCompleteTimeStamp;

        public Text StateText;
        public Text HighScoreText;
        public Text ScoreText;
        public Button StartButton;

        void Start()
        {
            InitKeyboard();
            InitTileMoveSignal();

            Board.StateManager.CurrentState.Subscribe((x)=>
            {
                StateText.text = x.ToString();
                StartButton.gameObject.SetActive(x == BoardState.GameEnd);
            });

            StartButton.onClick.AsObservable()
            .Where(_=>Board.StateManager.CurrentState.Value == BoardState.GameEnd)
            .Subscribe(_ => { Board.StateComplete(); });
            
            Board.ScoreManager.ScoreRP.SubscribeToText(ScoreText);
            Board.ScoreManager.HighScoreRP.SubscribeToText(HighScoreText);
        }

        private void Update()
        {
            if (lastMoveCompleteTimeStamp <= 0) return;

            if (Time.time <= lastMoveCompleteTimeStamp) return;
            
            Board.StateComplete();
            lastMoveCompleteTimeStamp = -1.0f;
        }

        private void InitTileMoveSignal()
        {
            Signal.Subscribe<MoveSignal>(SubscribeMoveSignal);
        }

        void SubscribeMoveSignal(MoveSignal moveSignal)
        {
            var complete = Time.time + moveSignal.duration;
            if (lastMoveCompleteTimeStamp < complete)
            {
                lastMoveCompleteTimeStamp = complete;
            }
        }

        void InitKeyboard()
        {
            BindKeyboardArrow();
        }

        void BindKeyboardArrow()
        {
            BindKeyboard(KeyCode.UpArrow, Direction.Up);
            BindKeyboard(KeyCode.RightArrow, Direction.Right);
            BindKeyboard(KeyCode.DownArrow, Direction.Down);
            BindKeyboard(KeyCode.LeftArrow, Direction.Left);
        }

        void BindKeyboard(KeyCode code, Direction direction)
        {
            this.UpdateAsObservable()
                .Where(_ => Input.GetKeyDown(code))
                .ThrottleFirst(TimeSpan.FromMilliseconds(500))
                .Subscribe((u) => { Board.MoveTiles(direction); });
        }
    }
}


