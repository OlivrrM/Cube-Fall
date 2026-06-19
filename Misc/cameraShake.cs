using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraShake : MonoBehaviour 
{
    public static Vector3 currentVelocity;
    public static float currentDecaySpeed;
    public static bool enabled = true;
    private void Start()
    {
        enabled = !Utilities.IntToBool(PlayerPrefs.GetInt("CameraShakeDisabled"));
    }
    public static void Shake(Vector3 velocity, float decaySpeed)
    {
        if (enabled){
            currentVelocity += velocity;
            currentDecaySpeed = decaySpeed;
        }
    }
    private void FixedUpdate()
    {
        transform.position = new Vector3(Random.Range(-currentVelocity.x, currentVelocity.x), Random.Range(-currentVelocity.y, currentVelocity.y), Random.Range(-currentVelocity.z, currentVelocity.z));
        currentVelocity = Vector3.Lerp(currentVelocity, new Vector3(0, 0, 0), Time.deltaTime * currentDecaySpeed);
    }
}
