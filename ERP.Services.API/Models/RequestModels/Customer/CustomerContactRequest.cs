﻿namespace ERP.Services.API.Models.RequestModels.Customer
{
    public class CustomerContactRequest
    {
        public Guid? CusConId { get; set; }
        public Guid? BusinessId { get; set; }
        public Guid? CusId { get; set; }
        public string? CusConFirstname { get; set; }
        public string? CusConLastname { get; set; }
        public string? TelNo { get; set; }
        public string? ExtentNo { get; set; }
        public string? MobileNo { get; set; }
        public string? Email { get; set; }
        public string? CusConStatus { get; set; }
    }
}
