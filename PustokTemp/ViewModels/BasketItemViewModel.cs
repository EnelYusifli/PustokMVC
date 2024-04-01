using PustokTemp.Models;

namespace PustokTemp.ViewModels;

public class BasketItemViewModel
{
    public int BookId { get; set; }
    public int Count { get; set; }
    public List<Book>? Books { get; set; }
}
