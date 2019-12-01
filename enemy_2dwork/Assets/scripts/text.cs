using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class text : MonoBehaviour
{
    public GameObject Bullet;//子彈物件

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Instantiate(Bullet, transform.position, Quaternion.Euler(0, 0, 0));
            Instantiate(Bullet, transform.position, Quaternion.Euler(0, 0, 30));
            Instantiate(Bullet, transform.position, Quaternion.Euler(0, 0, 60));
            Instantiate(Bullet, transform.position, Quaternion.Euler(0, 0, 90));
            Instantiate(Bullet, transform.position, Quaternion.Euler(0, 0, 120));
            Instantiate(Bullet, transform.position, Quaternion.Euler(0, 0, 150));
            Instantiate(Bullet, transform.position, Quaternion.Euler(0, 0, 180));
            Instantiate(Bullet, transform.position, Quaternion.Euler(0, 0, 210));
            Instantiate(Bullet, transform.position, Quaternion.Euler(0, 0, 240));
            Instantiate(Bullet, transform.position, Quaternion.Euler(0, 0, 270));
            Instantiate(Bullet, transform.position, Quaternion.Euler(0, 0, 300));
            Instantiate(Bullet, transform.position, Quaternion.Euler(0, 0, 330));

        }

    }
}
