using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ARVJ
{
    [RequireComponent(typeof(ARWorldAnchor))]
    public class ARWorldAnchorController : MonoBehaviour
    {
        [SerializeField]
        private Slider _slider;
        [SerializeField]
        private Dropdown _dropdown;

        private ARWorldAnchor _arWorldAnchor;

        private void Awake()
        {
            _arWorldAnchor = GetComponent<ARWorldAnchor>();
        }

        public void OnSliderValueChanged()
        {
            _arWorldAnchor.Rotation = Quaternion.AngleAxis(_slider.value, Vector3.up);
        }

        public void OnDropdownValueChanged()
        {
            var type = (ARWorldAnchor.AnchorPositionType)_dropdown.value;
            switch (type)
            {
                case ARWorldAnchor.AnchorPositionType.Left:
                    _arWorldAnchor.Position = new Vector3(ARWorldAnchor.LEFT_POSITION, 0f, 0f);
                    break;
                case ARWorldAnchor.AnchorPositionType.Center:
                    _arWorldAnchor.Position = new Vector3(ARWorldAnchor.CENTER_POSITION, 0f, 0f);
                    break;
                case ARWorldAnchor.AnchorPositionType.Right:
                    _arWorldAnchor.Position = new Vector3(ARWorldAnchor.RIGHT_POSITION, 0f, 0f);
                    break;
            }
        }
    }
}