using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TarMan : MonoBehaviour
{
    public GameObject screamHitbox;

    public TarManState currentState { get; private set; }

    public TarManWalkingState walkingState;

    private IEnumerable<TarManState> IterateStates()
    {
        yield return walkingState;
    }

    private Vector2 targetPosition;

    private void Awake()
    {
        foreach (TarManState state in IterateStates())
        {
            state.Awake();
        }
    }

    void Start()
    {
        foreach (TarManState state in IterateStates())
        {
            state.tarMan = this;
        }

        TransitionState(walkingState);

        foreach (TarManState state in IterateStates())
        {
            currentState.Start();
        }
    }

    public void TransitionState(TarManState newState)
    {
        if (currentState != null)
        {
            currentState.isCurrentState = false;
            currentState.Exit();
        }
        currentState = newState;
        if (currentState != null)
        {
            currentState.Enter();
            currentState.isCurrentState = true;
        }
    }

    private void OnValidate()
    {
        foreach (TarManState state in IterateStates())
        {
            state.OnValidate();
        }
    }

    private void Reset()
    {
        foreach (TarManState state in IterateStates())
        {
            state.tarMan = this;
            state.Reset();
        }
    }

    private void OnDrawGizmos()
    {
        foreach (TarManState state in IterateStates())
        {
            state.OnDrawGizmos();
        }
    }
}
