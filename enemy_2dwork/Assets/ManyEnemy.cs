using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManyEnemy : MonoBehaviour
{
    #region 設定
    public GameObject Self;
    public GameObject Main;
    [Range(-1f,1f)]
    public float OffUp = 10f;
    [Range(-1f,1f)]
    public float OffLeft = 10f;
    public const int maxHealth_many = 100;
    public int currentHealth_many = maxHealth_many;
    public RectTransform Health_many;
    public float Health_offY = 10f;
    public float Health_offX = 10f;
    #endregion

    /// <summary>
    /// 碰撞時執行
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        currentHealth_many = currentHealth_many - 10;
    }

    #region 事件
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
           // currentHealth_many = currentHealth_many - 10;
        }
        Health_many.sizeDelta = new Vector2(currentHealth_many, Health_many.sizeDelta.y);//血條與分身跟隨
        Vector2 TargeY = Camera.main.WorldToScreenPoint(Main.transform.position);//存主要位置
        Health_many.GetComponent<RectTransform>().position = TargeY + Vector2.up * Health_offY + Vector2.left * Health_offX;//血條跟隨
        Self.GetComponent<Transform>().position = Main.GetComponent<Transform>().position + Vector3.up * OffUp + Vector3.left * OffLeft;//分身跟隨
        
        if (currentHealth_many == 0)//死亡
        {
            Main.SetActive(false);
            Self.SetActive(false);
        }
    }
    #endregion

}
