﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldHealth : MonoBehaviour
{
    #region 血量設定
    
    public const int maxHealth = 100;
    [Header("最大血量")]
    public int currentHealth = maxHealth;//血量設定
    public RectTransform HealthBar;
    [Header("Y軸偏移值")]
    public float offsetY = 30f;
    [Header("X軸偏移值")]
    public float offsetX = 0f;
    [Header("跟隨目標")]
    public GameObject Target;//跟隨目標
    [Header("血量")]
    public GameObject Health;//血量


    private int max;//固定長度
    #endregion

    #region 執行
    private void Start()
    {
        max = currentHealth;
    }

    void Update()
    {
        HealthBar.sizeDelta = new Vector2(100 * currentHealth / max, HealthBar.sizeDelta.y);
        Vector2 TargeP = Camera.main.WorldToScreenPoint(Target.transform.position);
        Health.GetComponent<RectTransform>().position = TargeP + Vector2.up * offsetY + Vector2.left * offsetX;

    }
    #endregion
   
}