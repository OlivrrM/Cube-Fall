using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomizeMenuSelector : MonoBehaviour
{
    public Transform cam;
    public Skin selectedSkin;
    public float snapSpeed;
    float defaultSnapSpeed;

    float timeOffTouch;

    public float previouslySelected;
    public float currentlySelected;

    public CustomizeInstance customizeInstanceScript;

    public GameObject navigateSFX;

    float screenXMultiplier;
    float selectSnapScreenSizeMultiplier;
    private void Start()
    {
        selectSnapScreenSizeMultiplier = 1f;
        defaultSnapSpeed = snapSpeed;
        previouslySelected = 404;
    }
    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 9)
        {
            //selectedSkin = collision.transform.parent.GetComponent<Skin>();
            //collision.transform.parent.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 500));
        }
    }
    */
    private void Update()
    {
        if (!Input.GetMouseButton(0)&&StartTime.instanceTime>2f)
        {
            timeOffTouch += Time.deltaTime;
            if (timeOffTouch>0.15f&& currentlySelected>=0&&currentlySelected<=customizeInstanceScript.skinsUnlocked+2)
            {
                snapSpeed = Mathf.Lerp(snapSpeed, defaultSnapSpeed, Time.deltaTime * (defaultSnapSpeed/3));
                cam.position = Vector3.Lerp(cam.position, new Vector3(Mathf.Round(cam.position.x / (2.8f* selectSnapScreenSizeMultiplier)) * (2.8f* selectSnapScreenSizeMultiplier), cam.position.y, cam.position.z), Time.deltaTime * snapSpeed);
            }
        }
        else
        {
            timeOffTouch = 0;
            snapSpeed = 0;
        }
        currentlySelected = (Mathf.Round(cam.position.x / (2.8f* selectSnapScreenSizeMultiplier)) * (2.8f* selectSnapScreenSizeMultiplier)) / (2.8f* selectSnapScreenSizeMultiplier);
        if (currentlySelected != previouslySelected){
            GameObject go = GameObject.Find("Skin" + currentlySelected.ToString());
            if (go != null){
                if (previouslySelected != 404) { 
                    go.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 200));
                    if (StartTime.instanceTime > 0.25f){
                        GameObject sfx = Instantiate(navigateSFX, go.transform.position, Quaternion.identity);
                        PlaySound playSound = sfx.GetComponent<PlaySound>();
                        playSound.pitchA = Mathf.Clamp(1.5f + (currentlySelected / 20), 1.5f, 2f);
                        playSound.pitchB = Mathf.Clamp(1.5f + (currentlySelected / 20), 1.5f, 2f);
                        HapticFeedbackPlayer.Play(0);
                    }
                }
                selectedSkin.selectCharButton.targetSize = Vector3.zero;
                selectedSkin = go.GetComponent<Skin>();
                selectedSkin.selectCharButton.targetSize = Utilities.V3All(1);

            }
        }
        previouslySelected = currentlySelected;
    }
}
