using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public abstract class ActController : MonoBehaviour
{
    [Header("Settings")]
    public ActSettings settings = null;
    protected ActPhase phase = ActPhase.NONE;

    [Header("Results")]
    public ActResults results = null;

    [Header("Objects")]
    public Canvas canvas = null;
    public EventSystem eventSystem = null;

    [Serializable]
    public class ActSettings {
        public string type;
    }

    [Serializable]
    public class ActResults {

    };

    public class InvalidPhaseException : Exception {
        public readonly ActPhase currentPhase;

        public InvalidPhaseException(ActPhase currentPhase) {
            this.currentPhase = currentPhase;
        }
    };

    public class MissingSettingsException : Exception {};

    [Serializable]
    public enum ActPhase {
        NONE,
        INIT,
        START,
        END,
        CLEANUP
    }

    public ActPhase Phase { get; }

    /// <summary>
    /// Call this to initialize the Act (GameObjects, ActSettings, etc). All interactable components should be disabled in this phase.
    /// </summary>
    public virtual void ActInit() {
        phase = ActPhase.INIT;

        if(settings == null) {
            throw new MissingSettingsException();
        }
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

    protected virtual void Start() {
        canvas = GameObject.FindGameObjectWithTag("Canvas")?.GetComponent<Canvas>();
        eventSystem = GameObject.FindGameObjectWithTag("EventSystem")?.GetComponent<EventSystem>();
        ActInit();
    }

    protected virtual void Update() {

    }

    protected virtual void OnDestroy() {
        ActCleanup();
    }
}
