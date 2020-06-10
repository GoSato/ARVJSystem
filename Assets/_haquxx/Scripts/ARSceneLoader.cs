using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ARSceneLoader : MonoBehaviour
{
    [SerializeField]
    private string _scene;

    [SerializeField]
    private Toggle _toggle;

    private void Start()
    {
        _toggle.onValueChanged.AddListener(SceneLoad);
    }

    public void SceneLoad(bool isOn)
    {
        if (isOn)
        {
            SceneManager.LoadScene(_scene, LoadSceneMode.Additive);
        }
        else
        {
            SceneManager.UnloadSceneAsync(_scene);
        }
    }
}
