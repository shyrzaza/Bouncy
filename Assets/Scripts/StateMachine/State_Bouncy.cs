using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Bouncy : State<PlayerStateBehaviour>
{


    public State_Bouncy()
    {
        playerState = PlayerState.BOUNCY;
    }



    public override void Enter(PlayerStateBehaviour player)
    {
        //Debug.Log("Enter bouncy");
        player.circleCollider.sharedMaterial = player.bouncyMaterial;
    }
    public override void Exit(PlayerStateBehaviour player)
    {
        //Debug.Log("Exit bouncy");
    }
    public override void Execute(PlayerStateBehaviour player)
    {
        //GetCommands
        Debug.Log("Executing bouncy");







        //state changes
        if (Input.GetButtonDown("Harden"))
        {
            //switch to hard
            player.ChangeState(StateManager.Instance.hardState);
            return;
        }
        if(Input.GetButtonDown("Soften"))
        {
            //switch to SOFT
            player.ChangeState(StateManager.Instance.softState);
            return;
        }
    }

    public override string ToString()
    {
        return "Bouncy";
    }
}
