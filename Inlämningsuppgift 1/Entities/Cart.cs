namespace Inlämningsuppgift_1.Entities
{
    public class Cart
    {
        public int UserId { get; set; }
        //public IEnumerable<CartItem>? CartItems { get; set; }
        public List<CartItem>? CartItems { get; set; }
    }
}
