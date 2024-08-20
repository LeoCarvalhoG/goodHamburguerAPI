  namespace GoodHamburgerAPI.Models
  {  public class Order
    {
        public int Id { get; set; }
        public int? SandwichId { get; set; }
        public int? FriesId { get; set; }
        public int? SoftDrinkId { get; set; }
        public decimal TotalPrice { get; set; }
        public string? ErrorMessage { get; set; }
        public bool Success { get; internal set; }
    }
}