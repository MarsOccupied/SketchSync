namespace SketchSync.Models;

public class Room
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = string.Empty;
    public List<User> Users { get; set; } = new();
    public List<DrawingObject> DrawingObjects { get; set; } = new();
    public DateTime Created { get; set; } = DateTime.UtcNow;
    public bool IsPictionaryMode { get; set; } = false;
    public string? CurrentWord { get; set; }
    public DateTime? WordStartTime { get; set; }
    public int WordTimeLimit { get; set; } = 60; // seconds
}

