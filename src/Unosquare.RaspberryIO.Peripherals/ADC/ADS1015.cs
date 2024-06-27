using TGR.Unosquare.RaspberryIO.Abstractions;

namespace TGR.Unosquare.RaspberryIO.Peripherals.ADC
{
    /// <summary>
    /// ADS1015 Device.
    /// </summary>
    public class ADS1015 : ADS1x15
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ADS1015"/> class.
        /// </summary>
        /// <param name="device">The I2C device.</param>
        public ADS1015(II2CDevice device)
            : base(device, ADS1015CONVERSIONDELAY, 4)
        {
        }
    }
}
