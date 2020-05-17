using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    SOFT,
    BOUNCY,
    HARD
};

public class PlayerStateBehaviour : MonoBehaviour
{

    StateMachine<PlayerStateBehaviour> SM;
    public CircleCollider2D circleCollider;
    public PhysicsMaterial2D softMaterial;
    public PhysicsMaterial2D bouncyMaterial;
    public PhysicsMaterial2D hardMaterial;


    private void Awake()
    {
        circleCollider = GetComponentInChildren<CircleCollider2D>();
    }


    // Start is called before the first frame update
    void Start()
    {
        circleCollider.sharedMaterial = bouncyMaterial;


        //InitStateMachine
        SM = new StateMachine<PlayerStateBehaviour>(this);
        SM.SetCurrentState(StateManager.Instance.bouncyState);
        SM.SetGlobalState(StateManager.Instance.globalState);
    }

    // Update is called once per frame
    void Update()
    {
        SM.Update();
    }

    public void ChangeState(State<PlayerStateBehaviour> s)
    {
        SM.ChangeState(s);
    }

    public PlayerState GetCurrentPlayerState()
    {

        return SM.currentState.playerState;
    }

}
