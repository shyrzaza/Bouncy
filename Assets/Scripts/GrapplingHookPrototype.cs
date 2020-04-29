using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHookPrototype : MonoBehaviour
{

    public GameObject grapplingHookProjectilePrefab;
    private GrapplingHookProjectile projectile;
    const float OFFSET = 0.1f;
    const float STIFFNESS = 100f;

    const float RODSOFTNESS = 0.1f;

    private HookAnker hookAnker;

    Rigidbody2D rb;

    HookType hookType;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }


    // Start is called before the first frame update
    void Start()
    {
        HookAnker dummy;
        dummy.valid = false;
        dummy.position = Vector3.zero;
        dummy.restingLength = 0;
        hookAnker = dummy;

        hookType = HookType.ROD;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 shootingDirection = CheckTargetingMouseControl();
        //Debug.DrawLine(transform.position, transform.position+new Vector3(shootingDirection.x, shootingDirection.y,0));

        //Check for shooting Input
        if(Input.GetMouseButtonDown(0))
        {
            ShootProjectile(shootingDirection.normalized);
        }
    }

    private void FixedUpdate() {
        HookPhysicsUpdate(hookType);
    }

    private void HookPhysicsUpdate(HookType hookType)
    {
        if(!hookAnker.valid)
        {
            return;
        }

        if(!Input.GetMouseButton(0))
        {
            hookAnker.valid = false;
            return;
        }

        if(hookType == HookType.SPRING)
        {
            //calculate spring force
            Vector2 distanceVector = new Vector2(hookAnker.position.x, hookAnker.position.y) - new Vector2(transform.position.x, transform.position.y);
            float distance = distanceVector.magnitude;
            Vector2 direction = distanceVector.normalized;
            float delta = distance - hookAnker.restingLength;

            Vector2 springForce = delta * STIFFNESS * direction;

            rb.AddForce(springForce);
        }

        if(hookType == HookType.ROPE)
        {
            Vector2 distanceVector = new Vector2(hookAnker.position.x, hookAnker.position.y) - new Vector2(transform.position.x, transform.position.y);
            float distance = distanceVector.magnitude;

            if(distance > hookAnker.restingLength)
            {
                //damp the speed
                Vector2 dampVector = distanceVector.normalized *-1;
                float damp = Vector2.Dot(dampVector, rb.velocity);
                rb.velocity = rb.velocity + (dampVector * -1) * damp;

                //adjust position
                transform.position = transform.position + (new Vector3(dampVector.x, dampVector.y, 0) * -1) * (distance - hookAnker.restingLength);
            }

        }

        if(hookType == HookType.SPRINGROPE)
        {
            //calculate spring force
            Vector2 distanceVector = new Vector2(hookAnker.position.x, hookAnker.position.y) - new Vector2(transform.position.x, transform.position.y);
            float distance = distanceVector.magnitude;



            Vector2 direction = distanceVector.normalized;
            float delta = distance - hookAnker.restingLength;
            if(delta < 0)
            {
                return;
            }

            Vector2 springForce = delta * STIFFNESS * direction;

            rb.AddForce(springForce);
        }

        if(hookType == HookType.ROD)
        {

            //THIS IS VERY HARD
            //MAYBE USE UNITY HINGEJOINT INSTEAD

            
            Vector2 distanceVector = new Vector2(hookAnker.position.x, hookAnker.position.y) - new Vector2(transform.position.x, transform.position.y);
            float distance = distanceVector.magnitude;
            float delta = distance - hookAnker.restingLength;

            if(Mathf.Abs(delta) <= RODSOFTNESS)
            {
                return;
            }
            if(distance > hookAnker.restingLength)
            {
                //damp the speed
                Vector2 dampVector = distanceVector.normalized *-1;
                float damp = Vector2.Dot(dampVector, rb.velocity);
                rb.velocity = rb.velocity + (dampVector * -1) * damp;

                //adjust position
                transform.position = transform.position + (new Vector3(dampVector.x, dampVector.y, 0) * -1) * (distance - hookAnker.restingLength);
            }

            if(distance < hookAnker.restingLength)
            {
                //damp the speed
                Vector2 dampVector = distanceVector.normalized;
                float damp = Vector2.Dot(dampVector, rb.velocity);
                rb.velocity = rb.velocity + (dampVector * -1) * damp;

                //adjust position
                transform.position = transform.position + (new Vector3(dampVector.x, dampVector.y, 0) * -1) * (distance - hookAnker.restingLength);
            }

        }

    }

    public Vector2 CheckTargetingMouseControl()
    {   
        return Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
    }
    public void ShootProjectile(Vector2 direction)
    {
        if(projectile != null)
        {
            GameObject.Destroy(projectile.gameObject);
        }

        projectile = Instantiate(grapplingHookProjectilePrefab, transform.position + new Vector3(direction.x, direction.y, 0) * OFFSET, Quaternion.identity).GetComponent<GrapplingHookProjectile>();
        projectile.direction = direction;
        projectile.myMaster = this;
    }


    public void OnProjectileHit(Vector3 position)
    {   
        
        //create a spring
        HookAnker newAnker;
        newAnker.valid = true;
        newAnker.position=position;
        float distance = (new Vector2(position.x,position.y) - new Vector2(transform.position.x, transform.position.y)).magnitude;
        newAnker.restingLength = distance;
        hookAnker = newAnker;
    }

    private struct HookAnker
    {
        public bool valid;
        public Vector3 position;
        public float restingLength;
    }



    private enum HookType{
        SPRING,
        ROPE,
        SPRINGROPE,
        ROD
    }
}
