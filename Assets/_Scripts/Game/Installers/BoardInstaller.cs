using System;
using Fight2048.Game.Model;
using Zenject;
using Random = System.Random;

namespace Fight2048.Game.Installers
{
	public class BoardInstaller : Installer<BoardInstaller>
	{
		public override void InstallBindings()
		{
			var setting = Container.Resolve<Board.Setting>();
			
			Container.Bind<Random>().AsTransient();

			InstallBoardStateManager();
			Container.Bind<BoardScoreManager>().AsSingle();
			Container.Bind<TileRegistry>().AsSingle();
			Container.Bind<Grid>().AsSingle().WithArguments(setting.Size);
			Container.Bind<TileMoveHandler>().AsSingle();
			Container.Bind<TileSpawnHandler>().AsSingle();

			Container.Bind<Board>().AsSingle();
		}

		void InstallBoardStateManager()
		{
			Container.Bind<GameStart>().AsSingle();
			Container.Bind<WaitForMove>().AsSingle();
			Container.Bind<ProcessMove>().AsSingle();
			Container.Bind<AfterMove>().AsSingle();
			Container.Bind<GameEnd>().AsSingle();
			Container.Bind<BoardStateManager>().AsSingle();
		}
	}
}
