using RabbitMQEventBus.Comandos;
using RabbitMQEventBus.Eventos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQEventBus.BusRabbit
{
    public interface IRabbitEventBus
    {
        Task EnviarComando<T>(T Comando) where T : Comando;

        void Publish<T>(T @evento) where T: Evento;

        void Subscribe<T, TH>() where T: Evento
                                where TH : IEventoManejador<T>;
    }
}
