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
        public int? IdCompañia { get; set; }

        public FacturaEventoQueue(FacturaNubex factura, int? idCompañia)
        {
            Factura = factura;
            IdCompañia = idCompañia;
        }
    }
}
