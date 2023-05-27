using System;

namespace raspberyCar
{
    public class Distance
    {
        GpioControl GpioControl;
        public Distance(GpioControl gpioControl)
        {
            GpioControl = gpioControl;
        }

        /// <summary>
        /// Get distanse from Distance Sensor
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public int Get()
        {
            throw new Exception();
        }
    }
}