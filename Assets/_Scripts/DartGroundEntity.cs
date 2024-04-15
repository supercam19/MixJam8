using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartGroundEntity : MonoBehaviour
{
    private GameObject player;

    private int damage = 1;
    private int type = 0;
    private bool collected = false;

    public void Start()
    {
        player = GameObject.Find("Player");
    }

    public void Update()
    {
        if (Vector2.Distance(transform.position, player.transform.position) < 3)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, 0.01f);
        }
        if (Vector2.Distance(transform.position, player.transform.position) < 1)
        {
            player.GetComponent<PlayerController>().Pickup(damage, type);
            Destroy(gameObject);
        }
    }

    public void PassData(int damage, int type)
    {
        this.damage = damage;
        this.type = type;
    }
}
