using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ReflectionCamera : MonoBehaviour
{
    //[SerializeField]
    private Camera _mainCamera;

    [SerializeField]
    private GameObject _reflectionObj;

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        if (_mainCamera == null) return;

        var pos = _mainCamera.transform.position;
        var rot = _mainCamera.transform.eulerAngles;

        var diff = _mainCamera.transform.position.y - _reflectionObj.transform.position.y;

        pos.y = _reflectionObj.transform.position.y - diff;
        rot.x *= -1;
        rot.z *= -1;

        transform.position = pos;
        transform.rotation = Quaternion.Euler(rot);
    }
}
