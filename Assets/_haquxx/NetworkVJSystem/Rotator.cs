using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField]
    private float _speed = 1.0f;
    [SerializeField]
    private Vector3 _applyAxis = Vector3.one;

    private void Update()
    {
        transform.Rotate(_applyAxis * _speed);
    }
}
