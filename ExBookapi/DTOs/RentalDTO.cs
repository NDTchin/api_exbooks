namespace ExBookapi.DTOs;

public class RentalDTO
{
    public int RentalID { get; set; }
    public int CustomerID { get; set; }
    public DateTime RentalDate { get; set; }
    public DateTime ReturnDate { get; set; }
    public string Status { get; set; }
    public List<RentalDetailDTO> RentalDetails { get; set; }
}