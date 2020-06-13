using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class SceneSwitcher : MonoBehaviour
{
    private int _currentIndex = -1;

    [SerializeField]
    private string _oscAddress;
    [SerializeField]
    private Text _logText;

    public void LoadScene(int val)
    {
        if (val == _currentIndex)
            return;

        Debug.LogFormat("Load new content : {0}", val);
        SceneManager.LoadScene(val, LoadSceneMode.Additive);

        if (_currentIndex != -1)
        {
            SceneManager.UnloadSceneAsync(_currentIndex);
        }
        _currentIndex = val;

        if (_logText != null)
        {
            _logText.text = "[" + DateTime.Now + "]" + " : " + val;
        }
    }
}
