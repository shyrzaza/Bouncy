using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void OnTriggerEnter2D(Collider2D other)
    {

        GetComponent<Explodable>().explode();
        List<GameObject> fragments = GetComponent<Explodable>().fragments;
        foreach(GameObject go in fragments)
        {

        }

    }
}
