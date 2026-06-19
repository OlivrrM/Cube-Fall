using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadAssetsInstance : MonoBehaviour
{
    public GameObject[] dontDestroyOnLoad;
    private void Awake()
    {
        for (int i = 0; i < dontDestroyOnLoad.Length; i++)
        {
            DontDestroyOnLoad(dontDestroyOnLoad[i]);
        }
    }
    private void Start()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
