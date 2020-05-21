using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<T>{

	private T owner;
	public State<T> currentState{get;private set;}
	public State<T> previousState{get;private set;}
	public State<T> globalState{get;private set;}


	public StateMachine(T theOwner)
	{
		owner = theOwner;
		currentState = null;
		previousState = null;
		globalState = null;
	}


	public void SetCurrentState(State<T> s)
	{
		currentState = s;
	}
	public void SetPreviousState(State<T> s)
	{
		previousState = s;
	}
		public void SetGlobalState(State<T> s)
	{
		globalState = s;
	}

	public void Update()
	{
		if(globalState != null)
		{
			globalState.Execute(owner);
		}

		if(currentState != null)
		{

			currentState.Execute(owner);
		}
	}


    public void FixedUpdate()
    {
        if (globalState != null)
        {
            globalState.FixedExecute(owner);
        }

        if (currentState != null)
        {

            currentState.FixedExecute(owner);
        }
    }


	public void ChangeState(State<T> newState)
	{
		previousState = currentState;
		currentState.Exit(owner);
		currentState = newState;
		currentState.Enter(owner);
	}

	public void RevertToPreviousState()
	{
		if(previousState != null)
		{
			ChangeState(previousState);
		}
	}

	public bool IsInState(State<T> s)
	{
		return currentState == s;
	}

    /*
	public bool HandleMessage(Telegram msg)
	{

		if(currentState != null && currentState.OnMessage(owner, msg))
		{
			return true;
		}

		if(globalState != null && globalState.OnMessage(owner,msg))
		{
			return true;
		}

		return false;
	}

    */
}
