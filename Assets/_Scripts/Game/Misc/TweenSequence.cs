using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace Fight2048.Game.Misc
{
	public class TweenSequence
	{
		readonly Queue<Tween> _tweens = new Queue<Tween>();

		public bool IsPlaying => isPlaying;

		public void Append(Tween tween)
		{
			if (!isPlaying)
			{
				Play(tween);
				return;
			}
                
			_tweens.Enqueue(tween);
		}

		private bool isPlaying = false;
		void Play(Tween tween)
		{
			isPlaying = true;
			tween.OnComplete(Next);
			tween.Play();
		}

		void Next()
		{
			isPlaying = false;
			if (!_tweens.Any())
				return;
			Play(_tweens.Dequeue());
		}

		public void Clear(Transform transform)
		{
			isPlaying = false;
			transform.DOKill();
			_tweens.Clear();
		}
	}

}
