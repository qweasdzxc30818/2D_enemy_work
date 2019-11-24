using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEnemy : MonoBehaviour
{
    #region 設定
    public bool isGround;
    public Transform player;
    public Transform self;
    public RectTransform Health_Ground;
    public const int MaxHealth_ground = 100;
    public int currentHealth_Ground = MaxHealth_ground;
    public float OffY_Ground = 10F;
    public float OffX_Ground = 10F;
    public GameObject Enemy_Ground;
    #endregion

    #region 事件
    /// <summary>
    /// 碰撞時執行
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isGround == true)
        {
            currentHealth_Ground = currentHealth_Ground - 10;
        }
    }

    private void Start()
    {
        InvokeRepeating("time", 3, 3);//計時呼叫
    }

    /// <summary>
    /// 每n秒切換遁地布林值
    /// </summary>
    void time()
    {
        if (isGround == true)
        {
            isGround = false;
        }
        else
        {
            isGround = true;
        }
    }

    void Update()
    {

        if (isGround == true)
        {
            Enemy_Ground.SetActive(true);
            if (Input.GetKeyDown(KeyCode.H))
            {

                /// currentHealth_Ground = currentHealth_Ground - 10;
            }
        }

        

        Health_Ground.sizeDelta = new Vector2(currentHealth_Ground, Health_Ground.sizeDelta.y);//血條跟隨
        Vector2 target_ground = Camera.main.WorldToScreenPoint(Enemy_Ground.transform.position);
        Health_Ground.GetComponent<RectTransform>().position = target_ground + Vector2.up * OffY_Ground + Vector2.left * OffX_Ground;
       /* if (isGround == false) {
            self.rotation = new Quaternion(0, 0, 0, 0);
        }*/

        #endregion
    }
}
