using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawableFrame : MonoBehaviour
{
    public Vector2Int resolution;
    public Texture2D texture;

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
    }
    
    protected void OnDrawGizmos() 
    {
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
