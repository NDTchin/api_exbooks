namespace ExBookapi.DTOs;

public class RentalDetailDTO
{
    public int RentalDetailID { get; set; }
    public int ComicBookID { get; set; }
    public int Quantity { get; set; }
    public decimal PricePerDay { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
}