namespace ERP.Services.API.Models.ResponseModels.Customer
{
    public class CustomerResponse
    {
        public Guid? CusId { get; set; }
        public string? CusCustomId { get; set; }
        public Guid? BusinessId { get; set; }
        public string? CusType { get; set; }
        public string? CusName { get; set; }
        public string? CusNameEng { get; set; }
        public string? DisplayName { get; set; }
        public string? TaxId { get; set; }
        public string? BrnId { get; set; }
        public string? Building { get; set; }
        public string? RoomNo { get; set; }
        public string? Floor { get; set; }
        public string? Village { get; set; }
        public string? Moo { get; set; }
        public string? No { get; set; }
        public string? Road { get; set; }
        public string? Alley { get; set; }
        public string? Province { get; set; }
        public string? District { get; set; }
        public string? SubDistrict { get; set; }
        public string? PostCode { get; set; }
        public string? Website { get; set; }
        public string? CusStatus { get; set; }
        public bool? IsApprove { get; set; }
     
    }
}
