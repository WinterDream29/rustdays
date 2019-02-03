#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("wN8j2b8mF+zap7sKFf+bzpd9tTK0Nzk2BrQ3PDS0Nzc2rJZf1C/b6QTj+gImbYZqoQFCmssIBNearAuN6/qt7xKmLKTlgf7sdko4LSR7E3926e62Wvz0mdeWLhVzx0+1gAGDCcADJHnYNWFnWa79ljIPEF4c9IvshffCWV1WMRkFFhBV3AXbwRXI/JYuLymeG6iQiTaLVEYfozQhBJR9VQa0NxQGOzA/HLB+sME7Nzc3MzY1Gmhv9FYzdhmktSL8JTdv4rbznj24mh0VHCzJEY7f6eBOTqVT/ZXMr+36SQWJ7uw0FhCbgzZbLcN4CJIRI0Wuxj7q2Bk38uSFG/TsFOT/IS7IgDY77yN8MiprSGVnZWXHVPklGGZJ8S5EJShSiTQ1NzY3");
        private static int[] order = new int[] { 6,1,13,12,4,12,8,10,8,13,13,13,12,13,14 };
        private static int key = 54;

        public static byte[] Data() {
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif
