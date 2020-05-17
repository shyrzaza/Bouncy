using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Hard : State<PlayerStateBehaviour>
{

    public State_Hard()
    {
        playerState = PlayerState.HARD;
    }


    public override void Enter(PlayerStateBehaviour player)
    {
        //Debug.Log("Enter hard");
        player.circleCollider.sharedMaterial = player.hardMaterial;
    }
    public override void Exit(PlayerStateBehaviour player)
    {
        //Debug.Log("Exit hard");
    }
    public override void Execute(PlayerStateBehaviour player)
    {
        //GetCommands
        Debug.Log("Executing hard");


        //state switch
        if (!Input.GetButton("Harden"))
        {
            //switch back to bouncy
            player.ChangeState(StateManager.Instance.bouncyState);
            return;
        }
        if (Input.GetButtonDown("Soften"))
        {
            player.ChangeState(StateManager.Instance.softState);
        }
    }

    public override string ToString()
    {
        return "Hard";
    }
}
