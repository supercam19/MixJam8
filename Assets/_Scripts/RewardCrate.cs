using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardCrate : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip rewardSound;
    public AudioClip breakSound;
    public GameObject ladder;
    public void Start()
    {
        audioSource = GameObject.Find("Audio Source").GetComponent<AudioSource>();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Dart")
        {
            
            GetComponent<SpriteRenderer>().enabled = false;
            audioSource.clip = breakSound;
            audioSource.Play();
            Invoke("Reward", breakSound.length);
            ladder.SetActive(true);
        }
    }

    public void Reward()
    {
        GameObject.Find("Player").GetComponent<PlayerController>().Reward();
        audioSource.clip = rewardSound;
        audioSource.Play();
        Destroy(gameObject, rewardSound.length);
    }
}
