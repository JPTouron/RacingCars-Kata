namespace TDDMicroExercises.UnicodeFileToHtmlTextConverter
{
    /*
     * solid violations:
     * - DIP: working against the file system directly, also depending directly over httpUtilities
     * - O/C: no way to modify what it's doing with the file from outside
     * - SRP: reading file + formatting
     *
     */

    public class UnicodeFileToHtmlTextConverter
    {
        private readonly IFileReader fileReader;

        public UnicodeFileToHtmlTextConverter(IFileReader fileReader)
        {
            this.fileReader = fileReader;
        }

        public string ConvertToHtml()
        {
            var lines = fileReader.ReadLines();

            string html = string.Empty;
            foreach (var line in lines)
            {
                if (line != null)
                {
                    html += HttpUtility.HtmlEncode(line);
                    html += "<br />";
                }
            }

            return html;
        }

        public string GetFilename() => fileReader.FileNameWithPath;
    }

    internal class HttpUtility
    {
        public static string HtmlEncode(string line)
        {
            line = line.Replace("<", "&lt;");
            line = line.Replace(">", "&gt;");
            line = line.Replace("&", "&amp;");
            line = line.Replace("\"", "&quot;");
            line = line.Replace("\'", "&quot;");
            return line;
        }
    }
}