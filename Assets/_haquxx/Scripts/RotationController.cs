using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ARVJ
{
    [RequireComponent(typeof(ARWorldAnchor))]
    public class RotationController : MonoBehaviour
    {
        [SerializeField]
        private Slider _slider;

        private ARWorldAnchor _arWorldAnchor;

        private void Awake()
        {
            _arWorldAnchor = GetComponent<ARWorldAnchor>();
        }

        public void OnSliderValueChanged()
        {
            _arWorldAnchor.Rotation = Quaternion.AngleAxis(_slider.value, Vector3.up);
        }
    }
}