using RabbitMQEventBus.Dtos;
using RabbitMQEventBus.Eventos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQEventBus.EventoQueue
{
    public class FacturaEventoQueue:Evento
    {
        public FacturaNubex Factura { get; set; }
        public FacturaAtowerDTO FacturaAtower { get; set; }
        public int? IdCompañiaNubex { get; set; }
        public int? IdCompañiaAtower { get; set; }
        public string? Base64Pdf { get; set; } = null;

        public FacturaEventoQueue(FacturaNubex factura, FacturaAtowerDTO facturaAtower, int? idCompañiaNubex,string? base64Pdf,int? idCompañiaAtower)
        {
            Factura = factura;
            FacturaAtower = facturaAtower;
            IdCompañiaNubex = idCompañiaNubex;
            Base64Pdf = base64Pdf;
            IdCompañiaAtower = idCompañiaAtower;
        }
    }
}
