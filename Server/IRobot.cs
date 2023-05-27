using Lib;

namespace Server
{
    public interface IRobot
    {
        Task<int> GetLenght();

        public List<Item> GetCamera();

        public void Move(Lib.Action action);
    }
}
