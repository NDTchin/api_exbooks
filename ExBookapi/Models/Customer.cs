namespace ExBookapi.Models;

public class Customer
{
    public int CustomerID { get; set; }
    public string FullName { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime Registration { get; set; }
}