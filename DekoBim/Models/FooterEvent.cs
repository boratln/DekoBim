using iTextSharp.text.pdf.draw;
using iTextSharp.text.pdf;
using iTextSharp.text;

namespace DekoBim.Models
{
    public class FooterEvent : PdfPageEventHelper
    {
        private BaseFont _baseFont;
        public FooterEvent(BaseFont baseFont)
        {
            _baseFont = baseFont;
        }

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            
                PdfContentByte cb = writer.DirectContent;

                // Metin için ColumnText oluştur
                Phrase phrase = new Phrase("DEKO PROJE DANIŞMANLIK MÜHENDİSLİK İNŞAAT SANAYİ VE TİCARET LTD.ŞTİ.Türkali Mah. Ihlamurdere Cad. Altıntaş Sok. No: 17 Beşiktaş / İSTANBUL (212) 236 53 30 Pbx Faks : (212) 236 53 31 www.dekogroup.com.tr – info@dekogroup.com.tr", new Font(_baseFont, 12));

                // Phrase'i içeren Paragraph oluştur ve hizalamayı ayarla
                Paragraph paragraph = new Paragraph();
                paragraph.Add(phrase);
                paragraph.Alignment = Element.ALIGN_CENTER; // Metni ortala

                // ColumnText ile metni sayfaya ekle
                ColumnText ct = new ColumnText(writer.DirectContent);
                ct.AddElement(paragraph); // Paragraph'ı ColumnText'e ekle

                // Metni sayfanın altına yerleştir
                float bottomMarginSpace = 80; // Metnin ve sayfanın altı arasındaki boşluk
                ct.SetSimpleColumn(document.LeftMargin,
                                   document.BottomMargin + bottomMarginSpace,
                                   document.PageSize.Width - document.RightMargin,
                                   document.BottomMargin,
                                   15, // Leading (satır aralığı)
                                   Element.ALIGN_CENTER); // ColumnText için de hizalamayı ayarla

                ct.Go(); // ColumnText'i çiz

                // Metnin üstünde çizgi çiz
                float lineY = document.BottomMargin + bottomMarginSpace + phrase.Font.Size + 5; // Metnin üstündeki çizgi için yükseklik
                cb.MoveTo(document.LeftMargin, lineY);
                cb.LineTo(document.PageSize.Width - document.RightMargin, lineY);
                cb.Stroke();
            


        }
    }
}
