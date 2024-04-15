using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartBehavior : MonoBehaviour
{

    public float speed = 10.0f;
    public int type = 0;
    public int damage = 1;

    public GameObject dartEntity;

    private Vector2 target;

    Rigidbody2D body;
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        body.velocity = transform.right * speed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<EnemyAI>().TakeDamage(damage);
            Instantiate(dartEntity, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        else if (collision.gameObject.tag != "Player")
        {
            Instantiate(dartEntity, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    public void SetTarget(Vector2 target)
    {
        Vector2 direction = (target - (Vector2)transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        this.target = target;
    }

    public void SetDamage(int damage)
    {
        this.damage = damage;
    }
}
