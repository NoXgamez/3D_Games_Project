using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class BaseStateMachine : MonoBehaviour
{
    public UnityEvent<int> StateChanged;

    private BaseState CurrentStateImplementation;
    public int CurrentState {  get; private set; }

    protected Dictionary<int, BaseState> States = new();

    protected virtual void Awake() { }
    protected virtual void Start() { }

    protected virtual void Update()
    {
        if (CurrentStateImplementation != null)
            CurrentStateImplementation.Update();
    }

    public void SetState(int newState)
    {
        if (CurrentState == newState || !States.ContainsKey(newState)) return;

        CurrentState = newState;

        if (States.ContainsKey(CurrentState))
        {
            if (CurrentStateImplementation != null)
                CurrentStateImplementation.Exit();

            CurrentStateImplementation = States[CurrentState];
            CurrentStateImplementation.Enter(this);
        }
        StateChanged?.Invoke(newState);
    }
}
