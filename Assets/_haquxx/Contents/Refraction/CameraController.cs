using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Vector3 _axis;
    [SerializeField] private float _speed = 10f;

    private void Update()
    {
        transform.Rotate(_axis, _speed * Time.deltaTime);
    }
}
