using System.ComponentModel.DataAnnotations;

namespace PustokTemp.Models;

public class Checkout:BaseEntity
{
    [Required]
    [StringLength(100)]
    public string FirstName { get; set; }
    [Required]
    [StringLength(100)]
    public string LastName { get; set; }
    [Required]
    [StringLength(100)]
    public string Country { get; set; }
    [Required]
    [StringLength(100)]
    public string Email { get; set; }
    [Required]
    [StringLength(20)]
    public string PhoneNo { get; set; }
    [Required]
    [StringLength(100)]
    public string Address1 { get; set; }
    [Required]
    [StringLength(100)]
    public string Address2 { get; set; }
    [Required]
    [StringLength(100)]
    public string City { get; set; }
    [Required]
    [StringLength(100)]
    public string State { get; set; }
    public double TotalPrice { get; set; }
    public List<CheckoutBook>? CheckoutBooks { get; set; }
    public bool IsApproved { get; set; }
}
