using System;
using UnityEngine;
using VContainer.Unity;


public enum GameState
{
    MainMenu,
    InGame
}

/// <summary>
/// A special component that represents a discrete game state and its dependencies. The special feature it offers
/// is that it provides some guarantees that only one such GameState will be running at a time.
/// </summary>
/// <remarks>
/// Q: If these are MonoBehaviours, how do you have a single state that persists across multiple scenes?
/// A: Set your Persists property to true. If you transition to another scene that has the same gamestate, the
///    current GameState object will live on, and the version in the new scene will auto-destruct to make room for it.
/// Important Note: We assume that every Scene has a GameState object. If not, then it's possible that a Persisting game state
/// will outlast its lifetime (as there is no successor state to clean it up).
/// </remarks>
public abstract class GameStateBehaviour : LifetimeScope
{
    public virtual bool PersistsBetweenScenes
    {
        get { return false; }
    }

    public abstract GameState ActiveState { get; }

    // The single active GameState object. There can only be one.
    private static GameObject ActiveStateGO;

    protected override void Awake()
    {
        base.Awake();

        if(Parent != null)
        {
            Parent.Container.Inject(this);
        }
    }
    

    protected virtual void Start()
    {
        if (ActiveStateGO != null)
        {
            if (ActiveStateGO == gameObject)
            {
                // Already the active state
                return; 
            }

            var previousState = ActiveStateGO.GetComponent<GameStateBehaviour>();

            if (previousState.PersistsBetweenScenes && previousState.ActiveState == ActiveState)
            {
                // we need to make way for the DontDestroyOnLoad state that already exists.
                Destroy(gameObject);
                return;
            }

            // otherwise, the old state is going away. Either it wasn't a Persisting state, or it was,
            // but we're a different kind of state. In either case, we're going to replace it.
            Destroy(ActiveStateGO);
        }

        ActiveStateGO = gameObject;
        if (PersistsBetweenScenes)
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    protected override void OnDestroy()
    {
        if (!PersistsBetweenScenes)
        {
            ActiveStateGO = null;
        }
    }
}
