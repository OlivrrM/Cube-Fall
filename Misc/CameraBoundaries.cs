using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBoundaries : MonoBehaviour //UNUSED
{
    public GameObject xBoundaries;
    public GameObject yBoundaries;

    Plane[] cameraFrustum;
    bool setX;
    bool setY;

    Collider xCollider;
    Collider yCollider;

    public static bool setBoundaries;
    public static Vector2 boundaries;
    private void Start()
    {
        setBoundaries = false;
        boundaries = new Vector2(0, 0);
        /*
        if (setBoundaries) {
            setX = true;
            setY = true;
            xBoundaries.transform.position = new Vector3(boundaries.x, 0, 0);
            yBoundaries.transform.position = new Vector3(0, boundaries.y, 0);
        }
        */
        xCollider = xBoundaries.GetComponent<Collider>();
        yCollider = yBoundaries.GetComponent<Collider>();
    }

    private void FixedUpdate()
    {
        if (!setX || !setY)
        {
            cameraFrustum = GeometryUtility.CalculateFrustumPlanes(Camera.main);
            if (!setX)
            {
                var bounds = xCollider.bounds;
                if (GeometryUtility.TestPlanesAABB(cameraFrustum, bounds))
                {
                    xBoundaries.transform.position += new Vector3(1, 0, 0);
                }
                else
                {
                    boundaries.x = xBoundaries.transform.position.x;
                    setX = true;
                }
            }
            if (!setY)
            {
                var bounds = yCollider.bounds;
                if (GeometryUtility.TestPlanesAABB(cameraFrustum, bounds))
                {
                    yBoundaries.transform.position += new Vector3(0, -1, 0);
                }
                else
                {
                    boundaries.y = yBoundaries.transform.position.y;
                    setY = true;
                }
            }
        }
        else
        {
            setBoundaries = true;
            boundaries = new Vector2(xBoundaries.transform.position.x, yBoundaries.transform.position.y);
        }

    }
}
