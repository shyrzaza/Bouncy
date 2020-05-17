using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RopeRenderer : MonoBehaviour
{
    public float RestLength = 5;

    public float MaxVisualStretchLength = 8;

    public float WidthMultiplier = 4;

    public int RenderPoints = 30;

    public AnimationCurve BaseThiccness;

    public AnimationCurve StressThiccness;

    public Color RestColor = Color.white;

    public Color StressColor = Color.red;

    private Vector3[] _worldPoints;

    private LineRenderer _lineRenderer;

    private Keyframe[] _widthKeyframes;

    private GradientColorKey[] _colorKeys;

    private void Awake()
    {
        _worldPoints = new Vector3[RenderPoints];
        _lineRenderer = GetComponent<LineRenderer>();

        _lineRenderer.positionCount = RenderPoints;

        _widthKeyframes = Enumerable.Range(0, RenderPoints)
            .Select(i => new Keyframe((float) i / (float) (RenderPoints - 1), 1f)).ToArray();
        _lineRenderer.widthCurve = new AnimationCurve(_widthKeyframes);

        _lineRenderer.widthMultiplier = WidthMultiplier;

        _colorKeys = new[]
        {
            new GradientColorKey(RestColor, 0f),
            new GradientColorKey(RestColor, 0.5f),
            new GradientColorKey(RestColor, 1.0f)
        };

        _lineRenderer.enabled = false;
    }

    private void OnValidate()
    {
        if (null != _lineRenderer)
        {
            _lineRenderer.widthMultiplier = WidthMultiplier;
        }
    }

    public void UpdateContactPoints(IList<Vector3> contactPoints)
    {
        // PERF: THIS

        if (contactPoints.Count < 2)
        {
            _lineRenderer.enabled = false;
            return;
        }

        _lineRenderer.enabled = true;

        var segmentLengths = contactPoints.Zip(contactPoints.Skip(1), Vector3.Distance).ToArray();
        var totalLen = segmentLengths.Sum();

        var stress = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01((totalLen - RestLength) / (MaxVisualStretchLength - RestLength)));

        var lenAcc = 0.0f;
        var keyFrames = segmentLengths.Select((sLen, sId) =>
        {
            lenAcc += sLen;
            var kfTime = lenAcc / totalLen;
            var sEndPoint = contactPoints[sId + 1];
            return new[] { new Keyframe(kfTime, sEndPoint.x), new Keyframe(kfTime, sEndPoint.y), new Keyframe(kfTime, sEndPoint.z),  };
        }).Prepend(new [] { new Keyframe(0, contactPoints[0].x), new Keyframe(0, contactPoints[0].y), new Keyframe(0, contactPoints[0].z) }).ToArray();

        // Why the hell can't we avoid ToArray calls here??? #rigged #iwantrust
        var xPosCurve = new AnimationCurve(keyFrames.Select(kfs => kfs[0]).ToArray());
        var yPosCurve = new AnimationCurve(keyFrames.Select(kfs => kfs[1]).ToArray());
        var zPosCurve = new AnimationCurve(keyFrames.Select(kfs => kfs[2]).ToArray());

        for (int i = 0; i < RenderPoints; i++)
        {
            var t = (float) i / (float) (RenderPoints - 1);

            _worldPoints[i] = new Vector3(xPosCurve.Evaluate(t), yPosCurve.Evaluate(t), zPosCurve.Evaluate(t));

            _widthKeyframes[i] = new Keyframe(t, BaseThiccness.Evaluate(t) * Mathf.Lerp(1f, StressThiccness.Evaluate(t), stress));
        }

        _lineRenderer.widthCurve = new AnimationCurve(_widthKeyframes);

        _colorKeys[1] = new GradientColorKey(Color.Lerp(RestColor, StressColor, stress), 0.5f);
        var gradient = new Gradient();
        gradient.colorKeys = _colorKeys;
        _lineRenderer.colorGradient = gradient;

        _lineRenderer.SetPositions(_worldPoints.ToArray());
    }
}
