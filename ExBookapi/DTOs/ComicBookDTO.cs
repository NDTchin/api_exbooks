namespace ExBookapi.DTOs;

public class ComicBookDTO
{
    public int ComicBookID { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public decimal PricePerDay { get; set; }
}