using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuButtonAnimator : MonoBehaviour
{
    RectTransform thisRectTransform;
    public bool right;
    public bool vertical;
    public RectTransform rightAnchor;
    public RectTransform leftAnchor;
    public float speed;
    public static Transform charTopAnchor;
    public float yPos;

    public GameObject text;

    Vector2 startPos;
    private void Start()
    {
        if (text != null){
            text.SetActive(false);
            StartCoroutine(EnableGraphics());
        }
        thisRectTransform = gameObject.GetComponent<RectTransform>();
        if (charTopAnchor == null) { charTopAnchor = GameObject.Find("CharacterTop").transform; }
        startPos = thisRectTransform.anchoredPosition;
    }
    IEnumerator EnableGraphics()
    {
        yield return new WaitForSeconds(1);
        text.SetActive(true);
    }
    private void Update()
    {
        if (charTopAnchor.position.y > yPos){
            if (vertical){
                thisRectTransform.anchoredPosition = new Vector2(thisRectTransform.anchoredPosition.x, startPos.y+310);
            }
            else{
                if (right) { thisRectTransform.anchoredPosition = new Vector2(rightAnchor.anchoredPosition.x + 250, thisRectTransform.anchoredPosition.y); }
                else { thisRectTransform.anchoredPosition = new Vector2(leftAnchor.anchoredPosition.x - 250, thisRectTransform.anchoredPosition.y); }
            }
        }
        else{
            if (vertical) { thisRectTransform.anchoredPosition = new Vector2(thisRectTransform.anchoredPosition.x, Mathf.Lerp(thisRectTransform.anchoredPosition.y, startPos.y, Time.deltaTime * speed)); }
            else { thisRectTransform.anchoredPosition = new Vector2(Mathf.Lerp(thisRectTransform.anchoredPosition.x, 0, Time.deltaTime * speed), thisRectTransform.anchoredPosition.y); }
        }
    }
}
