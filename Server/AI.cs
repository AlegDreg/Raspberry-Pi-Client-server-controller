namespace Server
{
    public class AI
    {
        private IRobot Robot;
        private System.Timers.Timer Timer = new System.Timers.Timer(100);

        public AI(IRobot robot)
        {
            Robot = robot;
            Timer.AutoReset = true;
            Timer.Elapsed += Timer_Elapsed;
            Timer.Start();
        }

        private void Timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            //code for ai robot control 
        }
    }
}