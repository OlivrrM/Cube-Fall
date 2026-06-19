using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamViewEdgePos : MonoBehaviour
{
    Plane[] cameraFrustum;
    public Collider go;
    public Vector3 velocity;

    bool hitEdge;
    bool done;
    private void Start()
    {
        cameraFrustum = GeometryUtility.CalculateFrustumPlanes(Camera.main);
    }
    private void Update()
    {
        var bounds = go.bounds;
        if (GeometryUtility.TestPlanesAABB(cameraFrustum, bounds)){
            go.transform.position += velocity;
        }
        /*
        if (GeometryUtility.TestPlanesAABB(cameraFrustum, bounds)){
            if (!hitEdge) { go.transform.position += velocity; }
            else if (!done) { done = true; }
        }
        else{
            if (!done){
                hitEdge = true;
                go.transform.position -= velocity / 5;
            }
        }
        */
    }
}
