using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Bouncy : State<PlayerStateBehaviour>
{

    public float desiredColliderRadius = 1f;

    public State_Bouncy()
    {
        playerState = PlayerState.BOUNCY;
    }



    public override void Enter(PlayerStateBehaviour player)
    {
        //Debug.Log("Enter bouncy");
        player.circleCollider.sharedMaterial = player.bouncyMaterial;

        player.deformationManager.UpdateSurfacePointParameters(0, 0f, -1f, 1f);
        player.deformationManager.UpdateDeformationManagerParameters(0f, 5f, 0f);
        PlayerMovementPrototype.OnContact += OnContact;
        PlayerMovementPrototype.OnContactStay += OnContactStay;
        PlayerMovementPrototype.OnContactExit += OnContactExit;


    }

    public override void Exit(PlayerStateBehaviour player)
    {
        //Debug.Log("Exit bouncy");
        PlayerMovementPrototype.OnContact -= OnContact;
        PlayerMovementPrototype.OnContactStay -= OnContactStay;
        PlayerMovementPrototype.OnContactExit -= OnContactExit;

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

    public override void FixedExecute(PlayerStateBehaviour player)
    {
        Debug.Log("Fixed");
        //grow collider
        if (Mathf.Abs(player.deformationManager.collider.radius - desiredColliderRadius) > 0)
        {
            player.deformationManager.collider.radius += (desiredColliderRadius - player.deformationManager.collider.radius) * 0.9f;
        }
    }

    public override void OnContact(PlayerStateBehaviour player, string tag, Vector2 normal)
    {
        Debug.Log("OnContactBouncy");
    }
    public override void OnContactStay(PlayerStateBehaviour player, string tag, Vector2 normal)
    {
        Debug.Log("OnContactStayBouncy");
    }
    public override void OnContactExit(PlayerStateBehaviour player, string tag, Vector2 normal)
    {
        Debug.Log("OnContactExitBouncy");
    }

    public override string ToString()
    {
        return "Bouncy";
    }
}
