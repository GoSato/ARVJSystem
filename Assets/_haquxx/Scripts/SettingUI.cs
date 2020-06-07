using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.XR.ARFoundation;

public enum ARDisplayMode
{
    Setting,
    Content,
}

public class SettingUI : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _debugObjects;

    private bool _isSettingMode = true;

    private List<GameObject> _debugObjectsInstances = new List<GameObject>();
    [SerializeField]
    private Canvas _settingCanvas;

    [SerializeField]
    private ARPlaneManager _arPlaneManager;

    public ARDisplayMode ARDisplayMode
    {
        get
        {
            if(_isSettingMode)
            {
                return ARDisplayMode.Setting;
            }
            else
            {
                return ARDisplayMode.Content;
            }
        }
    }

    public void ChangeDisplayMode()
    {
        _isSettingMode = !_isSettingMode;
        switch (ARDisplayMode)
        {
            case ARDisplayMode.Content:
                HideDebugObects();
                break;
            case ARDisplayMode.Setting:
                ShowDebugObjects();
                break;
        }
    }

    private void ShowDebugObjects()
    {
        _debugObjectsInstances.ForEach(obj => obj.SetActive(true));
        _arPlaneManager.enabled = true;
        foreach (var plane in _arPlaneManager.trackables)
            plane.gameObject.SetActive(true);
        _settingCanvas.enabled = true;
    }

    private void HideDebugObects()
    {
        if (_debugObjectsInstances.Count == 0)
        {
            GetDebugObjects();
        }

        _debugObjectsInstances.ForEach(obj => obj.SetActive(false));
        _arPlaneManager.enabled = false;
        foreach (var plane in _arPlaneManager.trackables)
            plane.gameObject.SetActive(false);
        _settingCanvas.enabled = false;
    }

    private void GetDebugObjects()
    {
        foreach (var s in _debugObjects)
        {
            var name = s.name;
            name += "(Clone)";
            var obj = GameObject.Find(name);
            if (obj != null)
            {
                _debugObjectsInstances.Add(obj);
            }
        }
    }
}
