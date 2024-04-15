using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private Transform playerPos;
    private UnityEngine.AI.NavMeshAgent agent;
    public GameObject fireballPrefab;

    private int health = 3;
    private float lastFireTime = 0;
    public float fireCooldown = 1.0f;
    private int damage = 1;

    public bool aware = false;

    private SpriteRenderer spriteRenderer;

    private AudioSource audioSource;
    public AudioClip[] audioClips;

    public SpriteRenderer childRenderer;
    public LayerMask enemyLayer;
    public GameObject floorClearReward;

    private GameObject player;

    public int currentFloor = 0;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.stoppingDistance = 3;
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.Find("Player");
        playerPos = player.transform;
        audioSource = GameObject.Find("Audio Source").GetComponent<AudioSource>();

        // Random chance to increase stats based on floor
        for (int i = 0; i < currentFloor; i++)
        {
            int buff = UnityEngine.Random.Range(0, 3);
            if (buff == 0)
            {
                if (UnityEngine.Random.Range(0, 50) == 0)
                {
                    damage++;
                }
            }
            else if (buff == 1)
            {
                if (UnityEngine.Random.Range(0, 50) == 0)
                {
                    health++;
                }
            }
            else if (buff == 2)
            {
                if (UnityEngine.Random.Range(0, 50) == 0)
                {
                    if (fireCooldown > 0.5f)
                        fireCooldown -= 0.1f;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
        transform.position += new Vector3(0, 0, 0.1f);

        if (Vector2.Distance(transform.position, playerPos.position) < 3 && !aware)
        {
            NoticePlayer();
        }

        if (aware)
        {
            if (Vector2.Distance(transform.position, playerPos.position) > 3 - player.GetComponent<PlayerController>().stealth)
            {
                agent.SetDestination(playerPos.position);
            }
            if (Vector2.Distance(transform.position, playerPos.position) < 6 && (Time.time - lastFireTime) > fireCooldown)
            {
                ShootFireball();
            }     
        }
        else
        {
            Idle();
        }
        if (playerPos.position.x < transform.position.x)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }


    }

    private void Idle()
    {
        if (UnityEngine.Random.Range(0, 1000) == 1)
        {
            // Pick a random direction to move in
            Vector3 direction = new Vector3(UnityEngine.Random.Range(-1, 1), UnityEngine.Random.Range(-1, 1), 0).normalized;
            agent.SetDestination(transform.position + direction * 5);
        }
    }

    private void ShootFireball()
    {
        lastFireTime = Time.time;
        GameObject fireball = Instantiate(fireballPrefab, transform.position, Quaternion.identity) as GameObject;
        fireball.GetComponent<FireballBehavior>().SetDamage(damage);
        fireball.GetComponent<Rigidbody2D>().velocity = (playerPos.position - transform.position).normalized * 5;
        AudioClip clip = audioClips[UnityEngine.Random.Range(1, 4)];
        AudioSource src = Instantiate(audioSource);
        src.clip = clip;
        src.Play();
        Destroy(src, 5.0f);
    }

    public void TakeDamage(int damage)
    {
        if (!aware)
            NoticePlayer();
        health -= damage;
        if (health <= 0)
        {
            AudioSource src = Instantiate(audioSource);
            src.clip = audioClips[5];
            src.Play();
            Destroy(src, audioClips[5].length);
            GameObject.Find("SimpleRandomDungeon").GetComponent<RandomDungeonGenerator>().enemiesAlive--;
            if (GameObject.Find("SimpleRandomDungeon").GetComponent<RandomDungeonGenerator>().enemiesAlive == 0)
            {
                Instantiate(floorClearReward, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
        else
        {
            AudioSource src = Instantiate(audioSource);
            src.clip = audioClips[4];
            src.Play();
            Destroy(src, audioClips[4].length);
        }
    }

    private void NoticePlayer()
    {
        aware = true;

        childRenderer.enabled = true;
        AudioSource src = Instantiate(audioSource);
        src.clip = audioClips[0];
        src.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
        src.Play();
        Destroy(src, audioClips[0].length);

        Collider2D[] colliders = Physics2D.OverlapCircleAll((Vector2)transform.position, 3, enemyLayer);
        colliders[0].gameObject.GetComponent<EnemyAI>().Alarm();
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.tag == "Enemy" && !collider.gameObject.GetComponent<EnemyAI>().aware)
            {
                collider.gameObject.GetComponent<EnemyAI>().Alarm();
            }
        }
        
        
    }

    public void Alarm()
    {
        if (!aware)
        {
            NoticePlayer();
        }
    }
}
