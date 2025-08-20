using System;
using System.Collections.Generic;

namespace paradigm_shift_csharp
{
    enum BreachType { Normal, TooLow, TooHigh }

    class BatteryCheckResult
    {
        public string Parameter { get; }
        public BreachType Breach { get; }

        public BatteryCheckResult(string parameter, BreachType breach)
        {
            Parameter = parameter;
            Breach = breach;
        }

        public bool IsOk() => Breach == BreachType.Normal;

        public string Message()
        {
            return Breach switch
            {
                BreachType.TooLow => $"{Parameter} is too low!",
                BreachType.TooHigh => $"{Parameter} is too high!",
                _ => $"{Parameter} is OK"
            };
        }
    }

    class Checker
    {
        static BreachType InferBreach(float value, float min, float max)
        {
            if (value < min) return BreachType.TooLow;
            if (value > max) return BreachType.TooHigh;
            return BreachType.Normal;
        }

        static List<BatteryCheckResult> BatteryHealth(float temperature, float soc, float chargeRate)
        {
            var results = new List<BatteryCheckResult>
            {
                new BatteryCheckResult("Temperature", InferBreach(temperature, 0, 45)),
                new BatteryCheckResult("State of Charge", InferBreach(soc, 20, 80)),
                new BatteryCheckResult("Charge Rate", InferBreach(chargeRate, 0, 0.8f))
            };
            return results;
        }
        static bool BatteryIsOk(float temperature, float soc, float chargeRate)
        {
            var results = BatteryHealth(temperature, soc, chargeRate);
            foreach (var result in results)
            {
                if (!result.IsOk())
                {
                    Console.WriteLine(result.Message());
                    return false;
                }
            }
            return true;
        }

        static void ExpectTrue(bool expression)
        {
            if (!expression)
            {
                Console.WriteLine("Expected true, but got false");
                Environment.Exit(1);
            }
        }
        static void ExpectFalse(bool expression)
        {
            if (expression)
            {
                Console.WriteLine("Expected false, but got true");
                Environment.Exit(1);
            }
        }

        static int Main()
        {

            ExpectTrue(BatteryIsOk(25, 70, 0.7f));
            ExpectFalse(BatteryIsOk(50, 70, 0.7f));
            ExpectFalse(BatteryIsOk(-1, 70, 0.7f));
            ExpectFalse(BatteryIsOk(25, 10, 0.7f));
            ExpectFalse(BatteryIsOk(25, 90, 0.7f));
            ExpectFalse(BatteryIsOk(25, 70, 0.9f));
            Console.WriteLine("All checks completed");
            return 0;
        }
    }
}
