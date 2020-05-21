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
        player.deformationManager.UpdateDeformationManagerParameters(1000f, 5f, 1000f);

        PlayerMovementPrototype.OnContact += OnContact;
        PlayerMovementPrototype.OnContactStay += OnContactStay;
        PlayerMovementPrototype.OnContactExit += OnContactExit;


    }
    public override void Exit(PlayerStateBehaviour player)
    {
        // Debug.Log("Exit soft");
        PlayerMovementPrototype.OnContact -= OnContact;
        PlayerMovementPrototype.OnContactStay -= OnContactStay;
        PlayerMovementPrototype.OnContactExit -= OnContactExit;

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


    public override void OnContact(PlayerStateBehaviour player, string tag, Vector2 normal)
    {
        Debug.Log("OnContactSoft");
    }
    public override void OnContactStay(PlayerStateBehaviour player, string tag, Vector2 normal)
    {
        Debug.Log("OnContactStaySoft");
        Debug.DrawLine(player.transform.position, player.transform.position + new Vector3(normal.x, normal.y, 0) * 5f);

        player.transform.GetComponent<DeformationManager>().ApplyForceToSurfacePoints(-normal * 100f);
    }
    public override void OnContactExit(PlayerStateBehaviour player, string tag, Vector2 normal)
    {
        Debug.Log("OnContactExitSoft");
    }



    public override string ToString()
    {
        return "Soft";
    }
}
