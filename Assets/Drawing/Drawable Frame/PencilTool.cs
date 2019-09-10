using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PencilTool : DrawTool
{
    public Color color = Color.black;
    public float size = 1f;

    protected Vector2Int? HoveredPixel() {
        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if(frame.Bounds.Contains(worldPoint)) {
            Vector2 normalized = Rect.PointToNormalized(frame.Bounds, worldPoint);
            Vector2Int result = new Vector2Int(Mathf.FloorToInt(normalized.x * frame.resolution.x), Mathf.FloorToInt(normalized.y * frame.resolution.y));
            return result;
        }
        return null;
    }

    protected override void Start() {
        base.Start();
        InvokeRepeating("Apply", 0.05f, 0.05f);
    }

    protected void Update() {
        Color[] colors = frame.texture.GetPixels();
        if(Input.GetMouseButton(0)) {
            Vector2Int? pixel = HoveredPixel();
            if(pixel.HasValue) {
                int x_start = Mathf.Clamp(Mathf.CeilToInt((float)pixel.Value.x - size), 0, frame.resolution.x);
                int x_end = Mathf.Clamp(Mathf.CeilToInt((float)pixel.Value.x + size), 0, frame.resolution.x);
                int y_start = Mathf.Clamp(Mathf.CeilToInt((float)pixel.Value.y - size), 0, frame.resolution.y);
                int y_end = Mathf.Clamp(Mathf.CeilToInt((float)pixel.Value.y + size), 0, frame.resolution.y);
                for(int x = x_start; x < x_end; x++) {
                    for(int y = y_start; y < y_end; y++) {
                        if(Vector2Int.Distance(new Vector2Int(x, y), pixel.Value) < size) {
                            colors[x + y*frame.resolution.x] = color;
                        }
                    }
                }
            }
        }
        frame.texture.SetPixels(colors);
    }

    protected void Apply() {
        frame.texture.Apply();
    }
}
