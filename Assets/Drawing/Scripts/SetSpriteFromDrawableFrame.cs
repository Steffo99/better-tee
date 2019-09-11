using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSpriteFromDrawableFrame : MonoBehaviour
{   
    protected DrawableFrame frame;
    protected SpriteRenderer spriteRenderer;

    void Start()
    {
        frame = GetComponent<DrawableFrame>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Sprite sprite = Sprite.Create(frame.texture, new Rect(0, 0, frame.resolution.x, frame.resolution.y), new Vector2(0.5f, 0.5f), frame.resolution.x);
        spriteRenderer.sprite = sprite;
    }
}
