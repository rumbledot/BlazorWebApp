using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using WebAppBlazor.Data;

namespace WebAppBlazor.JSI
{
    //based on this tutorial https://www.youtube.com/watch?v=gUwUSbTR1D8&list=WL&index=1
    public class PDFGenerator
    {
        public void DownloadPDF(IJSRuntime js, string filename, byte[] pdf_binary)
        {
            js.InvokeVoidAsync("DownloadPDF",
                filename,
                Convert.ToBase64String(pdf_binary)
                );
        }

        public void ViewPDF(IJSRuntime js, string iFrame_id, byte[] pdf_binary)
        {
            js.InvokeVoidAsync("ViewPDF",
                iFrame_id,
                Convert.ToBase64String(pdf_binary)
                );
        }

        private byte[] PDFReport()
        {
            var memory_stream = new MemoryStream();

            float margin_left = 1.5f;
            float margin_right = 1.5f;
            float margin_top = 1.0f;
            float margin_bottom = 1.0f;

            Document pdf = new Document(
                PageSize.A4,
                margin_left.ToDPI(),
                margin_right.ToDPI(),
                margin_top.ToDPI(),
                margin_bottom.ToDPI()
                );

            pdf.AddTitle("Approval Module");
            pdf.AddAuthor("VIS");
            pdf.AddCreationDate();
            pdf.AddKeywords("Invoice");
            pdf.AddSubject("viKeyLess Invoice");

            PdfWriter writer = PdfWriter.GetInstance(pdf, memory_stream);

            var font_style = FontFactory.GetFont("Arial", 16, Color.WHITE);

            var label_hdr = new Chunk("Approval Module Invoice PDF", font_style);
            HeaderFooter header = new HeaderFooter(new Phrase(label_hdr), false)
            {
                BackgroundColor = new Color(50, 20, 120),
                Alignment = Element.ALIGN_CENTER,
                Border = Rectangle.NO_BORDER
            };
            pdf.Header = header;

            var label_footer = new Chunk("Page", font_style);
            HeaderFooter footer = new HeaderFooter(new Phrase(label_footer), true)
            {
                BackgroundColor = new Color(120, 3, 120),
                Alignment = Element.ALIGN_RIGHT,
                Border = Rectangle.NO_BORDER
            };
            pdf.Footer = footer;

            pdf.Open();

            var title = new Paragraph("Approval Module Report", new Font(Font.HELVETICA, 20, Font.BOLD));
            title.SpacingAfter = 18f;

            pdf.Add(title);

            font_style = FontFactory.GetFont("Tahoma", 12f, Font.NORMAL);

            var text = "PDF Viewer experiment";
            var phrase = new Phrase(text, font_style);
            pdf.Add(phrase);

            pdf.Close();

            return memory_stream.ToArray();
        }
    }
}
