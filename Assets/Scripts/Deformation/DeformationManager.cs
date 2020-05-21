using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeformationManager : MonoBehaviour
{

    const int NUM_SURFACEPOINTS = 20;
    public float surfaceRadius = 1.5f;
    float NEIGHBOR_SPRING_STIFFNESS = 1000f;

    float OPPOSITES_SPRING_STIFFNESS = 1000f;

    public float tangentStrength = 5.0f;

    public GameObject surfacePointPrefab;
    public SurfacePoint[] surfacePoints;
    public Transform surfacePointHolder;

    private Vector3[] localSurfacePointTargetPositions;

    private Rigidbody2D rb;

    public SpriteShapeManager spriteShapeManager;

    private float restingLength;

    private float oppositeRestingLength;


    public CircleCollider2D collider;


    public void UpdateDeformationManagerParameters(float neighborSpringStiffness, float tangentStrength, float oppositesSpringStiffness)
    {
        this.NEIGHBOR_SPRING_STIFFNESS = neighborSpringStiffness;
        this.OPPOSITES_SPRING_STIFFNESS = oppositesSpringStiffness;
        this.tangentStrength = tangentStrength;
    }
    public void UpdateSurfacePointParameters(float stiffness, float alignVelocityStrength, float maxDistance, float maxDistanceCorrectionSpeed)
    {
        foreach(SurfacePoint s in surfacePoints)
        {
            s.SetNewDeformationParameters(stiffness, alignVelocityStrength, maxDistance, maxDistanceCorrectionSpeed);
        }
    }

    private void Awake() {

        Debug.Assert(NUM_SURFACEPOINTS % 2 == 0, "nice try bro");

        surfacePoints = new SurfacePoint[NUM_SURFACEPOINTS];
        localSurfacePointTargetPositions = new Vector3[NUM_SURFACEPOINTS];
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponentInChildren<CircleCollider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        InitializeSurfacePoints();
        spriteShapeManager.InitializeSurfacePoints(gameObject.transform, surfacePoints);
        spriteShapeManager.SetTangentStrength(tangentStrength);

    }

    // Update is called once per frame
    void Update()
    {
        UpdateTargetPositions();
        UpdateTangents();

    }

    void FixedUpdate()
    {
        CalcAndApplyNeighborSpringForces();
        CalcAndApplyOppositeSpringForces();
    }


    private void UpdateTargetPositions()
    {
        for(int i = 0; i < NUM_SURFACEPOINTS; i++)
        {
            surfacePoints[i].targetPosition = transform.position + localSurfacePointTargetPositions[i];
            surfacePoints[i].parentVelocity = rb.velocity;
        }
    }

    void UpdateTangents()
    {
        for(int i = 0; i < surfacePoints.Length; i++)
        {
            SurfacePoint surfacePointLeft = surfacePoints[(i - 1 + surfacePoints.Length) % surfacePoints.Length];
            SurfacePoint surfacePointRight = surfacePoints[(i + 1) % surfacePoints.Length];
            SurfacePoint targetPoint = surfacePoints[i];

            Vector3 tangent = surfacePointLeft.transform.position - surfacePointRight.transform.position;
            spriteShapeManager.UpdateTangentAt(i, tangent);
        }
    }
    private void InitializeSurfacePoints()
    {
        float degreeStep = 360f/ (float)NUM_SURFACEPOINTS;

        for(int i = 0; i < NUM_SURFACEPOINTS; i++)
        {
            Vector3 pos = Vector3.up * surfaceRadius;
            //rotate
            pos = Quaternion.Euler(0,0,degreeStep * i) * pos;
            localSurfacePointTargetPositions[i] = pos;
            
            surfacePoints[i] = Instantiate(surfacePointPrefab, transform.position + pos, Quaternion.identity).GetComponent<SurfacePoint>();
            surfacePoints[i].transform.parent = surfacePointHolder;
        }

        restingLength = (surfacePoints[0].transform.position - surfacePoints[1].transform.position).magnitude;
        oppositeRestingLength = ((surfacePoints[0].transform.position - surfacePoints[(int)(surfacePoints.Length / 2)].transform.position).magnitude);
    }


    private void CalcAndApplyNeighborSpringForces()
    {
        for (int i = 0; i < surfacePoints.Length; i++)
        {
            SurfacePoint A = surfacePoints[i];
            SurfacePoint B = surfacePoints[(i + 1) % surfacePoints.Length];

            //calculate spring force
            Vector2 distanceVector = new Vector2(A.transform.position.x, A.transform.position.y) - new Vector2(B.transform.position.x, B.transform.position.y);
            float distance = distanceVector.magnitude;
            Vector2 direction = distanceVector.normalized;
            float delta = distance - restingLength;

            Vector2 springForce = delta * NEIGHBOR_SPRING_STIFFNESS * direction;

            A.ApplyForce(-springForce);
            B.ApplyForce(springForce);
        }
    }


    private void CalcAndApplyOppositeSpringForces()
    {
        for (int i = 0; i < surfacePoints.Length; i++)
        {
            SurfacePoint A = surfacePoints[i];
            SurfacePoint B = surfacePoints[(i + (int)(surfacePoints.Length/2)) % surfacePoints.Length];

            //calculate spring force
            Vector2 distanceVector = new Vector2(A.transform.position.x, A.transform.position.y) - new Vector2(B.transform.position.x, B.transform.position.y);
            float distance = distanceVector.magnitude;
            Vector2 direction = distanceVector.normalized;
            float delta = distance - oppositeRestingLength;

            Vector2 springForce = delta * OPPOSITES_SPRING_STIFFNESS * direction;

            A.ApplyForce(-springForce);
            B.ApplyForce(springForce);
        }
    }



    public bool CheckForContact(out string tag, out Vector2 normal)
    {
        bool result = false;
        string intermediateTag = "";
        normal = Vector2.zero;
        tag = "";

        int counter = 0;
        for(int i = 0; i < surfacePoints.Length; i++)
        {
            if(surfacePoints[i].ContactCheck(out intermediateTag))
            {
                if(intermediateTag != "")
                {
                    tag = intermediateTag;
                }
                result = true;
                Vector3 estimation = (transform.position - surfacePoints[i].targetPosition).normalized;
                normal += new Vector2(estimation.x, estimation.y);
                counter++;
            }
        }
        normal /= (float)counter;

        return result;
    }


    public Vector2 EstimateNormal()
    {
        return Vector2.zero;
    }

    public void ApplyForceToSurfacePoints(Vector2 force)
    {
        for (int i = 0; i < surfacePoints.Length; i++)
        {
            surfacePoints[i].ApplyForce(force);
        }
    }
}
