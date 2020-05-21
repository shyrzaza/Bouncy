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
        PlayerMovementPrototype.OnContact += OnContact;
    }
    public override void Exit(PlayerStateBehaviour player)
    {
        //Debug.Log("Exit hard");
        PlayerMovementPrototype.OnContact -= OnContact;
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

    public override void FixedExecute(PlayerStateBehaviour entity)
    {

    }


    public override void OnContact(PlayerStateBehaviour player, string tag)
    {
        Debug.Log("OnContactSoft");

    }

    public override string ToString()
    {
        return "Hard";
    }
}
