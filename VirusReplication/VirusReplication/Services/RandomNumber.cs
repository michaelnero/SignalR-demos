using System.Security.Cryptography;

namespace VirusReplication.Services {
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

        private static byte GetRandomByte() {
            using (var cryptoProvider = new RNGCryptoServiceProvider()) {
                byte[] randomNumber = new byte[1];
                cryptoProvider.GetBytes(randomNumber);

                return randomNumber[0];
            }
        }
    }
}