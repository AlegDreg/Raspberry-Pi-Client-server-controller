namespace Lib
{
    public class Item
    {
        public int x;
        public int y;
        public int id;
        public string name;
        public override string ToString()
        {
            return $"x:{x} y:{y} id:{id} name:{name}";
        }
    }

    public enum Action
    {
        forward,
        backward,
        right,
        left,
        stop
    }
}