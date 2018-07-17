using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using DG.Tweening;
using Fight2048.Game.Misc;
using Fight2048.Game.Model;
using NUnit.Framework.Constraints;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Fight2048.Game.Presenter
{
    public class TilePresenter : MonoBehaviour, IPoolable<Tile, IMemoryPool>, IDisposable
    {
        private Tile _tile;
        [Inject] public CoordToPosition Converter;
        [Inject] public Setting PresenterSetting;
        [Inject] public SignalBus Signal;
        
        public TextMeshPro ValueText;

        private IMemoryPool _pool;
        readonly CompositeDisposable _disposables = new CompositeDisposable();

        private int? _numberChanged;

        public void OnDespawned()
        {
            _pool = null;
            gameObject.SetActive(false);
            _disposables.Clear();
            transform.localScale = Vector3.one;
        }

        public void OnSpawned(Tile p1, IMemoryPool p2)
        {
            gameObject.SetActive(true);
            _tile = p1;
            _pool = p2;

            transform.position = Converter.ToTilePosition(p1.Cell.Coord);
            
            _tile.CellRP.Subscribe(cell =>
            {
                if (cell != null)
                {
                    var to = Converter.ToTilePosition(cell.Coord);
                    var tile = _tile;
                    var duration = tile.PendingDistance / PresenterSetting.MoveSpeed;

                    var seq = DOTween.Sequence();
                    seq.Append(transform.DOMove(to, duration));
                    if (_numberChanged.HasValue)
                    {
                        seq.AppendCallback(() =>
                        {
                            TweenNumberScale(_numberChanged.Value);
                            _numberChanged = null;
                        });
                    }

                    seq.Play();
                    Signal.Fire(new MoveSignal(){ duration = duration});
                }
                else
                {
                    var tile = _tile;
                    var delay = tile.PendingDistance / PresenterSetting.MoveSpeed;

                    var seq = DOTween.Sequence();
                    var scale = transform.DOScale(Vector3.zero, 0.3f).SetDelay(delay);
                    seq.Append(scale);
                    seq.AppendCallback(this.Dispose);
                    seq.Play();
                }
            }).AddTo(_disposables);

            _tile.NumberRP.Skip(1).Subscribe(n =>
            {
                _numberChanged = n;
            }).AddTo(_disposables);

            TweenNumberScale(p1.Number);
        }

        void TweenNumberScale(int val)
        {
            if(ValueText != null)
                ValueText.text = val.ToString();
            var seq2 = DOTween.Sequence();
            seq2.Append(transform.DOScale(1.2f, 0.2f));
            seq2.Append(transform.DOScale(1.0f, 0.2f));
            seq2.Play();
        }

        public void Dispose()
        {
            _pool.Despawn(this);
        }

        public class Factory : PlaceholderFactory<Tile, TilePresenter>
        {
        }

        [Serializable]
        public class Setting
        {
            public float MoveSpeed = 10.0f;
        }
    }
}

