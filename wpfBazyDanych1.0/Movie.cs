namespace wpfBazyDanych1._0;

public class Movie
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int Year { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; }
}