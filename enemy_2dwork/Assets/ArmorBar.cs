using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorBar : MonoBehaviour
{
    #region 盔甲設定
    public const int maxArmor = 100;
    [Header("最大血量")]
    public int currentArmor, currentArmor_2 = maxArmor;
    public RectTransform Armor, Armor_2;
    [Header("跟隨目標")]
    public GameObject Target_2;
    [Header("血條")]
    public GameObject Armorject;
    [Header("血條底圖")]
    public GameObject ArmorUnder;
    [Header("盔甲條")]
    public GameObject Armorject_2;
    [Header("Y軸偏移值")]
    public float offsety = 30f;
    [Header("X軸偏移值")]
    public float offsetx = 0f;
    [Header("盔甲一次攻擊減少量")]
    public int a = 0;
    #endregion
    #region 執行
    void Update()
    {


        if (currentArmor_2 > 0)
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                currentArmor_2 = currentArmor_2 - a;//盔甲受一次攻擊扣血量
            }
        }
        else if (Input.GetKeyDown(KeyCode.H))
        {
            currentArmor = currentArmor - 10;//血量受一次攻擊扣血量
        }

        Armor.sizeDelta = new Vector2(currentArmor, Armor.sizeDelta.y);
        Armor_2.sizeDelta = new Vector2(currentArmor_2, Armor_2.sizeDelta.y);
        Vector2 TargeX = Camera.main.WorldToScreenPoint(Target_2.transform.position);
        Armorject.GetComponent<RectTransform>().position = TargeX + Vector2.up * offsety + Vector2.left * offsetx;
        ArmorUnder.GetComponent<RectTransform>().position = TargeX + Vector2.up * offsety + Vector2.left * offsetx;
        Armorject_2.GetComponent<RectTransform>().position = TargeX + Vector2.up * offsety + Vector2.left * offsetx;//跟隨
    }
    #endregion
   
}