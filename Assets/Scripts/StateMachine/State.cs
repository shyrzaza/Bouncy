using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class State<T>{

	public State()
    {
    }
	public abstract void Enter(T entity);
	public abstract void Exit(T entity);
	public abstract void Execute(T entity);
	public abstract void FixedExecute(T entity);
    public abstract void OnContact(PlayerStateBehaviour player, string tag, Vector2 normal);
    public abstract void OnContactStay(PlayerStateBehaviour player, string tag, Vector2 normal);
    public abstract void OnContactExit(PlayerStateBehaviour player, string tag, Vector2 normal);

    //public abstract bool OnMessage(T entity, Telegram msg);

    public override abstract string ToString();

    public PlayerState playerState;
}
