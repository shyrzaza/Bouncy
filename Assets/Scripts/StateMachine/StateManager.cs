using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour {

	public State<PlayerStateBehaviour> globalState;
	public State<PlayerStateBehaviour> bouncyState;
	public State<PlayerStateBehaviour> softState;
	public State<PlayerStateBehaviour> hardState;



    private static StateManager _instance;
	public static StateManager Instance {get{return _instance;}}


	// Use this for initialization
	private void Awake()
	{
		if(_instance != null && Instance != this)
		{
			Destroy(this.gameObject);
		} else
		{
			_instance = this;
		}


        globalState = new State_Global();
        bouncyState = new State_Bouncy();
        hardState = new State_Hard();
        softState = new State_Soft();

    }
    void Start()
	{
		
	}
}
