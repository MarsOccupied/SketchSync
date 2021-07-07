namespace SketchSync.Models;

public enum DrawingType
{
    Freehand,
    Rectangle,
    Circle,
    Line,
    Text,
    StickyNote,
    Emoji,
    Eraser
}

public class DrawingObject
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public DrawingType Type { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string Color { get; set; } = "#000000";
    public int StrokeWidth { get; set; } = 2;
    public List<Point> Points { get; set; } = new();
    public double X { get; set; }
    public double Y { get; set; }
    public double Width { get; set; }
    public double Height { get; set; }
    public string Text { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public bool IsVisible { get; set; } = true;
}

public class Point
{
    public double X { get; set; }
    public double Y { get; set; }
}

