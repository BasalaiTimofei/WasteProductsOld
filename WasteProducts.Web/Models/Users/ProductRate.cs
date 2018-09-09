namespace WasteProducts.Web.Models.Users
{
    public class ProductRate
    {
        public string UserId { get; set; }

        public string ProductId { get; set; }

        public int Rating { get; set; }

        public string Description { get; set; }
    }
}