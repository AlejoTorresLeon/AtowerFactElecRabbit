using AtowerEnvioCorreos.Aplicacion.Interfaces;
using RabbitMQEventBus.BusRabbit;
using RabbitMQEventBus.EventoQueue;

namespace AtowerEnvioCorreos.ManejadorRabbit
{
    public class EmailEventoManejador : IEventoManejador<EmailEventoQueue>
    {
        private readonly ILogger<EmailEventoManejador> _logger;
        private readonly ICorreoService _correoService;
        public EmailEventoManejador(ILogger<EmailEventoManejador> logger, ICorreoService correoService)
        {
            _logger = logger;
            _correoService = correoService;
        }
        public EmailEventoManejador() { }


        public async Task Handle(EmailEventoQueue @event)
        {

            await _correoService.EnviarCorreoAsync(@event.Detinatario);
                

        }
    }
}
