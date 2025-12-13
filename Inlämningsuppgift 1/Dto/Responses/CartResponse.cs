namespace Inlämningsuppgift_1.Dto.Responses
{
    public class CartResponse
    {
        public int UserId { get; set; }
        public List<CartItemResponse> Items { get; set; } = new();
        public decimal Total { get; set; }
    }
}
