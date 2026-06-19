using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class LeaderboardPlayerSelect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public int ID;
    public LeaderboardManager_LL leaderboardManagerScript;
    public Image image;
    public float highlightedOpacity;
    public float highlightedOpacitySpeed;
    public float notHighlightedOpacitySpeed;
    public Color thisPlayerColour;
    public PlaySound hoverSFX;
    public PlayerProfileManager playerProfileManagerScript;
    bool highlighted;
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        if ((ID) <= leaderboardManagerScript.playersCurrentlyDisplayed) { 
            highlighted = true;
            hoverSFX.Play(true, 1f - ((float)ID / 20f)+0.25f); 
        }
    }
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        highlighted = false;
    }
    public void OnPointerDown(PointerEventData pointerEventData)
    {
        if (highlighted) { playerProfileManagerScript.SelectProfile(ID);}
    }
    private void Update()
    {
        if (highlighted) { image.color = new Color(image.color.r, image.color.g, image.color.b, Mathf.Lerp(image.color.a, highlightedOpacity, Time.deltaTime*highlightedOpacitySpeed)); }
        else { image.color = new Color(image.color.r, image.color.g, image.color.b, Mathf.Lerp(image.color.a, 0, Time.deltaTime*notHighlightedOpacitySpeed)); }
        if ((ID-1) == leaderboardManagerScript.thisPlayerDisplayID) { image.color = new Color(thisPlayerColour.r, thisPlayerColour.g, thisPlayerColour.b, image.color.a); }
    }
}
