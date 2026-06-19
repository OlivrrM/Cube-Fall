using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawnPosSetter : MonoBehaviour
{
    Plane[] cameraFrustum;
    public Collider platformSpawnPos;
    void Start()
    {
        cameraFrustum = GeometryUtility.CalculateFrustumPlanes(Camera.main);
        int LOOP_BREAKER = 0;
        while (true)
        {
            var bounds = platformSpawnPos.bounds;
            if (GeometryUtility.TestPlanesAABB(cameraFrustum, bounds))
            {
                platformSpawnPos.transform.position += new Vector3(0, -1, 0);
                print(LOOP_BREAKER);
            }
            LOOP_BREAKER++;
            if (LOOP_BREAKER > 50)
            {
                break;
            }
        }
    }
}
