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

    public override string ToString()
    {
        return "Global";
    }
}
