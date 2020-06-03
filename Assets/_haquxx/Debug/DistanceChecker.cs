using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class DistanceChecker : MonoBehaviour
{
    [SerializeField]
    private TextMesh _text;

    private void Update()
    {
        var cameraPos = Camera.main.transform.position;
        cameraPos.y = transform.position.y;
        var distance = Vector3.Distance(transform.position, cameraPos);
        _text.text = distance.ToString();
    }
}
