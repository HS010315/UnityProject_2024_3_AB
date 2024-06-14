using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : IState
{
    private StateMachine stateMachine;
    private float duration = 2.0f;
    private float timer;

    public ChaseState(StateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public void Enter()
    {
        Debug.Log("Entered Chase State");
        timer = 0f;
    }

    public void Execute()
    {
        timer += Time.deltaTime;
        if (timer >= duration)
        {
            stateMachine.ChangeState(new IdleState(stateMachine));
        }
    }

    public void Exit()
    {
        Debug.Log("Exiting Idle State");
    }
}
