namespace SketchSync.Models;

public class User
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = string.Empty;
    public string Color { get; set; } = "#000000";
    public double CursorX { get; set; }
    public double CursorY { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime LastActivity { get; set; } = DateTime.UtcNow;
}

