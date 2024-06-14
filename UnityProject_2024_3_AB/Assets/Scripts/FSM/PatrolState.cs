using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IState
{
    private StateMachine stateMachine;
    private float duration = 2.0f;
    private float timer;

    public PatrolState(StateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public void Enter()
    {
        Debug.Log("Entered Patrol State");
        timer = 0f;
    }

    public void Execute()
    {
        timer += Time.deltaTime;
        if (timer >= duration)
        {
            stateMachine.ChangeState(new ChaseState(stateMachine));
        }
    }

    public void Exit()
    {
        Debug.Log("Exiting Idle State");
    }
}
