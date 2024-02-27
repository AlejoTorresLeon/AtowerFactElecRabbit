using RabbitMQEventBus.Eventos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQEventBus.EventoQueue
{
    public class EmailEventoQueue:Evento
    {
        public string Detinatario { get; set; }
        public string Titulo { get; set; }

        public string Contenido { get; set; }

        public EmailEventoQueue(string detinatario, string titulo, string contenido)
        {
            Detinatario = detinatario;
            Titulo = titulo;
            Contenido = contenido;
        }
    }
}
