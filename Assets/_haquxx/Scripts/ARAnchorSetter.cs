using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System;
using UnityEngine.UI;

[RequireComponent(typeof(ARSessionOrigin))]
[RequireComponent(typeof(ARRaycastManager))]
public class ARAnchorSetter : MonoBehaviour
{
    [SerializeField]
    private GameObject _anchor;
    [SerializeField]
    private Slider _slider;
    [SerializeField]
    private Text _statusText;

    private ARSessionOrigin _arSessionOrigin;
    private ARRaycastManager _arRaycastManger;
    private Transform _anchorInstance;
    private bool _isLocked = false;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private void Start()
    {
        _arSessionOrigin = GetComponent<ARSessionOrigin>();
        _arRaycastManger = GetComponent<ARRaycastManager>();
        _anchorInstance = Instantiate(_anchor).transform;
    }

    private void Update()
    {
        if (_arRaycastManger.Raycast(new Vector2(Screen.width / 2.0f, Screen.height / 2.0f), hits, TrackableType.PlaneWithinPolygon))
        {
            Pose pose = hits[0].pose;

            if (!_isLocked)
            {
                // Move ARSessionOrigin, not content
                _arSessionOrigin.MakeContentAppearAt(_anchorInstance, pose.position, _rot);
            }
            else
            {
                _arSessionOrigin.MakeContentAppearAt(_anchorInstance, _rot);
            }
        }
    }

    public void LockAnchor()
    {
        _isLocked = true;
        _statusText.text = "Locked";
    }

    public void UnLockAnchor()
    {
        _isLocked = false;
        _statusText.text = "UnLocked";
    }

    private Quaternion _rot;

    public void UpdateRotation(Quaternion rot)
    {
        _rot = rot;
        _arSessionOrigin.MakeContentAppearAt(_anchorInstance, _anchorInstance.transform.position, _rot);
    }

    public void OnSliderValueChanged()
    {
        UpdateRotation(Quaternion.AngleAxis(_slider.value, Vector3.up));
    }
}
