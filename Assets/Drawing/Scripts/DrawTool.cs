using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawTool : MonoBehaviour
{
    protected DrawableFrame frame;

    protected virtual void Start() {
        frame = GetComponent<DrawableFrame>();
    }
}
