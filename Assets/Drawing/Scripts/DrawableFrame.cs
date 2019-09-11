using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawableFrame : MonoBehaviour
{
    [Header("Configuration")]
    public Vector2Int resolution;
    public Color startingColor = Color.white;

    [Header("State")]
    public bool locked = false;
    public bool hasChanged = false;

    [Header("References")]
    protected Texture2D texture = null;
    protected Sprite sprite = null;
    protected SpriteRenderer spriteRenderer;

    public Rect Bounds {
        get {
            return new Rect(transform.position.x - (transform.localScale.x / 2),
                            transform.position.y - (transform.localScale.y / 2),
                            transform.localScale.x,
                            transform.localScale.y);
        }
    }
    
    protected void Start()
    {
        texture = new Texture2D(resolution.x, resolution.y);
        if(resolution.x <= 128 || resolution.y <= 128) {
            texture.filterMode = FilterMode.Point;
        }
        else {
            texture.filterMode = FilterMode.Trilinear;
        }

        Color[] colors = texture.GetPixels();
        for(int i = 0; i < colors.Length; i++) {
            colors[i] = startingColor;
        }
        texture.SetPixels(colors);
        texture.Apply();

        spriteRenderer = GetComponent<SpriteRenderer>();
        sprite = Sprite.Create(texture, new Rect(0, 0, resolution.x, resolution.y), new Vector2(0.5f, 0.5f), resolution.x);
        spriteRenderer.sprite = sprite;
    }

    public Color[] GetPixels() {
        return texture.GetPixels();
    }

    public void SetPixels(Color[] colors) {
        if(!locked) {
            texture.SetPixels(colors);
            hasChanged = true;
        }
    }

    public byte[] ToPNG() {
        return texture.EncodeToPNG();
    }

    protected void Update() {
        if(hasChanged) {
            texture.Apply();
        }
    }
    
    protected void OnDrawGizmos() 
    {
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
