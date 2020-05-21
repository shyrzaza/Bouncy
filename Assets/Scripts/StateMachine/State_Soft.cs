using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Soft : State<PlayerStateBehaviour>
{

    float desiredColliderRadius = 0.5f;
    public State_Soft()
    {
        playerState = PlayerState.SOFT;

    }


    public override void Enter(PlayerStateBehaviour player)
    {
       // Debug.Log("Enter soft");
        player.circleCollider.sharedMaterial = player.softMaterial;


        player.deformationManager.UpdateSurfacePointParameters(200f, 10f, 1f, 0.9f);
        player.deformationManager.UpdateDeformationManagerParameters(1000f, 5f);

        PlayerMovementPrototype.OnContact += OnContact;


    }
    public override void Exit(PlayerStateBehaviour player)
    {
        // Debug.Log("Exit soft");
        PlayerMovementPrototype.OnContact -= OnContact;

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

    public override void FixedExecute(PlayerStateBehaviour player)
    {
        if (Mathf.Abs(player.deformationManager.collider.radius - desiredColliderRadius) > 0)
        {
            player.deformationManager.collider.radius += (desiredColliderRadius - player.deformationManager.collider.radius) * 0.9f;
        }
    }


    public override void OnContact(PlayerStateBehaviour player, string tag)
    {
        Debug.Log("OnContactSoft");

    }



    public override string ToString()
    {
        return "Soft";
    }
}
