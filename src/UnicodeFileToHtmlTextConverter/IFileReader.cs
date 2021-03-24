using System.Collections.Generic;

namespace TDDMicroExercises.UnicodeFileToHtmlTextConverter
{
    /*
     * solid violations:
     * - DIP: working against the file system directly, also depending directly over httpUtilities
     * - O/C: no way to modify what it's doing with the file from outside
     * - SRP: reading file + formatting
     *
     */

    public interface IFileReader
    {
        string FileNameWithPath { get; }

        IReadOnlyList<string> ReadLines();
    }
}