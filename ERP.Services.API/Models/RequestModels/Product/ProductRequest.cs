﻿namespace ERP.Services.API.Models.RequestModels.Product
{
    public class ProductRequest
    {
        public Guid? ProductId { get; set; }
        public Guid? OrgId { get; set; }
        public Guid? BusinessId { get; set; }
        public string? ProductCustomId { get; set; }
        public string? ProductName { get; set; }
        public decimal? MSRP { get; set; }
        public decimal? LwPrice { get; set; }
        public string? ProductStatus { get; set; }
    }
}
