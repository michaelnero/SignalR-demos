using System.Collections.Concurrent;

namespace VirusReplication.Services {
    public static class CalculatorRegistry {
        private static readonly ConcurrentDictionary<string, GenerationCalculator> calculators = new ConcurrentDictionary<string, GenerationCalculator>();

        public static void Register(GenerationCalculator calculator) {
            calculators.TryAdd(calculator.ConnectionID, calculator);
        }

        public static void Remove(string connectionID) {
            GenerationCalculator calculator;
            if (calculators.TryRemove(connectionID, out calculator)) {
                calculator.Stop();
            }
        }
    }
}