using System;
using System.Linq;
using Xunit;

namespace TDDMicroExercises.UnicodeFileToHtmlTextConverter
{
    public class HikerTest
    {
        [Fact]
        public void Foobar()
        {
            const string fileNameWithPath = "./foobar.txt";
            var fileReader = CreateFileReaderStub(fileNameWithPath);

            UnicodeFileToHtmlTextConverter converter = new UnicodeFileToHtmlTextConverter(fileReader);

            Assert.Equal(fileNameWithPath, converter.GetFilename());
        }

        [Theory]
        [InlineData(new object[] { "foobar.txt" })]
        [InlineData(new object[] { "foobar1.txt" })]
        [InlineData(new object[] { "foobar2.txt" })]
        [InlineData(new object[] { "foobar3.txt" })]
        public void Foobar2(string fileName)
        {
            string fileNameWithPath = $"./UnicodeFileToHtmlTextConverter.Tests/{fileName}";
            var fileReader = CreateFileReaderStub(fileNameWithPath);

            var expectedCount = 0;

            using (var fr = new System.IO.StreamReader(fileNameWithPath))
            {
                var line = fr.ReadLine();
                while (line != null)
                {
                    expectedCount++;
                    line = fr.ReadLine();
                }

                expectedCount++;
            }

            UnicodeFileToHtmlTextConverter converter = new UnicodeFileToHtmlTextConverter(fileReader);

            var result = converter.ConvertToHtml();

            var actualCount = result.Split("<br />").Length;

            Assert.Equal(expectedCount, actualCount);
        }

        [Theory]
        [InlineData(new object[] { "foobar4.txt", new char[] { '<', '>', '&', '\"', '\'' },
                                                  new string[] { "&amp;lt;", "&amp;gt;", "&amp;", "&quot;", "&quot;" } })]
        public void Foobar3(string fileName, char[] availableChars, string[] expectedEncodings)
        {
            string fileNameWithPath = $"./UnicodeFileToHtmlTextConverter.Tests/{fileName}";
            var fileReader = CreateFileReaderStub(fileNameWithPath);
            var contents = "";

            using (var fr = new System.IO.StreamReader(fileNameWithPath))
                contents = fr.ReadToEnd();

            UnicodeFileToHtmlTextConverter converter = new UnicodeFileToHtmlTextConverter(fileReader);

            var result = converter.ConvertToHtml();

            var lines = contents.Split("\r\n");

            foreach (var line in lines)
            {
                var theChar = availableChars.Single(x => x == Convert.ToChar(line));

                var charIdx = availableChars.ToList().FindIndex(x => theChar == x);

                var expectedEncoding = expectedEncodings[charIdx];

                var actualEncoding = result.Split("<br />")[charIdx];

                Assert.Equal(expectedEncoding, actualEncoding);
            }
        }

        private FileReaderStub CreateFileReaderStub(string fileNameWithPath)
        {
            return new FileReaderStub(fileNameWithPath);
        }
    }
}