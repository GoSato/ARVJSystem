using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;

namespace ARVJ
{   
    [RequireComponent(typeof(ARSessionOrigin))]
    [RequireComponent(typeof(ARRaycastManager))]
    public class ARWorldAnchor : MonoBehaviour
    {
        public enum AnchorPositionType
        {
            Left,
            Center,
            Right
        }

        public static readonly float LEFT_POSITION = -4.3f;
        public static readonly float CENTER_POSITION = 0.0f;
        public static readonly float RIGHT_POSITION = 4.3f;

        [SerializeField]
        private Transform _arAnchor;
        [SerializeField]
        private Text _statusText;
        [SerializeField]
        private AnchorPositionType _anchorPositionType = AnchorPositionType.Center;

        private ARSessionOrigin _arSessionOrigin;
        private ARRaycastManager _arRaycastManger;
        private bool _isLocked = false;
        private List<ARRaycastHit> hits = new List<ARRaycastHit>();

        private Vector3 _position;
        public Vector3 Position
        {
            get { return _position; }
            set
            {
                _position = value;
                if (_arSessionOrigin != null)
                {
                    _arAnchor.transform.position = _position;
                    _arSessionOrigin.MakeContentAppearAt(_arAnchor, _arAnchor.transform.position, _arAnchor.transform.rotation);
                    Debug.Log(_position);
                }
            }
        }

        private Quaternion _rotation;
        public Quaternion Rotation
        {
            get { return _rotation; }
            set
            {
                _rotation = value;
                if (_arSessionOrigin != null)
                    _arSessionOrigin.MakeContentAppearAt(_arAnchor, _arAnchor.transform.position, _rotation);
            }
        }

        private void Awake()
        {
            _arSessionOrigin = GetComponent<ARSessionOrigin>();
            _arRaycastManger = GetComponent<ARRaycastManager>();
        }

        private void Update()
        {
            if (_isLocked) return;

            if (_arRaycastManger.Raycast(new Vector2(Screen.width / 2.0f, Screen.height / 2.0f), hits, TrackableType.PlaneWithinPolygon))
            {
                Pose pose = hits[0].pose;

                // Move ARSessionOrigin, not content
                _arSessionOrigin.MakeContentAppearAt(_arAnchor, pose.position, _rotation);
            }
        }

        public void LockAnchor()
        {
            _isLocked = true;
            _statusText.text = "Locked";
            _statusText.color = Color.red;
        }

        public void UnlockAnchor()
        {
            _isLocked = false;
            _statusText.text = "Unlocked";
            _statusText.color = Color.green;
        }
    }
}