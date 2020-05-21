using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfacePoint : MonoBehaviour
{

    public Vector3 targetPosition;
    Rigidbody2D rb;
    float STIFFNESS = 200f;

    public Vector2 parentVelocity;
    public float circleCheckRadius = 0.1f;
    public LayerMask contactCheckMask;

    float alignVelocityStrength = 10f;

    float maxDistance = 1f;
    float maxDistanceCorrectionSpeed = 0.9f;


    Vector3 lastCollisionNormal;

    public void SetNewDeformationParameters(float stiffness, float alignVelocityStrength, float maxDistance, float maxDistanceCorrectionSpeed)
    {
        this.STIFFNESS = stiffness;
        this.alignVelocityStrength = alignVelocityStrength;
        this.maxDistance = maxDistance;
        this.maxDistanceCorrectionSpeed = maxDistanceCorrectionSpeed;
    }

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
        Vector2 distanceVector = new Vector2(targetPosition.x, targetPosition.y) - new Vector2(transform.position.x, transform.position.y);
        float distance = distanceVector.magnitude;
        Debug.DrawLine(transform.position, targetPosition);

        if (distance > maxDistance)
        {
            Vector2 correction = distanceVector.normalized * (distance);
            // correction = Vector2.ClampMagnitude(correction, 0.2f);
            //transform.Translate(new Vector3(correction.x, correction.y, 0));
            transform.position = transform.position + new Vector3(correction.x, correction.y, 0) * maxDistanceCorrectionSpeed;
            Debug.Log(maxDistanceCorrectionSpeed);
            
        }



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

        //enforce max distance


        Vector2 distanceVector = new Vector2(targetPosition.x, targetPosition.y) - new Vector2(transform.position.x, transform.position.y);
        float distance = distanceVector.magnitude;

        Vector2 toTargetSpringForce = CalcTargetPosSpringForce(distanceVector, distance);
        Vector2 alignVelocityVector = (parentVelocity - rb.velocity) * alignVelocityStrength;

        Vector2 fullForce = toTargetSpringForce + alignVelocityVector;


        //damping
        rb.velocity *= 0.95f;
        //toTargetSpringForce *= 0.90f;
        rb.AddForce(fullForce);




    }

    Vector2 CalcTargetPosSpringForce(Vector2 distanceVector, float distance)
    {
        //calculate spring force
        Vector2 direction = distanceVector.normalized;
        float delta = distance - 0.01f;

        Vector2 springForce = delta * STIFFNESS * direction;

        return springForce;
    }


    public void ApplyForce(Vector2 force)
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
