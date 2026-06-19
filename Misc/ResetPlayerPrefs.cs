using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResetPlayerPrefs : MonoBehaviour
{
    public float holdButtonTime;
    float currentHoldButtonTime;
    public GameObject holdingButtonObjectEnable;
    public KeyCode resetKey;

    public Slider resetDataProgressBar;
    private void Start()
    {
        resetDataProgressBar.value = 0;
        holdingButtonObjectEnable.SetActive(false);
    }
    private void Update()
    {
        if (Application.isEditor)
        {
            if (Input.GetKey(resetKey))
            {
                holdingButtonObjectEnable.SetActive(true);
                currentHoldButtonTime += Time.deltaTime;
                if (resetDataProgressBar != null) { resetDataProgressBar.value = currentHoldButtonTime / holdButtonTime; }
                if (currentHoldButtonTime >= holdButtonTime)
                {
                    PlayerPrefs.DeleteAll();
                    InstanceChange.LoadInstance(SceneManager.GetActiveScene().name, 1);
                }
            }
            else
            {
                if (resetDataProgressBar != null) { resetDataProgressBar.value = 0; }
                currentHoldButtonTime = 0;
                holdingButtonObjectEnable.SetActive(false);
            }
        }
    }
}
