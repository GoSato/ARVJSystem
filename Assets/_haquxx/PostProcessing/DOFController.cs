using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class DOFController : MonoBehaviour
{
    [SerializeField]
    private GameObject _postProcessingObject;

    [SerializeField]
    private Toggle _dofToggle;

    [SerializeField]
    private Slider _focusDistanceSlider;
    [SerializeField]
    private Slider _apertureSlider;
    [SerializeField]
    private Slider _focalLengthSlider;

    [SerializeField]
    private Text _focusDistanceText;
    [SerializeField]
    private Text _apertureText;
    [SerializeField]
    private Text _focalLengthText;

    private DepthOfField _dof;
    private PostProcessVolume _postProcessVolume;

    private void Start()
    {
        _dof = ScriptableObject.CreateInstance<DepthOfField>();
        _dof.enabled.Override(true);
        _dof.focusDistance.Override(_focusDistanceSlider.value);
        _dof.aperture.Override(_apertureSlider.value);
        _dof.focalLength.Override(_focalLengthSlider.value);
        _postProcessVolume = PostProcessManager.instance.QuickVolume(_postProcessingObject.layer, 100f, _dof);

        _dofToggle.onValueChanged.AddListener(OnDofToggleChanged);
        _focusDistanceSlider.onValueChanged.AddListener(OnFocusDistanceChanged);
        _apertureSlider.onValueChanged.AddListener(OnApertureChanged);
        _focalLengthSlider.onValueChanged.AddListener(OnFocalLengthChanged);

        OnFocusDistanceChanged(_focusDistanceSlider.value);
        OnApertureChanged(_apertureSlider.value);
        OnFocalLengthChanged(_focalLengthSlider.value);
    }

    public void OnDofToggleChanged(bool val)
    {
        _dof.enabled.Override(val);
    }

    public void OnFocusDistanceChanged(float val)
    {
        _dof.focusDistance.value = val;
        _focusDistanceText.text = val.ToString();
    }

    public void OnApertureChanged(float val)
    {
        _dof.aperture.value = val;
        _apertureText.text = val.ToString();
    }

    public void OnFocalLengthChanged(float val)
    {
        _dof.focalLength.value = val;
        _focalLengthText.text = val.ToString();
    }

    private void OnDestroy()
    {
        RuntimeUtilities.DestroyVolume(_postProcessVolume, true, true);
    }

}
