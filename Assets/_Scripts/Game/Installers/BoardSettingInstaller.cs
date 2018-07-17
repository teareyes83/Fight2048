using Fight2048.Game.Model;
using Fight2048.Game.Presenter;
using Fight2048.Game.Misc;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "BoardSettingInstaller", menuName = "Installers/BoardSettingInstaller")]
public class BoardSettingInstaller : ScriptableObjectInstaller<BoardSettingInstaller>
{
    public Board.Setting BoardSetting;
    public TileSpawnHandler.Setting SpawnSetting;
    public CoordToPosition.Setting ConverterSetting;
    public TilePresenter.Setting TilePresenterSetting;
    
    public override void InstallBindings()
    {
        Container.BindInstance(BoardSetting);
        Container.BindInstance(SpawnSetting);
        Container.BindInstance(ConverterSetting);
        Container.BindInstance(TilePresenterSetting);
    }
}