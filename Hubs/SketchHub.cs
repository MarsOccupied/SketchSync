using Microsoft.AspNetCore.SignalR;
using SketchSync.Models;
using SketchSync.Services;

namespace SketchSync.Hubs;

public class SketchHub : Hub
{
    private readonly RoomService _roomService;

    public SketchHub(RoomService roomService)
    {
        _roomService = roomService;
    }

    public async Task JoinRoom(string roomId, string userName, string color)
    {
        var user = new User
        {
            Id = Context.ConnectionId,
            Name = userName,
            Color = color
        };

        await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
        _roomService.AddUserToRoom(roomId, user);

        // Send current room state to the new user
        var room = _roomService.GetRoom(roomId);
        if (room != null)
        {
            await Clients.Caller.SendAsync("RoomJoined", room);
            await Clients.Group(roomId).SendAsync("UserJoined", user);
        }
    }

    public async Task LeaveRoom(string roomId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);
        var user = _roomService.RemoveUserFromRoom(roomId, Context.ConnectionId);

        if (user != null)
        {
            await Clients.Group(roomId).SendAsync("UserLeft", user);
        }
    }

    public async Task UpdateCursor(string roomId, double x, double y)
    {
        var user = _roomService.UpdateUserCursor(roomId, Context.ConnectionId, x, y);
        if (user != null)
        {
            await Clients.OthersInGroup(roomId).SendAsync("CursorMoved", user);
        }
    }

    public async Task AddDrawingObject(string roomId, DrawingObject drawingObject)
    {
        drawingObject.UserId = Context.ConnectionId;
        _roomService.AddDrawingObject(roomId, drawingObject);

        await Clients.OthersInGroup(roomId).SendAsync("DrawingObjectAdded", drawingObject);
    }

    public async Task UpdateDrawingObject(string roomId, DrawingObject drawingObject)
    {
        _roomService.UpdateDrawingObject(roomId, drawingObject);

        await Clients.OthersInGroup(roomId).SendAsync("DrawingObjectUpdated", drawingObject);
    }

    public async Task SendChatMessage(string roomId, string message)
    {
        var user = _roomService.GetUser(roomId, Context.ConnectionId);
        if (user != null)
        {
            var chatMessage = new ChatMessage
            {
                UserId = user.Id,
                UserName = user.Name,
                Message = message,
                Timestamp = DateTime.UtcNow
            };

            await Clients.Group(roomId).SendAsync("ChatMessageReceived", chatMessage);
        }
    }

    public async Task StartPictionary(string roomId)
    {
        var word = _roomService.StartPictionary(roomId);
        if (word != null)
        {
            await Clients.Group(roomId).SendAsync("PictionaryStarted", word);
        }
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        // Find and remove user from all rooms
        var roomsToUpdate = _roomService.GetRoomsWithUser(Context.ConnectionId);
        foreach (var roomId in roomsToUpdate)
        {
            await LeaveRoom(roomId);
        }

        await base.OnDisconnectedAsync(exception);
    }
}

public class ChatMessage
{
    public string UserId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
}

