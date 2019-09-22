using System;
using UnityEngine;
using UnityEngine.EventSystems;


namespace BetterTee.Viewer 
{

    public abstract class ActViewer : MonoBehaviour
    {
        [Header("Settings")]
        public ActSettings settings = null;
        protected ActPhase phase = ActPhase.NONE;
        
        [Header("Objects")]
        public Canvas canvas = null;
        public EventSystem eventSystem = null;

        public ActPhase Phase {
            get {
                return phase;
            }
        }

        [Serializable]
        public class InvalidPhaseException : Exception {
            public readonly ActPhase currentPhase;

            public InvalidPhaseException(ActPhase currentPhase) {
                this.currentPhase = currentPhase;
            }
        };

        /// <summary>
        /// Call this to initialize the Act (GameObjects, ActSettings, etc). All interactable components should be disabled in this phase.
        /// </summary>
        public virtual void ActInit() {
            phase = ActPhase.INIT;
            
            canvas = GameObject.FindGameObjectWithTag("Canvas")?.GetComponent<Canvas>();
            eventSystem = GameObject.FindGameObjectWithTag("EventSystem")?.GetComponent<EventSystem>();
        }

        /// <summary>
        /// Call this to enable all interactable components in the Act. It should be called when the player is ready to play.
        /// </summary>
        public virtual void ActStart() {
            if(Phase != ActPhase.INIT) throw new InvalidPhaseException(phase);
            phase = ActPhase.START;
        }
        
        /// <summary>
        /// Call this to disable once again all interactable components in the Act. It should be called when the Act is finished (time ran out, player reached submission limit, etc).
        /// </summary>
        public virtual void ActEnd() {
            if(Phase != ActPhase.START) throw new InvalidPhaseException(phase);
            phase = ActPhase.END;
        }

        /// <summary>
        /// Call this to cleanup all GameObjects created during the Init phase.
        /// </summary>
        public virtual void ActCleanup() {
            if(Phase != ActPhase.END) {
                Debug.LogWarningFormat("ActCleanup() was called during {0}", Phase);
            }
            phase = ActPhase.CLEANUP;
        }

        protected virtual void Awake() {

        }

        protected virtual void Start() {
            
        }

        protected virtual void Update() {

        }

        protected virtual void OnDestroy() {
            ActCleanup();
        }
    }

}