namespace ShoppingCartApi.ViewModels
{
    public class CartViewModel
    {
        public string CartId { get; set; }
        public int AlbumId { get; set; }
        public int GenreId { get; set; }
        public int ArtistId { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string ShoppingCartId { get; set; }
    }
    public class CartDeleteViewModel
    {
        public string ShoppingCartId { get; set; }
        public int RecordId { get; set; }
    }
}