using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMove : MonoBehaviour
{
    public float BulletSpeed = 1;
    public Transform Enemy;
    void Start()
    {
    }

    void Update()
    {
        transform.Translate(Vector2.up * BulletSpeed * Time.deltaTime);
        if (transform.position.y > 10 || transform.position.y < -10)
        {

            Destroy(gameObject);
        }
        if (transform.position.x > 10 || transform.position.x < -10)
        {

            Destroy(gameObject);
        }

    }
}
