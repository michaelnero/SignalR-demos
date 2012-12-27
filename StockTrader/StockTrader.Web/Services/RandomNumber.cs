using System.Security.Cryptography;

namespace StockTrader.Web.Services {
    public static class RandomNumber {
        public static int GetRandomInt(int max) {
            byte randomNumber = GetRandomByte();

            int mod = (randomNumber % max);
            return mod;
        }

        public static double GetRandomDouble() {
            byte randomNumber = GetRandomByte();

            int mod = randomNumber % 10;
            double random = mod / 10D;
            return random;
        }

        public static decimal GetRandomDecimal() {
            byte randomNumber = GetRandomByte();

            int mod = randomNumber % 100;
            decimal random = mod / 100m;
            return random;
        }

        private static byte GetRandomByte() {
            using (var cryptoProvider = new RNGCryptoServiceProvider()) {
                byte[] randomNumber = new byte[1];
                cryptoProvider.GetBytes(randomNumber);

                return randomNumber[0];
            }
        }
    }
}