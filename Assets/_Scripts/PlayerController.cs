using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayerController : MonoBehaviour
{
    public Sprite spriteUp;
    public Sprite spriteDown;
    public Sprite spriteSide;

    public float speed = 5.0f;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    public GameObject dartPrefab;

    private float horizontal;
    private float vertical;

    private int storedDirection = 0;
    private const int DOWN = 0;
    private const int UP = 1;

    private int health = 20;
    private float invincibilityTime = 0.5f;
    private float invincibilityActivated = 0.0f;
    private int dartCounter = 5;
    private int floorsCleared = 0;
    private int dartDamage = 1;
    private int maxDarts = 5;
    public float stealth = 0;
    private int maxHealth = 20;

    public GameObject simpleDungeonGenerator;

    public GameObject healthBar;
    public Text uiText;
    public Image volumeImage;
    public Text volumeText;
    public Sprite volumeOn;
    public Sprite volumeOff;
    public Text gameOver;
    public Text clears;
    public GameObject reset;
    public Image upgradeImg;
    public Text upgradeText;

    public AudioSource audioSource;
    public AudioClip[] audioClips;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        simpleDungeonGenerator.GetComponent<RandomDungeonGenerator>().RunProceduralGeneration();
        uiText.text = "x" + dartCounter + "/" + maxDarts;
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(0, 0);
        spriteRenderer.flipX = false;
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        spriteRenderer.flipX = false;
        // Prioritize horizontal sprite if moving diagonally
        // If not moving, use last direction that is not horizontal
        if (storedDirection == UP)
            spriteRenderer.sprite = spriteUp;
        else if (storedDirection == DOWN)
            spriteRenderer.sprite = spriteDown;

        if (vertical > 0)
        {
            spriteRenderer.sprite = spriteUp;
            storedDirection = UP;
        }
        else if (vertical < 0)
        {
            spriteRenderer.sprite = spriteDown;
            storedDirection = DOWN;
        }
        
        if (horizontal > 0)
            spriteRenderer.sprite = spriteSide;
        else if (horizontal < 0)
        {
            spriteRenderer.sprite = spriteSide;
            spriteRenderer.flipX = true;
        }
        // Lock player rotation
        transform.rotation = Quaternion.Euler(0, 0, 0);

        // Mousse input
        if (Input.GetMouseButtonDown(0))
        {
            if (dartCounter > 0)
            {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                var dart = Instantiate(dartPrefab, transform.position, transform.rotation).GetComponent<DartBehavior>();
                dart.SetTarget(mousePos);
                dart.SetDamage(dartDamage);
                dartCounter--;
                uiText.text = "x" + dartCounter + "/" + maxDarts;
                AudioSource src = Instantiate(audioSource);
                src.clip = audioClips[0];
                src.Play();
                Destroy(src, 5.0f);
            }
        }

        // Volume controls
        if (Input.GetKeyDown(KeyCode.M))
        { 
            audioSource.mute = !audioSource.mute;
            // change volume icon
            if (audioSource.mute)
            {
                volumeImage.sprite = volumeOff;
            }
            else if (audioSource.volume > 0)
            {
                volumeImage.sprite = volumeOn;
            }
            
        }
        else if (Input.GetKeyDown(KeyCode.N))
        {
            
            audioSource.volume = Mathf.Min(1f, audioSource.volume + 0.1f);
            volumeText.text = audioSource.volume * 100 + "%";
            Debug.Log(audioSource.volume);
            if (audioSource.volume > 0)
            {
                volumeImage.sprite = volumeOn;
            }
            
        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            
            audioSource.volume = Mathf.Max(0f, audioSource.volume - 0.1f);
            volumeText.text = audioSource.volume * 100 + "%";
            if (audioSource.volume == 0)
            {
                volumeImage.sprite = volumeOff;
            }
            
        }
        
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal, vertical).normalized * speed;
    }

    public void TakeDamage(int damage)
    {
        if (Time.time - invincibilityActivated < invincibilityTime)
        {
            return;
        }
        health -= damage;
        healthBar.GetComponent<HealthBarControl>().SetHealth(health);
        if (health <= 0)
        {
            gameOver.enabled = true;
            clears.enabled = true;
            clears.text = "Floors cleared: " + floorsCleared;
            reset.SetActive(true);
            Time.timeScale = 0;
        }

    }

    public void Pickup(int damage, int type)
    {
        if (dartCounter < maxDarts) { 
            dartCounter++;
            uiText.text = "x" + dartCounter + "/" + maxDarts;
        }
    }

    public void Reward()
    {
        int reward;
        String[] rewards = { "+3 Max Health", "+1 Max Dart", "+1 Dart Damage", "Health Restore", "+1 Stealth" };
        if (stealth < 2)
        {
            reward = UnityEngine.Random.Range(0, 5);
        }
        else
        {
            reward = UnityEngine.Random.Range(0, 4);
        }
        upgradeImg.enabled = true;
        upgradeText.enabled = true;
        upgradeText.text = rewards[reward];
        Invoke("HideRewards", 10);

        if (reward == 0)
        {
            health += 3;
            maxHealth += 3;
            healthBar.GetComponent<HealthBarControl>().SetHealth(health);
        }
        else if (reward == 1)
        {
            maxDarts++;
            dartCounter++;
            uiText.text = "x" + dartCounter + "/" + maxDarts;
        }
        else if (reward == 2)
        {
            foreach (GameObject dart in GameObject.FindGameObjectsWithTag("Dart"))
            {
                dart.GetComponent<DartBehavior>().damage++;
            }
            dartDamage++;
        }
        else if (reward == 3)
        {
            health = maxHealth;
            healthBar.GetComponent<HealthBarControl>().SetHealth(health);
        }
        else if (reward == 4)
        {
            stealth += 0.5f;
        }
        dartCounter = maxDarts;
        floorsCleared++;

        
    }

    public void HideRewards()
    {
        upgradeImg.enabled = false;
        upgradeText.enabled = false;
    }
}
