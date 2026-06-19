using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomizeInstanceSelect : MonoBehaviour
{
    public CustomizeInstance customizeInstanceScript;
    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.name.Contains("_SelectChar"))
                {
                    GameObject.Find(hit.transform.name.Replace("_SelectChar", "")).GetComponent<Skin>().Select();
                    hit.transform.GetChild(0).gameObject.GetComponent<ButtonBob>().Bob();
                    customizeInstanceScript.SilentMainMenu();
                    HapticFeedbackPlayer.Play(0);
                }
        }
        }
    }
}
