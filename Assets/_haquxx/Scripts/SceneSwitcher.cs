using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class SceneSwitcher : MonoBehaviour
{
    private int _currentIndex = -1;
    private int[] _activeSceneList = new int[3];

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

    public void LoadScene(Vector3Int indexList)
    {
        Debug.LogFormat("Load new content : {0},{1},{2}", indexList.x, indexList.y, indexList.z);

        var tempList = new int[3] { indexList.x, indexList.y, indexList.z };

        for (int i = 0; i < _activeSceneList.Length; i++)
        {
            if (Array.IndexOf(tempList, _activeSceneList[i]) == -1 && _activeSceneList[i] != 0)
            {
                SceneManager.UnloadSceneAsync(_activeSceneList[i]);
            }
        }

        for (int i = 0; i < tempList.Length; i++)
        {
            var index = Array.IndexOf(_activeSceneList, tempList[i]);
            if (index == -1 && tempList[i] != 0)
            {
                SceneManager.LoadScene(tempList[i], LoadSceneMode.Additive);
            }
        }

        _activeSceneList = new int[3] { indexList.x, indexList.y, indexList.z };

        if (_logText != null)
        {
            _logText.text = string.Format("[{0}] : {1},{2},{3}", DateTime.Now, indexList.x, indexList.y, indexList.z);
        }
    }
}
