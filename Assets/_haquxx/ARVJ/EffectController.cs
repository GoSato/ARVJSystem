using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectController : MonoBehaviour
{
    [SerializeField]
    private bool _applyReverse = false;

    [SerializeField]
    private Material _mat;

    [SerializeField]
    private Toggle _toggle;

    private int _index = 0;

    [SerializeField]
    private bool _auto = false;

    [SerializeField]
    private Toggle _autoToggle;

    [SerializeField]
    private float _interval = 1.0f;

    [SerializeField]
    private GameObject _arObjects;

    [SerializeField]
    private Toggle _arToggle;

    private float _elapsedTime;

    private enum EffectType
    {
        NONE,
        MONOCHROME,
        NEGAPOSI,
        PASTEL,
        EDGE,
        MOSAIC,
        COLORBALANCE,
        RGBSHIFT,
    }

    // Start is called before the first frame update
    void Start()
    {
        _toggle.onValueChanged.AddListener(OnToggleValueChanged);
        _mat.EnableKeyword("PEOPPLE");
        _mat.EnableKeyword("NONE");

        _autoToggle.onValueChanged.AddListener(ChangeAuto);
        _arToggle.onValueChanged.AddListener(ChangeAR);
    }

    private void Update()
    {
        if(_auto)
        {
            _elapsedTime += Time.deltaTime;
            if(_elapsedTime >= _interval)
            {
                ChangeEffect(UnityEngine.Random.Range(1, Enum.GetNames(typeof(EffectType)).Length));
                _elapsedTime = 0f;
            }
        }
    }

    private void OnToggleValueChanged(bool isOn)
    {
        Debug.Log("Toggle Changed");
        if (isOn)
        {
            _mat.EnableKeyword("BACKGROUND");
            _mat.DisableKeyword("PEOPPLE");
        }
        else
        {
            _mat.DisableKeyword("BACKGROUND");
            _mat.EnableKeyword("PEOPPLE");
        }
    }

    private void ChangeAuto(bool isOn)
    {
        _auto = isOn;
    }

    private void ChangeAR(bool isOn)
    {
        _arObjects.SetActive(isOn);
    }

    public void ChangeEffect(int index)
    {
        if (index == _index) return;

        var type = Enum.GetName(typeof(EffectType), _index);
        Debug.Log("Disable " + type);
        _mat.DisableKeyword(type);
        switch(index)
        {
            case 0:
                Debug.Log("NONE");
                break;
            case 1:
                Debug.Log("MONOCHROME");
                _mat.EnableKeyword("MONOCHROME");
                break;
            case 2:
                Debug.Log("NEGAPOSI");
                _mat.EnableKeyword("NEGAPOSI");
                break;
            case 3:
                Debug.Log("PASTEL");
                _mat.EnableKeyword("PASTEL");
                break;
            case 4:
                Debug.Log("EDGE");
                _mat.EnableKeyword("EDGE");
                break;
            case 5:
                Debug.Log("MOSAIC");
                _mat.EnableKeyword("MOSAIC");
                break;
            case 6:
                Debug.Log("COLORBALANCE");
                _mat.EnableKeyword("COLORBALANCE");
                break;
            case 7:
                Debug.Log("RGBSHIFT");
                _mat.EnableKeyword("RGBSHIFT");
                break;
        }
        _index = index;
    }
}
