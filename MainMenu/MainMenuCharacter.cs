using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuCharacter : MonoBehaviour
{
    public Rigidbody2D rb;
    public GameObject hitFloorParticle;
    public GameObject hitFloorParticleInverted;
    public PlaySound hitFloorSFX;
    public Vector2 rotationVelocity;
    public TextMeshProUGUI hsText;

    bool started;
    public Transform floor0;
    public Transform floor1;
    public MainMenuButtonAnimator mainMenuButtonAnimatorScript;

    public Animator charAnimator;

    public SpriteRenderer characterSpriteRenderer;

    float blinkCD;
    private void Start()
    {
        float velocity = 0;
        while (true){
            velocity = Random.Range(rotationVelocity.x, rotationVelocity.y);
            if (velocity <-4f||velocity>4f) { break; }
        }
        rb.AddTorque(velocity);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (rb.velocity.magnitude > 2.5f)
        {
            //if (MainMenuScreenManager.selectedScreenID > 0) { Instantiate(hitFloorParticleInverted, new Vector3(collision.GetContact(0).point.x, collision.GetContact(0).point.y, rb.velocity.magnitude * (((0)))), Quaternion.Euler(-90, 0, 0)); }
            //else { Instantiate(hitFloorParticle, new Vector3(collision.GetContact(0).point.x, collision.GetContact(0).point.y, rb.velocity.magnitude * (((0)))), Quaternion.Euler(-90, 0, 0)); }
            GameObject particle = Instantiate(hitFloorParticle, new Vector3(collision.GetContact(0).point.x, collision.GetContact(0).point.y, rb.velocity.magnitude * (((0)))), Quaternion.Euler(-90, 0, 0));
            var main = particle.GetComponent<ParticleSystem>().main;
            if (MainMenuScreenManager.selectedScreenID > 0) { main.startColor = new Color(-characterSpriteRenderer.color.r+1, -characterSpriteRenderer.color.g + 1, -characterSpriteRenderer.color.b + 1); }
            else { main.startColor = characterSpriteRenderer.color; }
            hitFloorSFX.Play();
        }
    }
    private void Update()
    {
        if (started){
            floor0.position = new Vector3(Mathf.Lerp(floor0.position.x, mainMenuButtonAnimatorScript.leftAnchor.position.x-2f,Time.deltaTime*2.5f), floor0.position.y, floor0.position.z);
            floor1.position = new Vector3(Mathf.Lerp(floor1.position.x, mainMenuButtonAnimatorScript.rightAnchor.position.x+58, Time.deltaTime*2.5f), floor1.position.y, floor1.position.z);
        }
        blinkCD -= Time.deltaTime;
        if (SceneManager.GetActiveScene().name != "MainMenu"){
            transform.position = new Vector3(Mathf.Lerp(transform.position.x, 0, Time.deltaTime * 20), transform.position.y, transform.position.z);
        }
    }
    private void FixedUpdate()
    {
        if (Application.isEditor && Input.GetKeyDown(KeyCode.F)) { InstanceChange.LoadInstance(SceneManager.GetActiveScene().name, 20); }
        if (SceneManager.GetActiveScene().name == "MainMenu"){
            transform.position = new Vector3(Mathf.Lerp(transform.position.x, 0, Time.deltaTime * 20), transform.position.y, transform.position.z);
        }
    }
    public void Play()
    {
        started = true;
    }
    private void OnMouseDown()
    {
        if (1 == 0) //Disabled for now
        {
            if (blinkCD <= 0 && SceneManager.GetActiveScene().name == "MainMenu")
            {
                blinkCD = 0.5f;
                charAnimator.SetBool("Clicked", true);
                StartCoroutine(BlinkCooldown());
            }
        }
    }
    IEnumerator BlinkCooldown()
    {
        yield return new WaitForSeconds(0.2f);
        charAnimator.SetBool("Clicked", false);
    }
}
