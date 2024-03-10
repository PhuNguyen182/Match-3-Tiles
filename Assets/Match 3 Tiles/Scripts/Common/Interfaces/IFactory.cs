namespace Match3Tiles.Scripts.Common.Interfaces
{
    public interface IFactory { }

    public interface IFactory<in T, out TR>
    {
        public TR Produce(T param);
    }
}
