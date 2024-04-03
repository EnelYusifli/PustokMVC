namespace PustokTemp.Models;

public class CheckoutBook:BaseEntity
{
    public int CheckoutId { get; set; }
    public int BookId { get; set; }
    public int Count { get; set; }
    public Book? Book { get; set; }
    public Checkout? Checkout { get; set; }
}
