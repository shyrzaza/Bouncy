using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class SpriteShapeManager : MonoBehaviour
{

    public Transform player;
    public SurfacePoint[] surfacePoints;
    SpriteShapeController controller;
    Spline spline;

    private float tangentStrength = 5.0f;
    
    
    void Awake()
    {
        controller = GetComponent<SpriteShapeController>();
        spline = controller.spline;
    }


    //to be called from deformation manager
    public void InitializeSurfacePoints(Transform player, SurfacePoint[] surfacePoints)
    {
        this.player = player;
        this.surfacePoints = surfacePoints;
       
        
    }

    // Start is called before the first frame update
    void Start()
    {
        //careful, might not be initialized already
        //should be fine due to execution order

        spline.Clear();


        for (int i = 0; i < surfacePoints.Length; i++)
        {
            spline.InsertPointAt(i, surfacePoints[i].transform.position);
            spline.SetTangentMode(i, ShapeTangentMode.Continuous);
        }
    }

    public void UpdateTangentAt(int index, Vector3 tangent)
    {
        Vector3 pointPosition = surfacePoints[index].transform.position;

        spline.SetRightTangent(index, -tangent / (2.0f * tangentStrength));
        spline.SetLeftTangent(index, tangent / (2.0f * tangentStrength));
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSplinePointPosition();
    }




    void UpdateSplinePointPosition()
    {
        for (int i = 0; i < surfacePoints.Length; i++)
        {
            spline.SetPosition(i, surfacePoints[i].transform.position);
        }
    }

    public void SetTangentStrength(float newStrength)
    {
        tangentStrength = newStrength;
    }
}
