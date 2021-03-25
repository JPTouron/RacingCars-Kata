using AutoFixture;
using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace TDDMicroExercises.UnicodeFileToHtmlTextConverter
{
    public class Refactored_UnicodeFileToHtmlTextConverterTest
    {
        [Fact]
        public void Foobar()
        {
            const string fileNameWithPath = "./foobar.txt";

            var enc = new Mock<IHttpUtility>();
            var fr = new Mock<IFileReader>();
            fr.SetupGet(x => x.FileNameWithPath).Returns(fileNameWithPath);

            var converter = new UnicodeFileToHtmlTextConverter(fr.Object, enc.Object);

            Assert.Equal(fileNameWithPath, converter.GetFilename());
        }

        [Theory]
        [ClassData(typeof(TextLinesProvider))]
        public void Foobar2(IEnumerable<string> lines)
        {
            var expectedCount = lines.Count(x => x != null);

            var enc = CreateEncoderMock();
            var fr = CreateFileReaderMock("./foobar.txt", lines.ToList());

            var converter = CreateTextConverter(enc, fr);

            var result = converter.ConvertToHtml();

            var actualCount = result.Split("<br />").Length - 1;

            Assert.Equal(expectedCount, actualCount);
        }

        //JP: This would become a test for the HttpUtility implementation that I'm really too lazy to implement... :-P
        //[Theory]
        //[InlineData(new object[] {  new string[] { "<", ">", "&", "\"", "\'" },
        //                            new char[] { '<', '>', '&', '\"', '\'' },
        //                            new string[] { "&amp;lt;", "&amp;gt;", "&amp;", "&quot;", "&quot;" } })]
        //public void Foobar3(string[] fileContents, char[] availableChars, string[] expectedEncodings)
        //{
        //    var fileReader = CreateFileReaderMock("", null);
        //    var httpEncoder = CreateEncoderMock();

        //    var contents = "";

        //    using (var fr = new System.IO.StreamReader(""))
        //        contents = fr.ReadToEnd();

        //    UnicodeFileToHtmlTextConverter converter = new UnicodeFileToHtmlTextConverter(fileReader.Object, httpEncoder.Object);

        //    var result = converter.ConvertToHtml();

        //    var lines = contents.Split("\r\n");

        //    foreach (var line in lines)
        //    {
        //        var theChar = availableChars.Single(x => x == Convert.ToChar(line));

        //        var charIdx = availableChars.ToList().FindIndex(x => theChar == x);

        //        var expectedEncoding = expectedEncodings[charIdx];

        //        var actualEncoding = result.Split("<br />")[charIdx];

        //        Assert.Equal(expectedEncoding, actualEncoding);
        //    }
        //}

        private Mock<IHttpUtility> CreateEncoderMock()
        {
            return new Mock<IHttpUtility>();
        }

        private Mock<IFileReader> CreateFileReaderMock(string fileNameWithPath, List<string> lines)
        {
            var fr = new Mock<IFileReader>();
            fr.SetupGet(x => x.FileNameWithPath).Returns(fileNameWithPath);
            fr.Setup(x => x.ReadLines()).Returns(lines);

            return fr;
        }

        private UnicodeFileToHtmlTextConverter CreateTextConverter(Mock<IHttpUtility> enc, Mock<IFileReader> fr)
        {
            return new UnicodeFileToHtmlTextConverter(fr.Object, enc.Object);
        }

        private class TextLinesProvider : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                foreach (var testCaseData in ParagraphWithNoEmptyLinesAbove())
                    yield return testCaseData;

                foreach (var testCaseData in ParagraphWithEmptyLinesBelow())
                    yield return testCaseData;

                foreach (var testCaseData in ParagraphWithEmptyLinesAboveAndBelow())
                    yield return testCaseData;

                foreach (var testCaseData in EmptyParagraph())
                    yield return testCaseData;
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            private void AppendRandomEmptyLines(List<string> lines)
            {
                for (int i = 0; i < GetRandomNumber(1, 5); i++)
                    lines.Add(null);
            }

            private IEnumerable<object[]> EmptyParagraph()
            {
                yield return new object[] { new List<string>() { null } };
            }

            private int GetRandomNumber(int minimum, int maximum)
            {
                Random random = new Random();
                return random.Next(minimum, maximum);
            }

            private IEnumerable<object[]> ParagraphWithEmptyLinesAboveAndBelow()
            {
                var sb = new List<string>();

                AppendRandomEmptyLines(sb);

                WriteTenRandomLinesInStringBuilder(sb);

                AppendRandomEmptyLines(sb);

                yield return new object[] { sb };
            }

            private IEnumerable<object[]> ParagraphWithEmptyLinesBelow()
            {
                var sb = new List<string>();

                WriteTenRandomLinesInStringBuilder(sb);

                AppendRandomEmptyLines(sb);

                yield return new object[] { sb };
            }

            private IEnumerable<object[]> ParagraphWithNoEmptyLinesAbove()
            {
                var sb = new List<string>();

                AppendRandomEmptyLines(sb);

                WriteTenRandomLinesInStringBuilder(sb);

                yield return new object[] { sb };
            }

            private string SomeText()
            {
                return "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer vehicula bibendum arcu";
            }

            private void WriteTenRandomLinesInStringBuilder(List<string> lines)
            {
                IFixture f = new Fixture();
                for (int i = 0; i < 10; i++)
                {
                    var writeText = f.Create<bool>();

                    if (writeText)
                        lines.Add(SomeText());
                    else
                        lines.Add(null);
                }
            }
        }
    }
}