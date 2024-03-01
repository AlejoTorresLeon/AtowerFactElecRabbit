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
        public string Asunto { get; set; }
        public string Cuerpo { get; set; }
        public string ServidorSmtp { get; set; }
        public int PuertoSmtp {  get; set; }
        public string UsuarioEmail { get; set; }
        public string PasswordEmail { get; set; }
        public EmailEventoQueue(string detinatario, string asunto, string cuerpo,int puertoSmtp, string usuarioEmail, string passwordEmail)
        {
            Detinatario = detinatario;
            Asunto = asunto;
            Cuerpo = cuerpo;
            PuertoSmtp = puertoSmtp;
            UsuarioEmail = usuarioEmail;
            PasswordEmail = passwordEmail;
        }
    }
}
