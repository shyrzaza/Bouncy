using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfacePoint : MonoBehaviour
{

    public Vector3 targetPosition;
    Rigidbody2D rb;
    const float STIFFNESS = 200f;

    public Vector2 parentVelocity;
    public float circleCheckRadius = 0.1f;
    public LayerMask contactCheckMask;

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
        
    }

    private void FixedUpdate() {

        /*Debug stuff
        Vector3 distance = (targetPosition - transform.position);
        if(distance.sqrMagnitude > 0.1f)
        {
            rb.velocity = distance.normalized * 35f;
        }
        else{
            rb.velocity = Vector2.zero;
        }
        */

        Vector2 toTargetSpringForce = CalcTargetPosSpringForce();
        Vector2 alignVelocityVector = (parentVelocity - rb.velocity) * 10f;

        Vector2 fullForce = toTargetSpringForce + alignVelocityVector;





        //damping
        rb.velocity *= 0.95f;
        //toTargetSpringForce *= 0.90f;
        rb.AddForce(fullForce);


    }

    Vector2 CalcTargetPosSpringForce()
    {
        //calculate spring force
        Vector2 distanceVector = new Vector2(targetPosition.x, targetPosition.y) - new Vector2(transform.position.x, transform.position.y);
        float distance = distanceVector.magnitude;
        Vector2 direction = distanceVector.normalized;
        float delta = distance - 0.01f;

        Vector2 springForce = delta * STIFFNESS * direction;

        return springForce;
    }


    public void ApplySpringForce(Vector2 force)
    {
        rb.AddForce(force);
    }

    public bool ContactCheck(out string tag)
    {
        Collider2D result = Physics2D.OverlapCircle(transform.position, circleCheckRadius, contactCheckMask);

        if(result != null)
        {
            tag = result.gameObject.tag;
            return true;
        }
        else
        {
            tag = "";
            return false;
        }

    }


}
