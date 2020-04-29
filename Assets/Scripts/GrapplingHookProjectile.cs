using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHookProjectile : MonoBehaviour
{

    public Vector2 direction;
    const float SPEED = 50f;
    Rigidbody2D rb;

    public GrapplingHookPrototype myMaster;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }


    // Start is called before the first frame update
    void Start()
    {
        //rb.AddForce(direction * SPEED, ForceMode2D.Impulse);
        rb.velocity = direction * SPEED;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Hookable")
        {
            rb.velocity = Vector2.zero;
            Debug.Log("Found something hookable");

            //establish a spring
            myMaster.OnProjectileHit(transform.position);
        }
    }
}
