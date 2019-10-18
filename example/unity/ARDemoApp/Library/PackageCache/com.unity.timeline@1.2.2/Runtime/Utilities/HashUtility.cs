namespace UnityEngine.Timeline
{
    static class HashUtility
    {
        // Note. We could have used "params int[] hashes" but we want to avoid allocating.

        public static int CombineHash(this int h1, int h2)
        {
            return h1 ^ (int)(h2 + 0x9e3779b9 + (h1 << 6) + (h1 >> 2)); // Similar to c++ boost::hash_combine
        }

        public static int CombineHash(int h1, int h2, int h3)
        {
            return CombineHash(h1, h2).CombineHash(h3);
        }

        public static int CombineHash(int h1, int h2, int h3, int h4)
        {
            return CombineHash(h1, h2, h3).CombineHash(h4);
        }

        public static int CombineHash(int h1, int h2, int h3, int h4, int h5)
        {
            return CombineHash(h1, h2, h3, h4).CombineHash(h5);
        }

        public static int CombineHash(int h1, int h2, int h3, int h4, int h5, int h6)
        {
            return CombineHash(h1, h2, h3, h4, h5).CombineHash(h6);
        }

        public static int CombineHash(int h1, int h2, int h3, int h4, int h5, int h6, int h7)
        {
            return CombineHash(h1, h2, h3, h4, h5, h6).CombineHash(h7);
        }

        public static int CombineHash(int[] hashes)
        {
            if (hashes == null || hashes.Length == 0)
                return 0;

            var h = hashes[0];
            for (int i = 1; i < hashes.Length; ++i)
            {
                h = CombineHash(h, hashes[i]);
            }

            return h;
        }
    }
}
