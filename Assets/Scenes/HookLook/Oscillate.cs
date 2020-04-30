using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillate : MonoBehaviour
{
    public float Wigglyness = 5f;
    public float Speed = 0.05f;

    private Vector3 From;
    private Vector3 To;

    private float _t = 0f;

    private void Start()
    {
        From = transform.position + Vector3.up * Wigglyness;
        To = transform.position + Vector3.down * Wigglyness;
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(From, To, (Mathf.Sin(_t += Time.deltaTime * Speed) + 1f) / 2f);
    }
}
