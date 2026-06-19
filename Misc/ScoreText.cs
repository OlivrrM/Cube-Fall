using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreText : MonoBehaviour
{
    public Transform canvasTopAnchor;
    public TextMeshProUGUI scoreText;
    public static TextMeshProUGUI text;
    private void Awake()
    {
        text = scoreText;
    }
    private void FixedUpdate()
    {
        canvasTopAnchor.position = new Vector3(canvasTopAnchor.position.x, Camera.main.transform.position.y + (Camera.main.transform.position.y - CameraBoundaries.boundaries.y) - 3, canvasTopAnchor.position.z);
    }
    private void Update()
    {
        if (Cache.player.transform.position.y > text.transform.position.y - 2 && !Level.death && !Level.levelComplete){
            text.color = Color.Lerp(text.color, new Color(text.color.r, text.color.g, text.color.b, 0.5f), Time.deltaTime * 3);
        }
        else{
            text.color = Color.Lerp(text.color, new Color(text.color.r, text.color.g, text.color.b, 1), Time.deltaTime * 3);
        }
    }
}
