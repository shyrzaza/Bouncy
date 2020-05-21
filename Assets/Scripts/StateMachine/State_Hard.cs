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
        player.deformationManager.UpdateSurfacePointParameters(0, 0f, -1f, 1f);
        player.deformationManager.UpdateDeformationManagerParameters(0f, 5f, 0f);

        player.circleCollider.sharedMaterial = player.hardMaterial;
        PlayerMovementPrototype.OnContact += OnContact;
        PlayerMovementPrototype.OnContactStay += OnContactStay;
        PlayerMovementPrototype.OnContactExit += OnContactExit;
    }
    public override void Exit(PlayerStateBehaviour player)
    {
        //Debug.Log("Exit hard");
        PlayerMovementPrototype.OnContact -= OnContact;
        PlayerMovementPrototype.OnContactStay -= OnContactStay;
        PlayerMovementPrototype.OnContactExit -= OnContactExit;
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


    public override void OnContact(PlayerStateBehaviour player, string tag, Vector2 normal)
    {
        Debug.Log("OnContactHard");
    }
    public override void OnContactStay(PlayerStateBehaviour player, string tag, Vector2 normal)
    {
        Debug.Log("OnContactStayHard");
    }
    public override void OnContactExit(PlayerStateBehaviour player, string tag, Vector2 normal)
    {
        Debug.Log("OnContactExitHard");
    }

    public override string ToString()
    {
        return "Hard";
    }
}
