using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BetterTee.Player 
{
    
    public class PencilTool : DrawTool
    {
        public Color selectedColor = Color.black;
        public float size = 1f;
        
        protected Color[] colors;
        
        private Vector2Int? previousPixel;
        private Vector2Int? pixel;

        protected Vector2Int? GetPixelAtScreenPosition(Vector2 screenPosition) {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(screenPosition);
            if(frame.Bounds.Contains(worldPoint)) {
                Vector2 normalized = Rect.PointToNormalized(frame.Bounds, worldPoint);
                Vector2Int result = new Vector2Int(Mathf.FloorToInt(normalized.x * frame.resolution.x), Mathf.FloorToInt(normalized.y * frame.resolution.y));
                return result;
            }
            return null;
        }

        protected override void Start() {
            base.Start();
            colors = frame.GetPixels();

            Input.simulateMouseWithTouches = false;
        }

        protected void Update() {
            bool hasChanged = false;

            // Touch
            foreach(Touch t in Input.touches) {
                pixel = GetPixelAtScreenPosition(t.position);
                previousPixel = GetPixelAtScreenPosition(t.position - t.deltaPosition);
                if(pixel.HasValue) {
                    if(previousPixel.HasValue) {
                        DrawBresenhamsThickLine(pixel.Value, previousPixel.Value, size, selectedColor);
                    }
                    else {
                        DrawFilledCircle(pixel.Value, size, selectedColor);
                    }
                }
                hasChanged = true;
            }

            // Mouse
            previousPixel = pixel;
            if(Input.GetMouseButton(0)) {
                pixel = GetPixelAtScreenPosition(Input.mousePosition);
                if(pixel.HasValue) {
                    if(previousPixel.HasValue) {
                        DrawBresenhamsThickLine(previousPixel.Value, pixel.Value, size, selectedColor);
                    }
                    else {
                        DrawFilledCircle(pixel.Value, size, selectedColor);
                    }
                }
                hasChanged = true;
            }
            else {
                pixel = null;
            }

            if(hasChanged) {
                frame.SetPixels(colors);
            }
        }

        protected void DrawPixel(int x, int y, Color color) {
            if(x < 0 || x >= frame.resolution.x || y < 0 || y >= frame.resolution.y) return;
            colors[x + y*frame.resolution.x] = color;
        }

        protected void DrawCircleEightPoints(Vector2Int center, int x_radius, int y_radius, Color color) {
            DrawPixel(center.x+x_radius, center.y+y_radius, color); 
            DrawPixel(center.x-x_radius, center.y+y_radius, color); 
            DrawPixel(center.x+x_radius, center.y-y_radius, color); 
            DrawPixel(center.x-x_radius, center.y-y_radius, color); 
            DrawPixel(center.x+y_radius, center.y+x_radius, color); 
            DrawPixel(center.x-y_radius, center.y+x_radius, color); 
            DrawPixel(center.x+y_radius, center.y-x_radius, color); 
            DrawPixel(center.x-y_radius, center.y-x_radius, color); 
        }

        // No idea on how does it work
        // https://www.geeksforgeeks.org/bresenhams-circle-drawing-algorithm/
        protected void DrawBresenhamEmptyCircle(Vector2Int center, int radius, Color color) {
            int x = 0;
            int y = radius;
            int d = 3 - 2 * radius;
            DrawCircleEightPoints(center, x, y, color);
            while(y >= x) {
                x++;
                if(d > 0) {
                    y--;
                    d = d + 4 * (x - y) + 10;
                }
                else {
                    d = d + 4 * x + 6;
                }
                DrawCircleEightPoints(center, x, y, color);
            }
        }

        protected void DrawFilledCircle(Vector2Int center, float radius, Color color) {
            int x_start = Mathf.CeilToInt((float)center.x - radius);
            int x_end = Mathf.CeilToInt((float)center.x + radius);
            int y_start = Mathf.CeilToInt((float)center.y - radius);
            int y_end = Mathf.CeilToInt((float)center.y + radius);
            for(int x = x_start; x < x_end; x++) {
                for(int y = y_start; y < y_end; y++) {
                    if(Vector2Int.Distance(new Vector2Int(x, y), center) < radius) {
                        DrawPixel(x, y, color);
                    }
                }
            }
        }

        // http://www.roguebasin.com/index.php?title=Bresenham%27s_Line_Algorithm
        protected void DrawBresenhamsThickLine(Vector2Int start, Vector2Int end, float radius, Color color) {
            int start_x = start.x, start_y = start.y, end_x = end.x, end_y = end.y;
            bool steep = Mathf.Abs(end_y - start_y) > Mathf.Abs(end_x - start_x);
            if (steep) {
                Utils.Swap<int>(ref start_x, ref start_y);
                Utils.Swap<int>(ref end_x, ref end_y); 
            }
            if (start_x > end_x) {
                Utils.Swap<int>(ref start_x, ref end_x);
                Utils.Swap<int>(ref start_y, ref end_y); 
            }
            int dX = (end_x - start_x), dY = Mathf.Abs(end_y - start_y), err = (dX / 2), ystep = (start_y < end_y ? 1 : -1), y = start_y;

            for (int x = start_x; x <= end_x; ++x)
            {
                if(steep) {
                    DrawFilledCircle(new Vector2Int(y, x), radius, color);
                }
                else {
                    DrawFilledCircle(new Vector2Int(x, y), radius, color);
                }
                err = err - dY;
                if (err < 0) { y += ystep;  err += dX; }
            }
        }

        protected void Apply() {
        }
    }

}