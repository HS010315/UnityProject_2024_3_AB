using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : StateMachine
{
    private void Start()
    {
        ChangeState(new IdleState(this));
    }
}
