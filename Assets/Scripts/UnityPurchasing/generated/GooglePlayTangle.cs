// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("ddxmsXjcpRObViHn3nxd4nAIZr9VE+nerdRoNWzvMGSrr7j52YoEIrgKiaq4hY6Bog7ADn+FiYmJjYiL/kcRv0mbd4t8CilJpvhhqz3qrb0KiYeIuAqJgooKiYmIZ7Ism+N+s1i4yglozbqrYzWoaXOx5ZRj9u+naspMjcT6q159vgbViNgv/nQcDTANYOlJOMhHVG+4yhrZGd6/B6AGU0zdyhjTLF3yTF66f60oXNPB6Snn7QbvsoduBG/zs+92qz5sZ611IY99U6xYtEjvBdKzKGYnCG+FTymyMuxc21L0WLmtN+SksH6EPzAmxVDB7ZlQCG+5re2KJ4FQj+FDym25dPW60bxeL0NGb53OAIzmW0XeMMY7iX8Scv1fYUGn3YqLiYiJ");
        private static int[] order = new int[] { 6,13,6,12,13,11,7,10,12,9,10,12,12,13,14 };
        private static int key = 136;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
