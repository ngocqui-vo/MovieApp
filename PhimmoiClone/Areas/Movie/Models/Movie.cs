namespace PhimmoiClone.Areas.Movie.Models;

public class Movie
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public DateTime Publish { get; set; }
    public float Rating { get; set; }
    public List<Actor>? Actors { get; set; }
    public List<Genre>? Genres { get; set; }
}