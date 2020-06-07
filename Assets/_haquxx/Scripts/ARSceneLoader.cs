using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ARSceneLoader : MonoBehaviour
{
    [SerializeField]
    private List<string> _sceneList;

    private void Start()
    {
        for (int i = 0; i < _sceneList.Count; i++)
        {
            SceneManager.LoadScene(_sceneList[i], LoadSceneMode.Additive);
        }
    }
}
