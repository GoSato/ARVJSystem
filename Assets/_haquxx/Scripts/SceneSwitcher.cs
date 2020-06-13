using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    private int _currentIndex = -1;

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
    }
}
