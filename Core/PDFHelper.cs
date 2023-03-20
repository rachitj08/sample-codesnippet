using iText.Html2pdf;
using System;
using System.IO;

namespace Core
{
    public static class PDFHelper
    {
        public static void GeneratePDF(string htmlText, string filePath)
        {
            //PdfWriter pdfFile = new PdfWriter(filePath);
            //HtmlConverter.ConvertToPdf(htmlText, pdfFile);

            using (var fileStream = File.Create(filePath))
            {
                
                HtmlConverter.ConvertToPdf(htmlText, fileStream);
                fileStream.Close();
            }
        }

        public static byte[] GetPDFBytes(string htmlText, string filePath)
        {
            using (var fileStream = File.Create(filePath))
            {

                HtmlConverter.ConvertToPdf(htmlText, fileStream);
                fileStream.Close();
                return File.ReadAllBytes(filePath);
            }
        }

    }
}
