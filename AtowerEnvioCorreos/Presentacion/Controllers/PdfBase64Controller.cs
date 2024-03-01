using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.IO.Image;
using iText.Kernel.Pdf.Canvas;

namespace AtowerEnvioCorreos.Presentacion.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PdfBase64Controller : ControllerBase
{
    // POST api/<PdfBase64Controller>
    [HttpPost]
    public IActionResult Post([FromBody] PdfFirmaRequest request, int posicionx, int posiciony)
    {
        try
        {
            // Decodificar las cadenas Base64 del PDF y de la firma
            byte[] pdfBytes = Convert.FromBase64String(request.PdfBase64);
            byte[] firmaBytes = Convert.FromBase64String(request.FirmaBase64);

            // Agregar la firma al PDF
            byte[] pdfConFirmaBytes = AgregarFirmaAPDF(pdfBytes, firmaBytes, posicionx, posiciony);

            // Devolver el PDF con la firma en formato Base64
            //string pdfConFirmaBase64 = Convert.ToBase64String(pdfConFirmaBytes);

            // Devolver el PDF como respuesta directa
            return File(pdfConFirmaBytes, "application/pdf", "documento_firmado.pdf");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error al agregar firma al PDF: {ex.Message}");
        }
    }

    private byte[] AgregarFirmaAPDF(byte[] pdfBytes, byte[] firmaBytes, int posicionx, int posiciony)
    {
        using (MemoryStream ms = new MemoryStream())
        {
            using (MemoryStream pdfStream = new MemoryStream(pdfBytes))
            {
                PdfReader reader = new PdfReader(pdfStream);
                PdfWriter writer = new PdfWriter(ms);
                PdfDocument pdfDoc = new PdfDocument(reader, writer);

                // Obtener el número total de páginas
                int numeroDePaginas = pdfDoc.GetNumberOfPages();

                // Acceder a la última página
                PdfPage page = pdfDoc.GetPage(numeroDePaginas);

                // Crear la imagen de la firma
                ImageData imageData = ImageDataFactory.Create(firmaBytes);
                Image firma = new Image(imageData);

                // Establecer la posición de la firma en la última página
                firma.SetFixedPosition(posicionx, posiciony); // Ajusta la posición según sea necesario

                // Agregar la imagen al documento
                PdfCanvas pdfCanvas = new PdfCanvas(page);
                Canvas canvas = new Canvas(pdfCanvas, page.GetPageSize());
                canvas.Add(firma);

                pdfDoc.Close();
            }

            return ms.ToArray();
        }
    }


    // Clase para el modelo de solicitud
    public class PdfFirmaRequest
    {
        public string PdfBase64 { get; set; }
        public string FirmaBase64 { get; set; }
    }
}
