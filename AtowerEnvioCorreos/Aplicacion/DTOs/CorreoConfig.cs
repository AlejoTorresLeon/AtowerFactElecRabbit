using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtowerEnvioCorreos.Aplicacion.DTOs
{
    public class CorreoConfig
    {
        public string? ServidorSmtp { get; set; }
        public int? PuertoSmtp { get; set; }
        public string? Usuario { get; set; }
        public string? Contraseña { get; set; }
    }
}
