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
        private readonly IHttpUtility httpUtility;

        public UnicodeFileToHtmlTextConverter(IFileReader fileReader, IHttpUtility httpUtility)
        {
            this.fileReader = fileReader;
            this.httpUtility = httpUtility;
        }

        public string ConvertToHtml()
        {
            var lines = fileReader.ReadLines();

            string html = string.Empty;
            foreach (var line in lines)
            {
                if (line != null)
                {
                    html += httpUtility.HtmlEncode(line);
                    html += "<br />";
                }
            }

            return html;
        }

        public string GetFilename() => fileReader.FileNameWithPath;
    }
}