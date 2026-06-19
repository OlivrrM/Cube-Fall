using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Notificationify : MonoBehaviour
{
    public Image image;
    public Color notificationCol;
    public Animator[] animators;
    public string notificationName;
    private void Awake()
    {
        if (PlayerPrefs.GetInt(notificationName) == 1){
            image.color = notificationCol;
            for (int i = 0; i < animators.Length; i++){
                animators[i].enabled = true;
            }
        }
        else{
            for (int i = 0; i < animators.Length; i++){
                animators[i].enabled = false;
            }
        }
    }
}
