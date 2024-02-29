using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace AtowerDocElectronico.Presentacion.Controllers.Factura
{
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
                iTextSharp.text.pdf.PdfReader reader = new iTextSharp.text.pdf.PdfReader(pdfBytes);
                using (PdfStamper stamper = new PdfStamper(reader, ms))
                {
                    // Obtener el número total de páginas
                    int numeroDePaginas = reader.NumberOfPages;

                    // Acceder a la última página
                    PdfContentByte canvas = stamper.GetOverContent(numeroDePaginas);

                    // Crear la imagen de la firma
                    Image firma = Image.GetInstance(firmaBytes);

                    // Establecer la posición de la firma en la última página
                    firma.SetAbsolutePosition(posicionx, posiciony); // Ajusta la posición según sea necesario

                    // Agregar la firma a la última página
                    canvas.AddImage(firma);
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
}
