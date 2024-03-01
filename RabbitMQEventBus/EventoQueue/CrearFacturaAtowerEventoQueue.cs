
using RabbitMQEventBus.Dtos;
using RabbitMQEventBus.Eventos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RabbitMQEventBus.EventoQueue
{
    public class CrearFacturaAtowerEventoQueue : Evento
    {
        public FacturaAtowerDTO FacturaAtower { get; set; }
        public FacturaNubex FacturaNubex { get; set; }
        public int IdCompany { get; set; }
        public string? Base64Pdf { get; set; } = null;
        public ResponseSimplificadoDian? ResponseNubex { get; set; }

        public CrearFacturaAtowerEventoQueue(FacturaAtowerDTO facturaAtower, FacturaNubex facturaNubex,int idCompany, string? base64Pdf, ResponseSimplificadoDian? responseNubex)
        {
            FacturaAtower = facturaAtower;
            FacturaNubex = facturaNubex;
            IdCompany = idCompany;
            Base64Pdf = base64Pdf;
            ResponseNubex = responseNubex;
        }
    }
}
