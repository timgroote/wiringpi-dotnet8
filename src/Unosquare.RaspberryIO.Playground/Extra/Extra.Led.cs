﻿namespace TGR.Unosquare.RaspberryIO.Playground.Extra
{
    using Abstractions;
    using global::System;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using Swan;
    using TGR.Unosquare.RaspberryIO;
    using TGR.Unosquare.WiringPi;

    public static partial class Extra
    {
        public static void TestLedBlinking()
        {
            using var cancellationTokenSource = new CancellationTokenSource();
            var task = Blink(cancellationTokenSource.Token);

            while (true)
            {
                var input = Console.ReadKey(true).Key;

                if (input != ConsoleKey.Escape)
                    continue;
                cancellationTokenSource.Cancel();
                break;
            }

            task.Wait(cancellationTokenSource.Token);
        }

        public static void TestLedDimming(bool hardware)
        {
            using var cancellationTokenSource = new CancellationTokenSource();
            var task = hardware ? DimHardware(cancellationTokenSource.Token) : DimSoftware(cancellationTokenSource.Token);

            while (true)
            {
                var input = Console.ReadKey(true).Key;

                if (input != ConsoleKey.Escape)
                    continue;
                cancellationTokenSource.Cancel();
                break;
            }

            task.Wait(cancellationTokenSource.Token);
        }

        /// <summary>
        /// For this test, connect an LED to Gpio13 and ground. (don't forget the resistor!).
        /// </summary>
        private static Task Blink(CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                Console.Clear();
                var blinkingPin = Pi.Gpio[BcmPin.Gpio13];

                // Configure the pin as an output
                blinkingPin.PinMode = GpioPinDriveMode.Output;

                // perform writes to the pin by toggling the isOn variable
                var isOn = false;
                while (!cancellationToken.IsCancellationRequested)
                {
                    isOn = !isOn;
                    blinkingPin.Write(isOn);
                    var ledState = isOn ? "on" : "off";
                    Console.Clear();
                    Console.WriteLine($"Blinking {ledState}");
                    Console.WriteLine(ExitMessage);
                    Thread.Sleep(500);
                }

                blinkingPin.Write(0);
            }, cancellationToken);
        }

        private static Task DimHardware(CancellationToken cancellationToken) =>
            Task.Run(async () =>
            {
                Console.Clear();
                Console.WriteLine("Hardware Dimming");
                Terminal.WriteLine(ExitMessage);

                var pin = (GpioPin)Pi.Gpio[BcmPin.Gpio13];
                pin.PinMode = GpioPinDriveMode.PwmOutput;
                pin.PwmMode = PwmMode.Balanced;
                pin.PwmClockDivisor = 2;

                while (!cancellationToken.IsCancellationRequested)
                {
                    for (var x = 0; x <= 100; x++)
                    {
                        pin.PwmRegister = (int)pin.PwmRange / 100 * x;
                        await Task.Delay(10, cancellationToken);
                    }

                    for (var x = 0; x <= 100; x++)
                    {
                        pin.PwmRegister = (int)pin.PwmRange - ((int)pin.PwmRange / 100 * x);
                        await Task.Delay(10, cancellationToken);
                    }
                }

                pin.PinMode = GpioPinDriveMode.Output;
                pin.Write(0);
            }, cancellationToken);

        /// <summary>
        /// For this test, connect a two-color LED to Gpio23, Gpio24 and ground. Tested with the KY-011 Module. (don't forget the resistor!).
        /// </summary>
        private static Task DimSoftware(CancellationToken cancellationToken) =>
            Task.Run(async () =>
            {
                Console.Clear();
                Console.WriteLine("Software Dimming");
                Terminal.WriteLine(ExitMessage);

                var pinGreen = (GpioPin)Pi.Gpio[BcmPin.Gpio23];
                var pinRed = (GpioPin)Pi.Gpio[BcmPin.Gpio24];

                pinGreen.PinMode = GpioPinDriveMode.Output;
                pinGreen.StartSoftPwm(0, 100);
                pinRed.PinMode = GpioPinDriveMode.Output;
                pinRed.StartSoftPwm(0, 100);

                var redOn = false;

                while (!cancellationToken.IsCancellationRequested)
                {
                    var pin = redOn ? pinRed : pinGreen;
                    redOn = !redOn;

                    for (var x = 0; x <= 100; x++)
                    {
                        pin.SoftPwmValue = pinGreen.SoftPwmRange / 100 * x;
                        await Task.Delay(10, cancellationToken);
                    }

                    for (var x = 0; x <= 100; x++)
                    {
                        pin.SoftPwmValue = pinGreen.SoftPwmRange - (pinGreen.SoftPwmRange / 100 * x);
                        await Task.Delay(10, cancellationToken);
                    }
                }

                pinGreen.Write(0);
                pinRed.Write(0);
                Terminal.WriteLine("End of task");
            }, cancellationToken);
    }
}
