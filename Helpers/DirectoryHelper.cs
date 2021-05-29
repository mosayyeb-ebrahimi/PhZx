using System;
using System.IO;

namespace PhZx.Helpers
{
    public static class DirectoryHelper
    {
        public static string CreateThumbDirectory()
        {
            var dir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\PhZx\\Thumb";
            CreateDirectoryIfNotExist(dir);
            return dir;
        }

        public static string CreateImageDirectory(string name)
        {
            var dir = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures) + "\\PhZx\\" + name;
            CreateDirectoryIfNotExist(dir);
            return dir;
        }
        public static string CreateImageDirectory(string dir, string name)
        {

            dir = dir + "\\" + name;
            CreateDirectoryIfNotExist(dir);
            return dir;
        }

        private static void CreateDirectoryIfNotExist(string dir)
        {
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }
    }
}
