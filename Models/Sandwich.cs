namespace GoodHamburgerAPI.Models
{
    public class Sandwich
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public decimal Price { get; set; }
    }
}