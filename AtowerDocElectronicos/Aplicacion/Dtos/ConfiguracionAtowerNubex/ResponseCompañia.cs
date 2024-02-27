namespace AtowerDocElectronico.Aplicacion.Dtos.ConfiguracionAtowerNubex
{
    public class CompanyResponseDTOs
    {
        public bool? success { get; set; }
        public string? message { get; set; }
        public string? password { get; set; }
        public string? token { get; set; }
        public CompanyDTOs? company { get; set; }
    }

    public class CompanyDTOs
    {
        public int? id { get; set; }
        public int? user_id { get; set; }
        public string? identification_number { get; set; }
        public string? dv { get; set; }
        public int? language_id { get; set; }
        public int? tax_id { get; set; }
        public int? type_environment_id { get; set; }
        public int? payroll_type_environment_id { get; set; }
        public int? sd_type_environment_id { get; set; }
        public int? type_operation_id { get; set; }
        public int? type_document_identification_id { get; set; }
        public int? country_id { get; set; }
        public int? type_currency_id { get; set; }
        public int? type_organization_id { get; set; }
        public int? type_regime_id { get; set; }
        public int? type_liability_id { get; set; }
        public int? municipality_id { get; set; }
        public string? merchant_registration { get; set; }
        public string? address { get; set; }
        public string? phone { get; set; }
        public string? password { get; set; }
        public string? newpassword { get; set; }
        public int? type_plan_id { get; set; }
        public int? type_plan2_id { get; set; }
        public int? type_plan3_id { get; set; }
        public int? type_plan4_id { get; set; }
        public DateTime? start_plan_date { get; set; }
        public DateTime? start_plan_date2 { get; set; }
        public DateTime? start_plan_date3 { get; set; }
        public DateTime? start_plan_date4 { get; set; }
        public DateTime? absolut_start_plan_date { get; set; }
        public int? state { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public UserDTOs? user { get; set; }
        public List<object>? send { get; set; }
    }
    public class UserDTOs
    {
        public int? id { get; set; }
        public string? name { get; set; }
        public string? email { get; set; }
        public object? email_verified_at { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public object? id_administrator { get; set; }
        public object? mail_host { get; set; }
        public object? mail_port { get; set; }
        public object? mail_username { get; set; }
        public object? mail_password { get; set; }
        public object? mail_encryption { get; set; }
    }

    public class ApiErrorResponse
    {
        public string? Message { get; set; }
        public Dictionary<string, List<string>>? Errors { get; set; }
    }
}
