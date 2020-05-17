using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float followSpeed;
    public Transform cameraTarget;

    public bool debug;

    private Vector3 velocity = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(debug)
        {
            transform.Translate(Vector3.right * Time.deltaTime * 5f);
            return;
        }
        Vector3 point = Camera.main.WorldToViewportPoint(cameraTarget.position);
        Vector3 delta = cameraTarget.position - Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
        Vector3 destination = transform.position + delta;
        //transform.Translate(destination * Time.deltaTime * 10f);


        transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, followSpeed);



    }
}
