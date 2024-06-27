namespace TGR.Unosquare.RaspberryIO.Playground.Extra
{
    using global::System;
    using global::System.Collections.Generic;
    using Swan;

    public static partial class Extra
    {
        private const string ExitMessage = "Press Esc key to continue . . .";

        private static readonly Dictionary<ConsoleKey, string> MainOptions = new Dictionary<ConsoleKey, string>
        {
            { ConsoleKey.B, "Test Button" },
            { ConsoleKey.L, "Led Blinking" },
            { ConsoleKey.D, "Led Dimming, Hardware PWM" },
            { ConsoleKey.S, "Led Dimming, Software PWM" },
        };

        public static void ShowMenu()
        {
            var exit = false;

            do
            {
                Console.Clear();
                var mainOption = Terminal.ReadPrompt("Extra Examples", MainOptions, "Esc to exit this menu");

                switch (mainOption.Key)
                {
                    case ConsoleKey.B:
                        TestButton();
                        break;
                    case ConsoleKey.L:
                        TestLedBlinking();
                        break;
                    case ConsoleKey.D:
                        TestLedDimming(true);
                        break;
                    case ConsoleKey.S:
                        TestLedDimming(false);
                        break;
                    case ConsoleKey.Escape:
                        exit = true;
                        break;
                }
            }
            while (!exit);
        }
    }
}
