using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RopeRenderer))]

public class Anchor3Point : MonoBehaviour
{
    public Transform Hinge;

    public Transform DynamicTarget;

    public float DynamicTargetOffset = 0f;

    private RopeRenderer _ropeRenderer;

    private Vector3[] _contactPoints;

    private void Awake()
    {
        _ropeRenderer = GetComponent<RopeRenderer>();
        _contactPoints = new Vector3[3];
    }

    private void Update()
    {
        var offset = (Hinge.position - DynamicTarget.position).normalized * DynamicTargetOffset;
        _contactPoints[0] = transform.position;
        _contactPoints[1] = Hinge.position;
        _contactPoints[2] = DynamicTarget.position + offset;
        _ropeRenderer.UpdateContactPoints(_contactPoints);
    }
}
