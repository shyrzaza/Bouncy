using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementPrototype : MonoBehaviour
{

    Rigidbody2D rb;
    const float FORCE_MULTIPLIER = 1000;


    DeformationManager deformationManager;

    PlayerStateBehaviour playerStateBehaviour;

    private bool jumpCharge = false;


    public float jumpForce = 0f;
    public float jumpForceIncreasePerSecond = 20f;
    public float maxJumpForce = 100f;


    public float currentCounterGravity = 0.0f;
    public float counterGravityDecay = 0.01f;
    public float counterGravity = 49.05f;


    bool inContact = false;
    bool inContactLastFrame = false;
    string contactTag = "";

    public delegate void voidDelegate(PlayerStateBehaviour player, string tag);
    public static voidDelegate OnContact;



    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        deformationManager = GetComponent<DeformationManager>();
        playerStateBehaviour = GetComponent<PlayerStateBehaviour>();
    }


    //TODO ADD PREEMPTIVE STOP FOR COUNTER GRAVITY


    // Start is called before the first frame update
    void Start()
    {
        
    }


    void FixedUpdate()
    {
       // rb.AddForce(Vector2.up * 49.05f * rb.mass);
       //F = m * g
    }


    // Update is called once per frame
    void Update()
    {


        //check for contact and trigger event
        inContact = deformationManager.CheckForContact(out contactTag);
        if(inContact && !inContactLastFrame)
        {
            if(OnContact != null)
            {
                OnContact(GetComponent<PlayerStateBehaviour>(), contactTag);
            }
        }
        inContactLastFrame = inContact;




        if (playerStateBehaviour.GetCurrentPlayerState() == PlayerState.SOFT)
        {

            if(!jumpCharge)
            {
                Vector2 movement = GetMovementInputRaw();
                rb.AddForce(movement * FORCE_MULTIPLIER * Time.deltaTime);
            }



            ///Jumping
            ///

            if (inContact)
            {

                if (contactTag == "Sticky")
                {
                    if(!jumpCharge)
                    {
                        //Initialize Jump
                        jumpCharge = true;
                        rb.velocity = Vector2.zero;

                        StartCoroutine(JumpCoroutine());

                        rb.isKinematic = true;
                    }

                }
                    
            }

        }
        else if(playerStateBehaviour.GetCurrentPlayerState() == PlayerState.BOUNCY)
        {

            Vector2 movement = GetMovementInputRaw();
            rb.AddForce(movement * FORCE_MULTIPLIER * Time.deltaTime);


        }
        else if(playerStateBehaviour.GetCurrentPlayerState() == PlayerState.HARD)
        {

            Vector2 movement = GetMovementInputRaw();
            rb.AddForce(movement * FORCE_MULTIPLIER * Time.deltaTime);
        }

        
        
      
    }

    IEnumerator JumpCoroutine()
    {
        jumpForce = 0f;
        while(jumpCharge)
        {

            if(jumpForce < maxJumpForce)
            {
                jumpForce += jumpForceIncreasePerSecond * Time.deltaTime;
            }


            if (!Input.GetButton("Soften"))
            {
                jumpCharge = false;

                //jump

                Debug.Log("Jump");

                rb.isKinematic = false;

                Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

                rb.AddForce(direction.normalized * jumpForce, ForceMode2D.Impulse);
                StartCoroutine(CounterGravityCoroutine());

            }

            yield return 0;
        }

        yield return null;
    } 

    IEnumerator CounterGravityCoroutine()
    {
        currentCounterGravity = 1.0f;

        while(currentCounterGravity > 0.0f)
        {

            rb.AddForce(Vector2.up * rb.mass * counterGravity * currentCounterGravity);

            currentCounterGravity -= counterGravityDecay;

            yield return new WaitForFixedUpdate();
        }
    }
    
    Vector2 GetMovementInputRaw()
    {
        
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        return new Vector2(horizontal, vertical).normalized;
    }

}
