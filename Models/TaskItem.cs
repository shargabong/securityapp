namespace securityapp.Models;

public class TaskItem
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string UserId { get; set; } = "";
    public AppUser? User { get; set; }
}