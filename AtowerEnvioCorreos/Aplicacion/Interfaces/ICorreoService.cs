﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtowerEnvioCorreos.Aplicacion.Interfaces
{
    public interface ICorreoService
    {
        Task EnviarCorreoAsync(string destinatario);
    }
}
