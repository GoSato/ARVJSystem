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

public class DisplayManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _hideObjects;
    [SerializeField]
    private Canvas _settingCanvas;
    [SerializeField]
    private ARPlaneManager _arPlaneManager;

    private bool _isSettingMode = true;

    public ARDisplayMode ARDisplayMode
    {
        get
        {
            if (_isSettingMode)
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
                HideObjects();
                break;
            case ARDisplayMode.Setting:
                ShowObjects();
                break;
        }
    }

    private void ShowObjects()
    {
        _hideObjects.ForEach(obj => obj.SetActive(true));
        _arPlaneManager.enabled = true;
        foreach (var plane in _arPlaneManager.trackables)
            plane.gameObject.SetActive(true);
        _settingCanvas.enabled = true;
    }

    private void HideObjects()
    {
        _hideObjects.ForEach(obj => obj.SetActive(false));
        _arPlaneManager.enabled = false;
        foreach (var plane in _arPlaneManager.trackables)
            plane.gameObject.SetActive(false);
        _settingCanvas.enabled = false;
    }
}