using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CamViewEdgeCanvasScale : MonoBehaviour
{
    Plane[] cameraFrustum;
    public Collider go;
    public Vector3 velocity;
    public RectTransform rectTransform;
    [HideInInspector] public float moveMultiplier;
    [HideInInspector] public bool forward;
    string sceneName;
    private void Start()
    {
        moveMultiplier = 2;
        cameraFrustum = GeometryUtility.CalculateFrustumPlanes(Camera.main);
        sceneName = SceneManager.GetActiveScene().name;
    }
    private void Update()
    {
        //if (sceneName == "Customize") { cameraFrustum = GeometryUtility.CalculateFrustumPlanes(Camera.main); }
        var bounds = go.bounds;
        if (GeometryUtility.TestPlanesAABB(cameraFrustum, bounds))
        {
            if (!forward) { moveMultiplier *= 0.5f; }
            forward = true;
            go.transform.position += velocity*moveMultiplier;
            rectTransform.sizeDelta += new Vector2(-(velocity.x*moveMultiplier)*180, 0);
        }
        else
        {
            if (forward) { moveMultiplier *= 0.5f; forward = false; }
            go.transform.position -= velocity * moveMultiplier;
            rectTransform.sizeDelta -= new Vector2(-(velocity.x * moveMultiplier) * 180, 0);
        }
    }
}
