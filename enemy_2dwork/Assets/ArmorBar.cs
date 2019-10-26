using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorBar : MonoBehaviour
{
    #region 盔甲設定
    public const int maxArmor = 100;
    public int currentArmor, currentArmor_2 = maxArmor;//血量設定
    public RectTransform Armor, Armor_2;
    public float offsety = 30f;
    public float offsetx = 0f;
    void Update()
    {


        if (currentArmor_2 > 0)
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                currentArmor_2 = currentArmor_2 - 10;
            }
        }
        else if (Input.GetKeyDown(KeyCode.H))
        {
            currentArmor = currentArmor - 10;
        }

        Armor.sizeDelta = new Vector2(currentArmor, Armor.sizeDelta.y);
        Armor_2.sizeDelta = new Vector2(currentArmor_2, Armor_2.sizeDelta.y);
        Vector2 TargeX = Camera.main.WorldToScreenPoint(Target_2.transform.position);
        Armorject.GetComponent<RectTransform>().position = TargeX + Vector2.up * offsety + Vector2.left * offsetx;
        ArmorUnder.GetComponent<RectTransform>().position = TargeX + Vector2.up * offsety + Vector2.left * offsetx;
        Armorject_2.GetComponent<RectTransform>().position = TargeX + Vector2.up * offsety + Vector2.left * offsetx;
    }
    #endregion
    public GameObject Target_2;
    public GameObject Armorject;
    public GameObject ArmorUnder;
    public GameObject Armorject_2;
}