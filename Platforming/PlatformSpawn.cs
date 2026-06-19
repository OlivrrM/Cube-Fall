using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawn : MonoBehaviour
{
    public GameObject spawnPlatformsPos;

    public GameObject[] platforms;
    public GameObject[] platformsPC;
    public float spawnTime;
    public float pcSpawnTime;
    float currentSpawnTime;

    public GameObject platformsBin;

    public static int platformsActive;

    bool mobile;

    public CameraMove cameraMoveScript;
    private void Start()
    {
        platformsActive = 3;
        if (Cache.isMobile) mobile = true;
        //StartCoroutine(CheckMobile());
    }
    IEnumerator CheckMobile()
    {
        yield return new WaitForSeconds(0.25f);
        /*
        if (Screen.width > Screen.height)
        {
            spawnTime = pcSpawnTime;
            print("(PlatformSpawn.cs) Set spawnTime to pcSpawnTime because PC aspect ratio was detected");
            platforms = platformsPC;
            print("(PlatformSpawn.cs) Set platforms to platformsPC because PC aspect ratio was detected");
        }
        */
    }
    private void Update()
    {
        if (CameraBoundaries.setBoundaries)
        {
            //spawnPlatformsPos.transform.position = new Vector3(0, CameraBoundaries.boundaries.y, 0);
            currentSpawnTime += Time.deltaTime * cameraMoveScript.firstLevelMultiplier;
            if (currentSpawnTime >= spawnTime && Cache.player.transform.position.y > Camera.main.transform.position.y - 7.5f)
            {
                Vector3 spawnPos = new Vector3(0, 0, 0);
                if (Level.easyPlatforms > 0){
                    spawnPos = new Vector3((Random.Range(spawnPlatformsPos.transform.position.x - (CameraBoundaries.boundaries.x + 2) / 2, spawnPlatformsPos.transform.position.x + (CameraBoundaries.boundaries.x + 2) / 2))/12, spawnPlatformsPos.transform.position.y - 1, spawnPlatformsPos.transform.position.z);
                    Level.easyPlatforms--;
                }
                else if (Level.mediumPlatforms > 0)
                {
                    spawnPos = new Vector3((Random.Range(spawnPlatformsPos.transform.position.x - (CameraBoundaries.boundaries.x + 2) / 2, spawnPlatformsPos.transform.position.x + (CameraBoundaries.boundaries.x + 2) / 2)) / 4, spawnPlatformsPos.transform.position.y - 1, spawnPlatformsPos.transform.position.z);
                    Level.mediumPlatforms--;
                }
                else{
                    spawnPos = new Vector3(Random.Range(spawnPlatformsPos.transform.position.x - (CameraBoundaries.boundaries.x + 2) / 2, spawnPlatformsPos.transform.position.x + (CameraBoundaries.boundaries.x + 2) / 2), spawnPlatformsPos.transform.position.y - 1, spawnPlatformsPos.transform.position.z);
                }
                Instantiate(platforms[Random.Range(0, platforms.Length)], spawnPos, Quaternion.identity, platformsBin.transform);
                platformsActive++;
                currentSpawnTime = 0;
            }
        }
    }
}
