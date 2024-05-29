using iTextSharp.text;
using iTextSharp.text.pdf;

namespace DekoBim.Models
{
    public class PdfHeaderFooter: PdfPageEventHelper
    {
        public override void OnEndPage(PdfWriter writer, Document document)
        {
            // Resimleri yükle
            Image leftImage = Image.GetInstance("");
            Image rightImage = Image.GetInstance("");

            // Resim boyutlarını ayarla
            leftImage.ScaleToFit(50f, 50f);
            rightImage.ScaleToFit(50f, 50f);

            // Resim konumlarını ayarla
            leftImage.SetAbsolutePosition(document.LeftMargin, document.PageSize.Height - 50f - document.TopMargin);
            rightImage.SetAbsolutePosition(document.PageSize.Width - 50f - document.RightMargin, document.PageSize.Height - 50f - document.TopMargin);

            // Resimleri sayfaya ekle
            writer.DirectContent.AddImage(leftImage);
            writer.DirectContent.AddImage(rightImage);
        }
    }
}
