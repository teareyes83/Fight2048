using System;
using Fight2048.Game.Misc;
using Fight2048.Game.Model;
using TMPro;
using UnityEngine;
using Zenject;
using UniRx;

namespace Fight2048.Game.Presenter
{
	public class CellPresenter : MonoBehaviour, IPoolable<Cell, IMemoryPool>, IDisposable
	{
		public Cell _cell;		
		[Inject] public CoordToPosition Converter;
		
		private IMemoryPool _pool;
		readonly CompositeDisposable _disposables = new CompositeDisposable(); 
		
		public TextMeshPro CoordText;

		public class Factory : PlaceholderFactory<Cell, CellPresenter>
		{
		}

		public void OnDespawned()
		{
			gameObject.SetActive(false);
			_disposables.Clear();
		}

		public void OnSpawned(Cell p1, IMemoryPool p2)
		{
			gameObject.SetActive(true);
			_cell = p1;
			_pool = p2;
			
			_cell.CoordRP.Subscribe((Coord coord) =>
			{
				transform.position = Converter.ToCellPosition(coord);
			}).AddTo(_disposables);

			if(CoordText != null)
				CoordText.text = p1.Coord.ToString();
		}

		public void Dispose()
		{
			_pool.Despawn(this);
		}
	}

}

