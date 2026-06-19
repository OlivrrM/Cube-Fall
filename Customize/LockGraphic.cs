using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LockGraphic : MonoBehaviour
{
    
    public GameObject newLock;
    public Coin coinScript;
    public GameObject currentLock;

    public CustomizeInstance customizeInstanceScript;

    float timeSinceUnlock;

    public Transform coinTransform;

    public CustomizeMenuSelector customizeMenuSelectorScript;

    float highlightedIndexOnTap;

    private void Start()
    {
        currentLock = GameObject.Find("Lock");
    }
    public void Unlock()
    {
        coinScript.transform.parent = GameObject.Find("Canvas").transform;
        coinScript.Collect();
        currentLock.GetComponent<LockText>().Collect();
        Invoke("NewLock", 1);
    }
    void NewLock()
    {
        GameObject newLockGO = Instantiate(newLock, transform.position, Quaternion.identity,transform);
        coinScript = newLockGO.GetComponent<Coin>();
        currentLock = newLockGO;
    }
    private void Update()
    {
        timeSinceUnlock += Time.deltaTime;
        if (Input.GetMouseButtonDown(0))
        {
            highlightedIndexOnTap = customizeMenuSelectorScript.currentlySelected;
        }
    }
    private void OnMouseUp()
    {
        float a = customizeMenuSelectorScript.currentlySelected; //7
        float b = customizeInstanceScript.skinsUnlocked; //6
        float c = highlightedIndexOnTap; //7


        //if ((a == (b + 1f)) && (c == (b+ 1f)))
        if (Camera.main.transform.position.x<transform.position.x+1&& Camera.main.transform.position.x > transform.position.x - 1)
        {
            if (customizeInstanceScript.navigateVelocity < 1f && customizeInstanceScript.navigateVelocity > -1f)
            {
                if (Application.isMobilePlatform)
                {
                    if (Input.touchCount > 0)
                    {
                        if (Input.GetTouch(0).deltaPosition.x == 0)
                        {
                            if (EncryptedPlayerPrefs.GetInt("Coins") >= customizeInstanceScript.cost)
                            {
                                if (timeSinceUnlock > 1)
                                {
                                    customizeInstanceScript.PurchaseNewSkin();
                                    timeSinceUnlock = 0;
                                    coinTransform.localScale = new Vector3(coinTransform.localScale.x * 0.75f, coinTransform.localScale.y * 0.75f, coinTransform.localScale.z * 0.75f);
                                }
                            }
                            else
                            {
                                currentLock.GetComponent<LockText>().InsufficientFunds();
                            }
                        }
                    }
                }
                else
                {
                    if (Input.GetMouseButtonUp(0))
                    {
                        if (EncryptedPlayerPrefs.GetInt("Coins") >= customizeInstanceScript.cost)
                        {
                            if (timeSinceUnlock > 1)
                            {
                                customizeInstanceScript.PurchaseNewSkin();
                                timeSinceUnlock = 0;
                                coinTransform.localScale = new Vector3(coinTransform.localScale.x * 0.25f, coinTransform.localScale.y * 0.25f, coinTransform.localScale.z * 0.25f);
                            }
                        }
                        else
                        {
                            currentLock.GetComponent<LockText>().InsufficientFunds();
                        }
                    }
                }
            }
        }
    }
}
