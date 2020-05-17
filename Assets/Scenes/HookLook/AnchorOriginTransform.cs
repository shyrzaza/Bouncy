using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RopeRenderer))]
public class AnchorOriginTransform : MonoBehaviour
{
    public Transform DynamicTarget;

    public float StaticTargetOffset = 0f;

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
        var offset = (transform.position - DynamicTarget.position).normalized;
        _contactPoints[0] = transform.position + offset * -StaticTargetOffset;
        _contactPoints[1] = DynamicTarget.position + offset * DynamicTargetOffset;
        _ropeRenderer.UpdateContactPoints(_contactPoints);
    }
}
