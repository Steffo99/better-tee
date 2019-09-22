using UnityEngine;


namespace BetterTee.Player
{
        
    public class DrawTool : MonoBehaviour
    {
        protected DrawableFrame frame;

        protected virtual void Start() {
            frame = GetComponent<DrawableFrame>();
        }
    }

}
