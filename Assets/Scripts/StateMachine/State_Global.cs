using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Global : State<PlayerStateBehaviour>
{
    public override void Enter(PlayerStateBehaviour player)
    {
    }
    public override void Exit(PlayerStateBehaviour player)
    {
    }
    public override void Execute(PlayerStateBehaviour player)
    {
        //GetCommands
    }

    public override void FixedExecute(PlayerStateBehaviour entity)
    {

    }



    public override void OnContact(PlayerStateBehaviour player, string tag, Vector2 normal)
    {
    }
    public override void OnContactStay(PlayerStateBehaviour player, string tag, Vector2 normal)
    {
    }
    public override void OnContactExit(PlayerStateBehaviour player, string tag, Vector2 normal)
    {
    }


    public override string ToString()
    {
        return "Global";
    }
}
