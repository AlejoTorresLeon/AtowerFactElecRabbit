namespace RabbitMQEventBus.Dtos
{
    public class Cliente
    {
        public string? Identificacion { get; set; }
        public int? Dv { get; set; }
        public string? Nombre { get; set; }
        public long? Telefono { get; set; }
        public string? Direccion { get; set; }
        public string? Correo { get; set; }
        public int? IdTipoDocumento { get; set; }
        public int? IdTipoOrganizacion { get; set; }
        public int? IdTipoRegimen { get; set; }
        public int? IdTipoResponsabilidad { get; set; }
        public int? IdCiudad { get; set; }
    }

    public class FormaPago
    {
        public int? IdFormaPago { get; set; }
        public int? IdMetodoPago { get; set; }
        public string? FechaPago { get; set; }
        public int? Duracion { get; set; }
    }

    public class DescuentoGeneral
    {
        public int? IdTipoDescuento { get; set; }
        public string? Descripcion { get; set; }
        public decimal? ValorDescuento { get; set; }
        public decimal? ValorBaseDescuento { get; set; }
    }

    public class ImpuestoCargo
    {
        public int? IdTipoImpuesto { get; set; }
        public decimal? ValorImpuesto { get; set; }
        public decimal? ValorBaseImpuesto { get; set; }
        public float? Porcentaje { get; set; }
    }

    public class Cargo
    {
        //public int? IdUnidadMedida { get; set; }
        public int? Cantidad { get; set; }
        public decimal? ValorCargo { get; set; }
        public string? Descripcion { get; set; }
        public string? Nota { get; set; }
        public string? Codigo { get; set; }
        public List<ImpuestoCargo>? ImpuestoCargo { get; set; }
        public decimal? ValorNeto { get; set; }
    }

    public class ImpuestoTotal
    {
        public int? IdTipoImpuesto { get; set; }
        public decimal? ValorImpuesto { get; set; }
        public float? Porcentaje { get; set; }
        public decimal? ValorBaseImpuesto { get; set; }
    }

    public class TotalesNeto
    {
        public decimal? TotalCargos { get; set; }
        //public decimal? TotalImpuestoIncluido { get; set; }
        public decimal? TotalDescuento { get; set; }
        public decimal? TotalPagar { get; set; }
    }

    public class FacturaAtowerDTO
    {
        public string Numero_factura { get; set; } = null!;
        public string Fecha { get; set; }
        public string? Hora { get; set; }
        public string Prefijo { get; set; } = null!;
        public bool? EnviarCorreo { get; set; }
        public string? DetalleGeneral { get; set; }
        public string? DetalleCabecera { get; set; }
        public string? DetallePieHoja { get; set; }
        public Cliente? Cliente { get; set; }
        public FormaPago? FormaPago { get; set; }
        public List<DescuentoGeneral>? DescuentosGenerales { get; set; }
        public List<Cargo>? Cargos { get; set; }
        public List<ImpuestoTotal>? ImpuestoTotales { get; set; }
        public TotalesNeto? TotalesNeto { get; set; }
    }
}
