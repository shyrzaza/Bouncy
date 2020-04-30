using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeformationManager : MonoBehaviour
{

    const int NUM_SURFACEPOINTS = 12;
    public float surfaceRadius = 1.5f;

    public GameObject surfacePointPrefab;
    public SurfacePoint[] surfacePoints;
    public Transform surfacePointHolder;

    private Vector3[] localSurfacePointTargetPositions;

    private Rigidbody2D rb;

    private void Awake() {
        surfacePoints = new SurfacePoint[NUM_SURFACEPOINTS];
        localSurfacePointTargetPositions = new Vector3[NUM_SURFACEPOINTS];
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        InitializeSurfacePoints();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTargetPositions();
    }


    private void UpdateTargetPositions()
    {
        for(int i = 0; i < NUM_SURFACEPOINTS; i++)
        {
            surfacePoints[i].targetPosition = transform.position + localSurfacePointTargetPositions[i];
            surfacePoints[i].parentVelocity = rb.velocity;
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
    }
}
