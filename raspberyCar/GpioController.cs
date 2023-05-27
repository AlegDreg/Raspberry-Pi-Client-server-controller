using System.Device.Gpio;

namespace raspberyCar
{
    public class GpioControl : GpioController
    {
        ~GpioControl()
        {
            Dispose();
        }
    }
}