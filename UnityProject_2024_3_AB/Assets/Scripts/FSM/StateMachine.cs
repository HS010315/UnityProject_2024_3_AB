using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    void Enter();
    void Execute();
    void Exit();
}

public class StateMachine : MonoBehaviour
{
    private IState currentState;                        //현재 상태값 인터페이스

    private void Update()
    {
        currentState?.Execute();
    }

    public void ChangeState(IState newState)
    {
        currentState?.Exit();                           //이전 상태 탈출
        currentState = newState;                        //인수로 받은 상태값입력
        currentState?.Enter();                          //다음 상태값    
    }
}
