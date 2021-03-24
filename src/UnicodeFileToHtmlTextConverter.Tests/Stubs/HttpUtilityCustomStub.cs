namespace TDDMicroExercises.UnicodeFileToHtmlTextConverter.Tests.Stubs
{
    /// <summary>
    /// A possible implementation for the IHttpUtility
    /// </summary>
    internal class HttpUtilityCustomStub : IHttpUtility
    {
        public string HtmlEncode(string line)
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