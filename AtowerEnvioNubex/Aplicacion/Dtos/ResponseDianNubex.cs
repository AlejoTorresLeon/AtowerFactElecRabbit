namespace AtowerEnvioNubex.Aplicacion.Dtos
{
    public class ResponseDianNubex
    {
        public string message { get; set; }
        public bool send_email_success { get; set; }
        public bool send_email_date_time { get; set; }
        public ResponseDianDto ResponseDian { get; set; }
        public string invoicexml { get; set; }
        public string zipinvoicexml { get; set; }
        public string unsignedinvoicexml { get; set; }
        public string reqfe { get; set; }
        public string rptafe { get; set; }
        public string attacheddocument { get; set; }
        public string urlinvoicexml { get; set; }
        public string urlinvoicepdf { get; set; }
        public string urlinvoiceattached { get; set; }
        public string cufe { get; set; }
        public string QRStr { get; set; }
        public int certificate_days_left { get; set; }
        public int resolution_days_left { get; set; }
    }

    public class ResponseDianDto
    {
        public EnvelopeDto Envelope { get; set; }
    }

    public class EnvelopeDto
    {
        public HeaderDto Header { get; set; }
        public BodyDto Body { get; set; }
    }

    public class HeaderDto
    {
        public ActionDto Action { get; set; }
        public SecurityDto Security { get; set; }
    }

    public class ActionDto
    {
        public AttributesDto _attributes { get; set; }
        public string _value { get; set; }
    }

    public class SecurityDto
    {
        public AttributesDto _attributes { get; set; }
        public TimestampDto Timestamp { get; set; }
    }

    public class TimestampDto
    {
        public AttributesDto _attributes { get; set; }
        public DateTime Created { get; set; }
        public DateTime Expires { get; set; }
    }

    public class BodyDto
    {
        public SendBillSyncResponseDto SendBillSyncResponse { get; set; }
    }

    public class SendBillSyncResponseDto
    {
        public ErrorMessageDto ErrorMessage { get; set; }
        public string IsValid { get; set; }
        public string StatusCode { get; set; }
        public string StatusDescription { get; set; }
        public string StatusMessage { get; set; }
        public string XmlBase64Bytes { get; set; }
        public AttributesDto XmlBytes { get; set; }
        public string XmlDocumentKey { get; set; }
        public string XmlFileName { get; set; }
        public SendBillSyncResultDto SendBillSyncResult { get; set; } // Agregado SendBillSyncResult
    }

    public class SendBillSyncResultDto
    {
        public ErrorMessageDto ErrorMessage { get; set; }
        public string IsValid { get; set; }
        public string StatusCode { get; set; }
        public string StatusDescription { get; set; }
        public string StatusMessage { get; set; }
        public string XmlBase64Bytes { get; set; }
        public AttributesDto XmlBytes { get; set; }
        public string XmlDocumentKey { get; set; }
        public string XmlFileName { get; set; }
    }

    public class ErrorMessageDto
    {
        public List<string> @string { get; set; }
    }

    public class AttributesDto
    {
        public string nil { get; set; }
    }
}
