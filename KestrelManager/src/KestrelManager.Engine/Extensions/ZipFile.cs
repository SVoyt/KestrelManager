using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

namespace KestrelManager.Engine.Extensions
{
    public static class ZipArchiveExtensions
    {
        public static void ExtractToDirectory(this ZipArchive archive, string destinationDirectoryName, bool overwrite)
        {
            if (!overwrite)
            {
                archive.ExtractToDirectory(destinationDirectoryName);
                return;
            }
            foreach (ZipArchiveEntry file in archive.Entries)
            {
                string completeFileName = Path.Combine(destinationDirectoryName, file.FullName);
                if (!string.IsNullOrEmpty( Path.GetDirectoryName(file.FullName)))
                {// Assuming Empty for Directory
                    Directory.CreateDirectory(Path.GetDirectoryName(completeFileName));
                }

                file.ExtractToFile(completeFileName, true);

            }
        }
    }
}
