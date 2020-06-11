using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameRate : MonoBehaviour
{
    [SerializeField]
    private int _frameRate = 30;

    private void Awake()
    {
        Application.targetFrameRate = _frameRate;
    }
}