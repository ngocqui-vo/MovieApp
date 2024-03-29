﻿using System.ComponentModel.DataAnnotations;
using MovieApp.Areas.Identity.Models;

namespace MovieApp.Areas.Cinema.Models;

public class Comment
{
    [Key]
    public int Id { get; set; }
    public string? UserId { get; set; }
    public int MovieId { get; set; }
    public Movie? Movie { get; set; }
    public AppUser? User { get; set; }
    public string? Message { get; set; }
    public DateTime Created { get; set; } = DateTime.Now;
}