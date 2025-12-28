using System.IO;
using System.IO.Compression;

namespace Menedżer_notatek_i_rysunków.Persistence
{
    public class ZipExportService
    {
        public void ExportJsonToZip(string jsonPath, string zipPath)
        {
            if (!File.Exists(jsonPath))
                throw new FileNotFoundException("JSON file not found.", jsonPath);

            if (File.Exists(zipPath))
                File.Delete(zipPath);

            using (var zip = ZipFile.Open(zipPath, ZipArchiveMode.Create))
            {
                zip.CreateEntryFromFile(
                    jsonPath,
                    Path.GetFileName(jsonPath),
                    CompressionLevel.Optimal
                );
            }
        }

        public void ImportZip(string zipPath, string extractDirectory)
        {
            if (!File.Exists(zipPath))
                throw new FileNotFoundException("ZIP file not found.", zipPath);

            if (Directory.Exists(extractDirectory))
                Directory.Delete(extractDirectory, true);

            ZipFile.ExtractToDirectory(zipPath, extractDirectory);
        }
    }


}

