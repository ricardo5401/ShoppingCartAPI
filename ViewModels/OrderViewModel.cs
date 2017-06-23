namespace ShoppingCartApi.ViewModels
{
    public class OrderViewModel
    {
        public int OrderId { get; set; }
        public string ShoppingCartId { get; set; }
    }

    public class OrderPromoViewModel
    {
        public int OrderId { get; set; }
        public string ShoppingCartId { get; set; }
        public string PromoCode { get; set; }
    }
}