namespace MoneyMind.Models
{
    public class StockPurchase
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Symbol { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal PurchasePrice { get; set; }
        public DateTime PurchaseDate { get; set; }
    }
}