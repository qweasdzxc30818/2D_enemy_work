using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthBar : MonoBehaviour
{
    #region 血量設定
    public const int maxHealth = 100;
    public int currentHealth = maxHealth;//血量設定
    public RectTransform HealthBar;
    public float offsetY = 30f;
    public float offsetX = 0f;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))

        {

            //接受傷害

            currentHealth = currentHealth - 10;

        }
        HealthBar.sizeDelta = new Vector2(currentHealth, HealthBar.sizeDelta.y);
        Vector2 TargeP = Camera.main.WorldToScreenPoint(Target.transform.position);
        Health.GetComponent<RectTransform>().position = TargeP + Vector2.up * offsetY + Vector2.left * offsetX;
        HealthUnder.GetComponent<RectTransform>().position = TargeP + Vector2.up * offsetY + Vector2.left * offsetX;
    }
    #endregion
    public GameObject Target;
    public GameObject Health;
    public GameObject HealthUnder;
}
