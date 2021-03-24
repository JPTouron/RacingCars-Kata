using System.Collections.Generic;
using System.IO;

namespace TDDMicroExercises.UnicodeFileToHtmlTextConverter
{
    /// <summary>
    /// a simple stub, a representation of a possible implementation to test the converter
    /// </summary>
    internal class FileReaderStub : IFileReader
    {
        public FileReaderStub(string fileNameWithPath)
        {
            FileNameWithPath = fileNameWithPath;
        }

        public string FileNameWithPath { get; }

        public IReadOnlyList<string> ReadLines()
        {
            var lines = new List<string>();
            using (TextReader unicodeFileStream = File.OpenText(FileNameWithPath))
            {
                var line = unicodeFileStream.ReadLine();
                lines.Add(line);

                while (line != null)
                {
                    line = unicodeFileStream.ReadLine();
                    lines.Add(line);
                }
            }

            return lines;
        }
    }
}