using SketchSync.Models;

namespace SketchSync.Services;

public class RoomService
{
    private readonly Dictionary<string, Room> _rooms = new();
    private readonly List<string> _pictionaryWords = new()
    {
        "cat", "dog", "house", "tree", "car", "sun", "moon", "star", "fish", "bird",
        "apple", "banana", "pizza", "cake", "book", "phone", "computer", "chair", "table", "door"
    };

    public Room CreateRoom(string? name = null)
    {
        var room = new Room
        {
            Id = GenerateRoomId(),
            Name = name ?? $"Room {DateTime.Now:yyyyMMddHHmmss}"
        };

        _rooms[room.Id] = room;

        // TODO: Implement SQLite persistence for popular rooms
        // Currently rooms are lost when server restarts

        return room;
    }

    public Room? GetRoom(string roomId)
    {
        return _rooms.GetValueOrDefault(roomId);
    }

    public void AddUserToRoom(string roomId, User user)
    {
        if (_rooms.TryGetValue(roomId, out var room))
        {
            // Remove existing user with same connection if exists
            room.Users.RemoveAll(u => u.Id == user.Id);
            room.Users.Add(user);
        }
    }

    public User? RemoveUserFromRoom(string roomId, string userId)
    {
        if (_rooms.TryGetValue(roomId, out var room))
        {
            var user = room.Users.FirstOrDefault(u => u.Id == userId);
            if (user != null)
            {
                room.Users.Remove(user);
                // Clean up empty rooms after some time
                if (room.Users.Count == 0)
                {
                    _ = Task.Delay(TimeSpan.FromMinutes(5)).ContinueWith(_ =>
                    {
                        if (_rooms.TryGetValue(roomId, out var r) && r.Users.Count == 0)
                        {
                            _rooms.Remove(roomId);
                        }
                    });
                }
                return user;
            }
        }
        return null;
    }

    public User? UpdateUserCursor(string roomId, string userId, double x, double y)
    {
        if (_rooms.TryGetValue(roomId, out var room))
        {
            var user = room.Users.FirstOrDefault(u => u.Id == userId);
            if (user != null)
            {
                user.CursorX = x;
                user.CursorY = y;
                user.LastActivity = DateTime.UtcNow;
                return user;
            }
        }
        return null;
    }

    public void AddDrawingObject(string roomId, DrawingObject drawingObject)
    {
        if (_rooms.TryGetValue(roomId, out var room))
        {
            room.DrawingObjects.Add(drawingObject);
        }
    }

    public void UpdateDrawingObject(string roomId, DrawingObject drawingObject)
    {
        if (_rooms.TryGetValue(roomId, out var room))
        {
            var existing = room.DrawingObjects.FirstOrDefault(d => d.Id == drawingObject.Id);
            if (existing != null)
            {
                var index = room.DrawingObjects.IndexOf(existing);
                room.DrawingObjects[index] = drawingObject;
            }
        }
    }

    public User? GetUser(string roomId, string userId)
    {
        if (_rooms.TryGetValue(roomId, out var room))
        {
            return room.Users.FirstOrDefault(u => u.Id == userId);
        }
        return null;
    }

    public IEnumerable<string> GetRoomsWithUser(string userId)
    {
        return _rooms.Where(r => r.Value.Users.Any(u => u.Id == userId))
                     .Select(r => r.Key);
    }

    public string? StartPictionary(string roomId)
    {
        if (_rooms.TryGetValue(roomId, out var room))
        {
            room.IsPictionaryMode = true;
            room.CurrentWord = _pictionaryWords[Random.Shared.Next(_pictionaryWords.Count)];
            room.WordStartTime = DateTime.UtcNow;
            return room.CurrentWord;
        }
        return null;
    }

    private string GenerateRoomId()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, 6)
            .Select(s => s[Random.Shared.Next(s.Length)]).ToArray());
    }
}
