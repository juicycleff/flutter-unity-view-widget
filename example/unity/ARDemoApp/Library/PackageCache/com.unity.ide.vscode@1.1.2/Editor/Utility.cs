namespace VSCodeEditor
{
    public static class Utility
    {
        public static string FileNameWithoutExtension(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return "";
            }

            var indexOfDot = -1;
            var indexOfSlash = 0;
            for (var i = path.Length - 1; i >= 0; i--)
            {
                if (indexOfDot == -1 && path[i] == '.')
                {
                    indexOfDot = i;
                }
                if (indexOfSlash == 0 && path[i] == '/' || path[i] == '\\')
                {
                    indexOfSlash = i + 1;
                    break;
                }
            }

            if (indexOfDot == -1)
            {
                indexOfDot = path.Length - 1;
            }

            return path.Substring(indexOfSlash, indexOfDot - indexOfSlash);
        }
    }
}