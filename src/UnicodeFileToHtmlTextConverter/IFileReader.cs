using System.Collections.Generic;

namespace TDDMicroExercises.UnicodeFileToHtmlTextConverter
{
    public interface IFileReader
    {
        string FileNameWithPath { get; }

        IReadOnlyList<string> ReadLines();
    }
}