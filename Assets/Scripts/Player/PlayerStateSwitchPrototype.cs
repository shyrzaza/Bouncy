using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateSwitchPrototype : MonoBehaviour
{

    public PlayerSoftness softness;
    CircleCollider2D circleCollider;
    public PhysicsMaterial2D softMaterial;
    public PhysicsMaterial2D bouncyMaterial;
    public PhysicsMaterial2D hardMaterial;

    public PlayerSoftness formerSoftness;

    private void Awake() {
        circleCollider = GetComponentInChildren<CircleCollider2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        softness = PlayerSoftness.BOUNCY;
        formerSoftness = PlayerSoftness.BOUNCY;
    }

    // Update is called once per frame
    void Update()
    {
        //Check for softness switch

        if(PlayerSettings.controllerControl)
        {
            if (Input.GetButton("Harden"))
            {
                //switch to hard
                softness = PlayerSoftness.HARD;
            }
            else if (Input.GetButton("Soften"))
            {
                //switch to soft
                softness = PlayerSoftness.SOFT;
            }
            else
            {
                //switch to bouncy
                softness = PlayerSoftness.BOUNCY;
            }
            if (formerSoftness != softness)
            {
                OnStateSwitch();
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.E))
            {
                //switch to hard
                softness = PlayerSoftness.HARD;
            }
            else if (Input.GetKey(KeyCode.Q))
            {
                //switch to soft
                softness = PlayerSoftness.SOFT;
            }
            else
            {
                //switch to bouncy
                softness = PlayerSoftness.BOUNCY;
            }
            if (formerSoftness != softness)
            {
                OnStateSwitch();
            }
        }


        formerSoftness = softness;
    }


    void OnStateSwitch()
    {
        if(softness == PlayerSoftness.SOFT)
        {
            circleCollider.sharedMaterial = softMaterial;
        }
        if(softness == PlayerSoftness.BOUNCY)
        {
            circleCollider.sharedMaterial = bouncyMaterial;
        }
        if(softness == PlayerSoftness.HARD)
        {
            circleCollider.sharedMaterial = hardMaterial;
        }
    }


    public enum PlayerSoftness{
        SOFT,
        BOUNCY,
        HARD
    }
}
