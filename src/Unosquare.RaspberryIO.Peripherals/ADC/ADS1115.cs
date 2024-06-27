using TGR.Unosquare.RaspberryIO.Abstractions;

namespace TGR.Unosquare.RaspberryIO.Peripherals.ADC
{
    /// <summary>
    /// ADS1115 Device.
    /// Also available pre-packaged on an experimental board as KY-053. 
    /// </summary>
    public class ADS1115 : ADS1x15
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ADS1115"/> class.
        /// </summary>
        /// <param name="device">The I2C device.</param>
        public ADS1115(II2CDevice device)
            : base(device, ADS1015CONVERSIONDELAY, 0)
        {
        }
    }
}
