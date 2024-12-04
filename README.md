# SketchSync - Instant Multiplayer Doodle Magic

A real-time collaborative drawing application built with ASP.NET Core and Blazor Server.

## Features ✅

- **Real-time collaboration** with SignalR
- **Room-based drawing** - create or join rooms instantly
- **Multiple drawing tools**: Freehand, shapes (rectangle, circle, line), text, emoji stamps
- **Live cursors** showing other users' positions
- **Real-time chat** for communication
- **Pictionary mode** (basic implementation)
- **Responsive design** with modern UI

## Known Issues & TODOs 🚧

### High Priority
- **FIXED: Canvas performance issues** - Large numbers of drawing objects cause slowdown
- **Canvas redraw optimization needed** - Currently redraws everything on each update
- **Memory leak in room cleanup** - Rooms not properly disposed when empty
- **Text input modal positioning** - Modal appears in wrong location on mobile

### Medium Priority
- **Undo/Redo functionality** - Not yet implemented
- **Laser pointer mode** - Missing implementation
- **Export to PNG/SVG** - Basic structure exists but not functional
- **Zoom and pan** - Canvas navigation not implemented
- **Grid and snap** - Missing grid overlay and snapping

### Low Priority
- **User avatars** - Currently using simple colored dots
- **Voice chat hints** - No voice functionality (by design)
- **Theme toggles** - Dark mode, retro pixel style
- **Persistent rooms** - SQLite storage for popular rooms
- **Mobile optimization** - Touch drawing needs improvement

### Bugs
- **Emoji picker doesn't close** when clicking outside
- **Text tool crashes** on empty input in some browsers
- **Cursor sync lag** on slow connections
- **Room ID validation** - No proper validation for room codes

## Getting Started

1. Clone the repository
2. Run `dotnet run` in the SketchSync directory
3. Open https://localhost:5001 in your browser
4. Create a room or join an existing one

## Architecture

- **Frontend**: Blazor Server with HTML5 Canvas
- **Backend**: ASP.NET Core with SignalR for real-time communication
- **Data**: In-memory storage (temporary rooms)
- **Real-time**: WebSocket connections via SignalR

## Contributing

This project is in active development. Feel free to contribute fixes for any of the issues listed above!

## Tech Stack

- ASP.NET Core 8.0
- Blazor Server
- SignalR
- HTML5 Canvas
- CSS3 with modern styling

