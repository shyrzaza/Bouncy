using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Soft : State<PlayerStateBehaviour>
{

    public State_Soft()
    {
        playerState = PlayerState.SOFT;
    }


    public override void Enter(PlayerStateBehaviour player)
    {
       // Debug.Log("Enter soft");
        player.circleCollider.sharedMaterial = player.softMaterial;
    }
    public override void Exit(PlayerStateBehaviour player)
    {
       // Debug.Log("Exit soft");
    }
    public override void Execute(PlayerStateBehaviour player)
    {
        //GetCommands
        Debug.Log("Executing soft");

    

        //state switch
        if (!Input.GetButton("Soften"))
        {
            //switch back to bouncy
            player.ChangeState(StateManager.Instance.bouncyState);
            return;
        }
        if(Input.GetButtonDown("Harden"))
        {
            player.ChangeState(StateManager.Instance.hardState);
        }
    }

    public override string ToString()
    {
        return "Soft";
    }
}
