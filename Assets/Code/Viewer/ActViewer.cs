using UnityEngine;
using UnityEngine.EventSystems;


namespace BetterTee.Viewer 
{

    public class ActViewer
    {
        [Header("Settings")]
        public ActSettings settings = null;
        protected ActPhase phase = ActPhase.NONE;
        
        [Header("Objects")]
        public Canvas canvas = null;
        public EventSystem eventSystem = null;
    }

}