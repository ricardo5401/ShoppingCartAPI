using ShoppingCartApi.Models;

namespace ShoppingCartApi.ViewModels
{
    public class OrderViewModel
    {
        public int OrderId { get; set; }
        public string ShoppingCartId { get; set; }
    }

    public class OrderFormViewModel
    {
        public Order order { get; set; }
        public string shoppingCartId { get; set;}
        public string promoCode { get; set;}
    }

    public class OrderPromoViewModel
    {
        public int OrderId { get; set; }
        public string ShoppingCartId { get; set; }
        public string PromoCode { get; set; }
    }
}