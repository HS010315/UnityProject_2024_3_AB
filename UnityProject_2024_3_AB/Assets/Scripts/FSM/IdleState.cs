using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState
{
    private StateMachine stateMachine;
    private float duration = 2.0f;
    private float timer;

    public IdleState(StateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public void Enter()
    {
        Debug.Log("Entered Idle State");
        timer = 0f;
    } 

    public void Execute()
    {
        timer += Time.deltaTime;
        if(timer >= duration)
        {
            stateMachine.ChangeState(new PatrolState(stateMachine));
        }
    }

    public void Exit()
    {
        Debug.Log("Exiting Idle State");
    }
}
