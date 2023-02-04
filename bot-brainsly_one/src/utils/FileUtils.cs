using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace bot_brainsly_one.src.utils
{
    public class FileUtils
    {
        public List<string> FoldersOnPathDirectory;
        public string BaseProjectDirectory;

        public FileUtils()
        {
            FoldersOnPathDirectory = AppDomain.CurrentDomain.BaseDirectory
                .Split(Path.DirectorySeparatorChar)
                .ToList();

            BaseProjectDirectory = string.Join
                (
                    Path.DirectorySeparatorChar.ToString(),
                    FoldersOnPathDirectory.GetRange(0, FoldersOnPathDirectory.Count - 4)
                );
        }

        public bool FileIsOpen(string fileName)
        {
            try
            {
                Stream stream = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.None);

                stream.Close();

                return false;
            }
            catch (Exception error)
            {
                return true;
            }
        }
    }
}
