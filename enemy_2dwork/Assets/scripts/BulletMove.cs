using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMove : MonoBehaviour
{
    public float BulletSpeed = 1;
    public Transform Enemy;
    public float BulletToEnemy;
    void Start()
    {
    }

    void Update()
    {
        /*BulletToEnemy = Vector2.Distance(Enemy.position, transform.position);
        transform.Translate(Vector2.up * BulletSpeed * Time.deltaTime);
        if (BulletToEnemy >= 5)
        {
            Destroy(gameObject);
        }*/
       
    }
}
