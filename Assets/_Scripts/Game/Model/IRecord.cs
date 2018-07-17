namespace Fight2048.Game.Model
{
    public interface IRecord
    {
        void Save(string key, int value);
        int Load(string key, int defaultValue);
    }
}