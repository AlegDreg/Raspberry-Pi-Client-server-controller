using Lib;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Threading.Tasks;
using System.Timers;

namespace raspberyCar
{
    public class ActionObj
    {
        public int Port;
        public bool R;
    }

    public class Movements
    {
        GpioControl controller;
        Action? CurrentAction;
        Action? NextAction;
        Timer Timer;
        private readonly int[] pins = { 17, 22, 23, 24 };

        public Movements(GpioControl gpioControl)
        {
            controller = gpioControl;
            OpenPins();
            Timer = new Timer(150);
            Timer.AutoReset = true;
            Timer.Elapsed += Timer_Elapsed;
            Timer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (NextAction != null)
            {
                StartAction((Action)NextAction);

                NextAction = null;
            }
            else
            {
                if (CurrentAction != null)
                    StopPins();
            }
        }

        private void StartAction(Action action)
        {
            StopPins();
            switch (action)
            {
                case Action.forward: Forward(); break;
                case Action.backward: Backward(); break;
                case Action.stop: StopPins(); break;
                case Action.left: TurnLeft(); break;
                case Action.right: TurnRight(); break;
            }
        }

        ~Movements()
        {
            ClosePins();
        }

        private void OpenPins()
        {
            foreach (var pin in pins)
                controller.OpenPin(pin, PinMode.Output);
        }

        private void ClosePins()
        {
            foreach (var pin in pins)
                controller.ClosePin(pin);
        }

        private void StopPins()
        {
            foreach (var pin in pins)
                controller.Write(pin, PinValue.Low);
            CurrentAction = null;
        }

        public void Move(Action action)
        {
            NextAction = action;
        }

        public async void Move(List<ActionObj> obj, int millsec)
        {
            StopPins();
            for (int i = 0; i < obj.Count; i++)
            {
                controller.Write(obj[i].Port, obj[i].R ? PinValue.High : PinValue.Low); //назад правое
            }
            await Task.Delay(millsec);
            StopPins();
        }

        public void Move(string action)
        {
            switch (action)
            {
                case "forward":
                    NextAction = Action.forward; break;
                case "backward":
                    NextAction = Action.backward; break;
                case "right":
                    NextAction = Action.right; break;
                case "left":
                    NextAction = Action.left; break;
            }
        }

        private void TurnRight()
        {
            controller.Write(17, PinValue.High); //назад правое
            controller.Write(22, PinValue.Low); //веред правое
            controller.Write(23, PinValue.Low); //назад левое
            controller.Write(24, PinValue.High); //вперед левое
            CurrentAction = Action.right;
        }

        private void TurnLeft()
        {
            controller.Write(17, PinValue.Low); //назад правое
            controller.Write(22, PinValue.High); //веред правое
            controller.Write(23, PinValue.High); //назад левое
            controller.Write(24, PinValue.Low); //вперед левое
            CurrentAction = Action.left;
        }

        private void Backward()
        {
            controller.Write(17, PinValue.High);
            controller.Write(22, PinValue.Low);
            controller.Write(23, PinValue.High);
            controller.Write(24, PinValue.Low);
            CurrentAction = Action.backward;
        }

        private void Forward()
        {
            controller.Write(17, PinValue.Low);
            controller.Write(22, PinValue.High);
            controller.Write(23, PinValue.Low);
            controller.Write(24, PinValue.High);
            CurrentAction = Action.forward;
        }
    }
}