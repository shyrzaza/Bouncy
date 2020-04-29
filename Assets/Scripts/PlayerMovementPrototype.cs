using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementPrototype : MonoBehaviour
{

    Rigidbody2D rb;
    const float FORCE_MULTIPLIER = 1000;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 movement = GetMovementInputRaw();
        rb.AddForce(movement * FORCE_MULTIPLIER * Time.deltaTime);

    }
    
    Vector2 GetMovementInputRaw()
    {
        
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        return new Vector2(horizontal, vertical).normalized;
    }

}
