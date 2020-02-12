using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OSCEffectController : MonoBehaviour
{
    private EffectController _effectController;
    private OSCReceiver _oscReceiver;

    private void Start()
    {
        _effectController = FindObjectOfType<EffectController>();
        _oscReceiver = FindObjectOfType<OSCReceiver>();

        if (_oscReceiver == null || _effectController == null) return;

        _oscReceiver.OnUpdate += HandleOSCData;
    }

    private void HandleOSCData(OSCData data)
    {
        int index = 0;

        switch (data.Address)
        {
            case "/1/recenable":
                index = 1;
                break;
            case "/1/input":
                index = 2;
                break;
            case "/1/solo":
                index = 3;
                break;
        }

        _effectController.ChangeEffect(index);
    }
}
