namespace ExBookapi.DTOs;

public class CustomerDTO
{
    public int CustomerID { get; set; }
    public string FullName { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime Registration { get; set; }
}