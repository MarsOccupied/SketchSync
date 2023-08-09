// SketchSync JavaScript utilities

window.SketchSync = {
    getCanvasRect: function (canvas) {
        return canvas.getBoundingClientRect();
    },

    drawFreehand: function (canvas, points, color, strokeWidth) {
        const ctx = canvas.getContext('2d');
        ctx.strokeStyle = color;
        ctx.lineWidth = strokeWidth;
        ctx.lineCap = 'round';
        ctx.lineJoin = 'round';

        ctx.beginPath();
        if (points.length > 0) {
            ctx.moveTo(points[0].x, points[0].y);
            for (let i = 1; i < points.length; i++) {
                ctx.lineTo(points[i].x, points[i].y);
            }
        }
        ctx.stroke();
    },

    drawRectangle: function (canvas, x, y, width, height, color, strokeWidth) {
        const ctx = canvas.getContext('2d');
        ctx.strokeStyle = color;
        ctx.lineWidth = strokeWidth;

        ctx.strokeRect(x, y, width, height);
    },

    drawCircle: function (canvas, x, y, width, height, color, strokeWidth) {
        const ctx = canvas.getContext('2d');
        ctx.strokeStyle = color;
        ctx.lineWidth = strokeWidth;

        const radius = Math.min(Math.abs(width), Math.abs(height)) / 2;
        const centerX = x + width / 2;
        const centerY = y + height / 2;

        ctx.beginPath();
        ctx.arc(centerX, centerY, radius, 0, 2 * Math.PI);
        ctx.stroke();
    },

    drawLine: function (canvas, x1, y1, x2, y2, color, strokeWidth) {
        const ctx = canvas.getContext('2d');
        ctx.strokeStyle = color;
        ctx.lineWidth = strokeWidth;
        ctx.lineCap = 'round';

        ctx.beginPath();
        ctx.moveTo(x1, y1);
        ctx.lineTo(x2, y2);
        ctx.stroke();
    },

    drawText: function (canvas, text, x, y, color, fontSize) {
        const ctx = canvas.getContext('2d');
        ctx.fillStyle = color;
        ctx.font = `${fontSize}px Arial`;
        ctx.fillText(text, x, y);
    },

    drawEmoji: function (canvas, emoji, x, y, fontSize) {
        const ctx = canvas.getContext('2d');
        ctx.font = `${fontSize}px Arial`;
        ctx.fillText(emoji, x, y);
    },

    clearCanvas: function (canvas) {
        const ctx = canvas.getContext('2d');
        ctx.clearRect(0, 0, canvas.width, canvas.height);
    },

    redrawCanvas: function (canvas, drawingObjects) {
        const ctx = canvas.getContext('2d');
        ctx.clearRect(0, 0, canvas.width, canvas.height);

        drawingObjects.forEach(obj => {
            switch (obj.type) {
                case 0: // Freehand
                    this.drawFreehand(canvas, obj.points, obj.color, obj.strokeWidth);
                    break;
                case 1: // Rectangle
                    this.drawRectangle(canvas, obj.x, obj.y, obj.width, obj.height, obj.color, obj.strokeWidth);
                    break;
                case 2: // Circle
                    this.drawCircle(canvas, obj.x, obj.y, obj.width, obj.height, obj.color, obj.strokeWidth);
                    break;
                case 3: // Line
                    if (obj.points.length >= 2) {
                        this.drawLine(canvas, obj.points[0].x, obj.points[0].y, obj.points[1].x, obj.points[1].y, obj.color, obj.strokeWidth);
                    }
                    break;
                case 4: // Text
                    this.drawText(canvas, obj.text, obj.x, obj.y, obj.color, 16);
                    break;
                case 5: // Emoji
                    this.drawEmoji(canvas, obj.text, obj.x, obj.y, 24);
                    break;
            }
        });
    }
};

