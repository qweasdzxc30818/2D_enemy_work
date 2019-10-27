using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEnemy : MonoBehaviour
{
    #region 設定
    public bool isGround;
    public RectTransform Health_Ground;
    public const int MaxHealth_ground = 100;
    public int currentHealth_Ground = MaxHealth_ground;
    public float OffY_Ground = 10F;
    public float OffX_Ground = 10F;
    public GameObject Enemy_Ground;
    #endregion
    #region 執行
    void Update()
    {
        if (isGround == true)
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                currentHealth_Ground = currentHealth_Ground - 10;
            }
        }
        Health_Ground.sizeDelta = new Vector2(currentHealth_Ground, Health_Ground.sizeDelta.y);
        Vector2 target_ground = Camera.main.WorldToScreenPoint(Enemy_Ground.transform.position);
        Health_Ground.GetComponent<RectTransform>().position = target_ground + Vector2.up * OffY_Ground + Vector2.left * OffX_Ground;

    }
    #endregion
}
