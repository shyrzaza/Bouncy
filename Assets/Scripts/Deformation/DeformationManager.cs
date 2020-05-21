using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeformationManager : MonoBehaviour
{

    const int NUM_SURFACEPOINTS = 20;
    public float surfaceRadius = 1.5f;
    float NEIGHBOR_SPRING_STIFFNESS = 1000f;
    public float tangentStrength = 5.0f;

    public GameObject surfacePointPrefab;
    public SurfacePoint[] surfacePoints;
    public Transform surfacePointHolder;

    private Vector3[] localSurfacePointTargetPositions;

    private Rigidbody2D rb;

    public SpriteShapeManager spriteShapeManager;

    private float restingLength;


    public CircleCollider2D collider;


    public void UpdateDeformationManagerParameters(float neighborSpringStiffness, float tangentStrength)
    {
        this.NEIGHBOR_SPRING_STIFFNESS = neighborSpringStiffness;
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
    }


    private void CalcAndApplyNeighborSpringForces()
    {
        for(int i = 0; i < surfacePoints.Length; i ++)
        {
            SurfacePoint A = surfacePoints[i];
            SurfacePoint B = surfacePoints[(i + 1) % surfacePoints.Length];

            //calculate spring force
            Vector2 distanceVector = new Vector2(A.transform.position.x, A.transform.position.y) - new Vector2(B.transform.position.x, B.transform.position.y);
            float distance = distanceVector.magnitude;
            Vector2 direction = distanceVector.normalized;
            float delta = distance - restingLength;

            Vector2 springForce = delta * NEIGHBOR_SPRING_STIFFNESS * direction;

            A.ApplySpringForce(-springForce);
            B.ApplySpringForce(springForce);
        }
    }

    public bool CheckForContact(out string tag)
    {
        for(int i = 0; i < surfacePoints.Length; i++)
        {
            if(surfacePoints[i].ContactCheck(out tag))
            {
                return true;
            }
        }

        tag = "";
        return false;
    }
}
