using AtowerDocElectronico.Aplicacion.Interfaces;
using RabbitMQEventBus.BusRabbit;
using RabbitMQEventBus.EventoQueue;

namespace AtowerDocElectronico.Aplicacion.Services.Rabbit
{
    public class CrearFacturaAtowerEventoManejador:IEventoManejador<CrearFacturaAtowerEventoQueue>
    {
        private readonly ILogger<CrearFacturaAtowerEventoManejador> _logger;
        private readonly IFacturaAtower _crearFacturaAtower;
        

        public CrearFacturaAtowerEventoManejador(ILogger<CrearFacturaAtowerEventoManejador> logger, IFacturaAtower crearFacturaAtower) 
        {
            _logger = logger;
            _crearFacturaAtower = crearFacturaAtower;
        }
        public CrearFacturaAtowerEventoManejador() { }

        public async Task Handle(CrearFacturaAtowerEventoQueue @event)
        {
            await _crearFacturaAtower.ActualizarFacturasEnviadas(@event.FacturaAtower, @event.FacturaNubex,@event.ResponseNubex,@event.IdCompany, @event.Base64Pdf);

            await Task.CompletedTask;
            return;
        }
    }
}
