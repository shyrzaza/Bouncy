using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RopeRenderer))]
public class AnchorOriginTransform : MonoBehaviour
{
    public Transform DynamicTarget;

    public float DynamicTargetOffset = 0f;

    private RopeRenderer _ropeRenderer;

    private Vector3[] _contactPoints;

    private void Awake()
    {
        _ropeRenderer = GetComponent<RopeRenderer>();
        _contactPoints = new Vector3[2];
    }

    private void Update()
    {
        var offset = (transform.position - DynamicTarget.position).normalized * DynamicTargetOffset;
        _contactPoints[0] = transform.position;
        _contactPoints[1] = DynamicTarget.position + offset;
        _ropeRenderer.UpdateContactPoints(_contactPoints);
    }
}
