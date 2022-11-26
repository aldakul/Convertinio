using iText.Kernel.Pdf.Canvas.Parser.Listener;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf;
using System.Text;

namespace Converter.Tests.FileConverter.Pdf.helpers
{
    public class PdfHelper
    {
        public string GetPdfContent(string uploadPath)
        {
            var pdfReader = new PdfReader(uploadPath);
            var pdfDocument = new PdfDocument(pdfReader);
            var stringBuilder = new StringBuilder();
            for (var i = 1; i <= pdfDocument.GetNumberOfPages(); ++i)
            {
                ITextExtractionStrategy simpleTextExtractionStrategy = new SimpleTextExtractionStrategy();
                var page = pdfDocument.GetPage(i);
                var textFromPage = PdfTextExtractor.GetTextFromPage(page, simpleTextExtractionStrategy);
                stringBuilder.Append(textFromPage);
            }
            pdfReader.Close();
            return stringBuilder.ToString();
        }
    }
}
