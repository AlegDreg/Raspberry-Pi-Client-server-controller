using Lib;
using NeuroNet;

namespace Server
{
    public class RobotActions : IRobot
    {
        private Neuro Neuro;
        private List<Item> items = new List<Item>();
        private int lastLenght = 0;
        private int minLastLenght = 100;

        public RobotActions()
        {
            Neuro = new Neuro("http://192.168.0.140/");
            Neuro.OnDetect += (List<Item> it) =>
            {
                items.Clear();
                items.AddRange(it);
                Console.WriteLine(string.Join(",", it.Select(x => x.name)));
            };

            Neuro.Run();
        }

        public async Task<int> GetLenght()
        {
            if (lastLenght < minLastLenght)
                lastLenght = await ControllerActions.GetLenght();
            else
            {
                Task task = new Task(async () =>
                {
                    lastLenght = await ControllerActions.GetLenght();
                });
                task.Start();
            }

            return lastLenght;
        }

        public List<Item> GetCamera()
        {
            return items;
        }

        public void Move(Lib.Action action)
        {
            ControllerActions.Move(action.ToString());
        }
    }
}
