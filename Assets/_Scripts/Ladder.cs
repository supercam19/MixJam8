using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    private GameObject dungeonGen;
    private Transform player;
    private AudioSource audioSource;
    public AudioClip ladderSound;

    public void Start()
    {
        dungeonGen = GameObject.Find("SimpleRandomDungeon");
        player = GameObject.Find("Player").transform;
        audioSource = GameObject.Find("Audio Source").GetComponent<AudioSource>();
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            AudioSource src = Instantiate(audioSource);
            src.clip = ladderSound;
            src.Play();
            Destroy(src, ladderSound.length);
            player.position = new Vector3(0, 0, 0);
            dungeonGen.GetComponent<RandomDungeonGenerator>().RunProceduralGeneration();
            Destroy(gameObject);
        }
    }
}
