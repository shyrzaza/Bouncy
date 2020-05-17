using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class CycleHardness : MonoBehaviour
{
    private float _hardness = 0;
    private float _direction = 1f;

    void Update()
    {
        if (_hardness > 1.0)
        {
            _hardness = 1f;
            _direction = -1f;
        } else if (_hardness < -1f)
        {
            _hardness = -1f;
            _direction = 1f;
        }

        _hardness += Time.deltaTime * _direction * 0.5f;

        GetComponent<SpriteShapeRenderer>().material.SetFloat("_Hardness", _hardness);
    }
}
