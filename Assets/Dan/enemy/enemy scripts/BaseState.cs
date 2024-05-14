using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class BaseState 
{
    public virtual void Enter(BaseStateMachine controller) { }
    public virtual void Update() { }
    public virtual void Exit() { }
}
