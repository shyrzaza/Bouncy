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
        
        if(other.gameObject.tag != "Player")
        {
            return;
        }
        else
        {

            //check consistency
            if (other.gameObject.transform.root.gameObject.GetComponent<PlayerStateBehaviour>().GetCurrentPlayerState() != PlayerState.HARD)
            {
                return;
            }
            else
            {
                //check speed
          

                GetComponent<Explodable>().explode();
                List<GameObject> fragments = GetComponent<Explodable>().fragments;
                foreach (GameObject go in fragments)
                {
                    Vector2 dist = go.transform.position - other.gameObject.transform.root.gameObject.transform.position;
                    go.GetComponent<Rigidbody2D>().AddForce(dist.normalized * 50.0f, ForceMode2D.Impulse);
                }
            }
        }





    }
}
