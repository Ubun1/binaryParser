using System;
using System.Windows;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;

namespace JonesWPF
{
    static class FolderManager
    {
        static List<string> filePaths = new List<string>();
        
        public static List<string> GetFilesPaths(string selectedPath)
        {
            string[] tmpFilePaths;

            var directories = new List<string>();
            
            tmpFilePaths = Directory.GetFiles(selectedPath, "*.prn", SearchOption.AllDirectories);
            foreach (string path in tmpFilePaths)
                if (path.Contains("voac") && !path.Contains("init"))
                {
                    filePaths.Add(path);
                    directories.Add(Path.GetDirectoryName(path));
                }
            return directories.Union(directories).ToList();

        }

        public static List<string> ChoozeFilesFrom(string directory)
        {
            var currentFilePaths = new List<string>();

            foreach (var path in filePaths)
            {
                if (path.IndexOf(directory) == -1)
                    continue;
                currentFilePaths.Add(path);
            }
            return currentFilePaths;
        }
    }
}
