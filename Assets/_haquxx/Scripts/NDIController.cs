using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NDIController : MonoBehaviour
{
    [SerializeField]
    private GameObject _ndiSender;

    [SerializeField]
    private Toggle _toggle;

    private void Start()
    {
        _toggle.onValueChanged.AddListener(SetActive);
    }

    public void SetActive(bool active)
    {
        _ndiSender.SetActive(active);
    }
}
