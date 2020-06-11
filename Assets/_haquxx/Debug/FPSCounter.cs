using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour
{
    [SerializeField]
    private float _updateInterval = 0.5f;
    [SerializeField]
    private Text _text;

    private float timeleft = 0f;
    private float accum = 0f;
    private int frames;

    private void Update()
    {
        timeleft -= Time.deltaTime;
        accum += Time.timeScale / Time.deltaTime;
        ++frames;

        if (timeleft <= 0.0f)
        {
            timeleft = _updateInterval;

            if (_text != null)
            {
                var fps = accum / frames;
                _text.text = fps.ToString("f2");
                var col = fps >= 60.0f ? Color.green : Color.red;
                _text.color = col;
            }

            accum = 0.0f;
            frames = 0;
        }
    }
}