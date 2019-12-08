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

    private int _index = 1;

    private enum EffectType
    {
        NONE,
        MONOCHROME,
        NEGAPOSI,
        PASTEL,
        EDGE,
        MOSAIC,
        COLORBALANCE,
    }

    // Start is called before the first frame update
    void Start()
    {
        _toggle.onValueChanged.AddListener(OnToggleValueChanged);
        _mat.EnableKeyword("BACKGROUND");
        _mat.EnableKeyword("MONOCHROME");
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

    public void OnEffectClick(int index)
    {
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
        }
        _index = index;
    }
}
