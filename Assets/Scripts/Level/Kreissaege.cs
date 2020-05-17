using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kreissaege : MonoBehaviour
{

    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation *= Quaternion.Euler(0, 0, speed * Time.deltaTime);


    }

    void FixedUpdate()
    {
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if(other.gameObject.transform.root.gameObject.tag == "Player")
        {
            LevelManager.instance.ReloadLevel();
        }
    }
}
