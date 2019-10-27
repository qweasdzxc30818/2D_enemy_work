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
    void OnTriggerEnter2D ()
    {
        currentHealth_many = currentHealth_many - 10;
    }
    #region 執行
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            currentHealth_many = currentHealth_many - 10;
        }
        Health_many.sizeDelta = new Vector2(currentHealth_many, Health_many.sizeDelta.y);
        Vector2 TargeY = Camera.main.WorldToScreenPoint(Main.transform.position);
        Health_many.GetComponent<RectTransform>().position = TargeY + Vector2.up * Health_offY + Vector2.left * Health_offX;
        Self.GetComponent<Transform>().position = Main.GetComponent<Transform>().position + Vector3.up * OffUp + Vector3.left * OffLeft;
        
    }
    #endregion

}
