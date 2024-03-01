using System.Text.RegularExpressions;

namespace AtowerDocElectronico.Aplicacion.Dtos.ConfiguracionAtowerNubex
{
    public class RegistrarSoftwareDTO
    {
        public string Id { get; set; }
        public int Pin { get; set; }
    }

    public class RegistrarCertificadoDTo
    {        
        public string CertificadoBase64 { get; set; }
        public string password { get; set; }
    }


    public class UnprocessableEntityException : Exception
    {
        public UnprocessableEntityException(string message) : base(message) { }
    }

    public class RegistrarResolucionDTo
    {
        private static readonly Regex fechaRegex = new Regex(@"^\d{4}-\d{2}-\d{2}$");

        public string Prefijo { get; set; }
        public string NumeroResolucion { get; set; }
        private string _fechaResolucion;
        public string FechaResolucion
        {
            get => _fechaResolucion;
            set
            {
                if (!fechaRegex.IsMatch(value))
                    throw new UnprocessableEntityException("La fecha debe estar en el formato AAAA-MM-DD");
                _fechaResolucion = value;
            }
        }
        public string ClaveTecnica { get; set; }
        public long RangoDesde { get; set; }
        public long RangoHasta { get; set; }
        private string _fechaInicio;
        public string FechaInicio
        {
            get => _fechaInicio;
            set
            {
                if (!fechaRegex.IsMatch(value))
                    throw new UnprocessableEntityException("La fecha debe estar en el formato AAAA-MM-DD");
                _fechaInicio = value;
            }
        }
        private string _fechaFin;
        public string FechaFin
        {
            get => _fechaFin;
            set
            {
                if (!fechaRegex.IsMatch(value))
                    throw new UnprocessableEntityException("La fecha debe estar en el formato AAAA-MM-DD");
                _fechaFin = value;
            }
        }
    }

    public class RegistrarCompanyResponseDtos
    {
        public string message { get; set; }
    }

    public class RegistrarResolucionNotasDTo
    {
        public string Prefijo { get; set; }
        public long RangoDesde { get; set; }
        public long RangoHasta { get; set; }


    }
}
