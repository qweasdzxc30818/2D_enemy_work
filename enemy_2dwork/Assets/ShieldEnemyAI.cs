using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ShieldEnemyAI : MonoBehaviour
{
    #region 屬性
    public Transform player;
    public Vector3 begin;
    [Header("遊走半徑")]
    public float wanderRadius;//遊走半徑
    [Header("追擊半徑")]
    public float chaseRadius;//追擊半徑
    [Header("自衛半徑")]
    public float defend;//自衛半徑   
    public float chargeRadius;//盾衝半徑
    public float chargeSpeed;//盾衝速度
    [Header("遊走速度")]
    public float walkSpeed;//遊走速度
    [Header("追擊速度")]
    public float runSpeed;//追擊速度
    [Header("返回速度")]
    public float backSpeed;//轉向速度
    public Vector2 Chargepoint;
    public Vector2 ChargeBegin;
    public enum MonsterState//敵人總狀態
    {
        Stand,
        Walk,
        Chase,
        charge,
        Return
    }
    public float timer;
    [Header("當前狀態")]
    public MonsterState currentState = MonsterState.Stand;//默認原地
    public float[] actWeight = { 3000, 4000 };
    [Header("下個動作的切換間隔")]
    public float actResttime;//狀態切換間隔
    [Header("上次切換時間")]
    public float lastAct;//上次切換時間
    private float enemyToPlayer;//敵人跟玩家之間距離
    private float enemyBegin;//敵人與初始位置的距離
    private float rotate;
    private bool is_Running = false;
    [Header("敵人本身")]
    public Transform self;
    public bool isCharge = false;
    public bool canCharge = true;
    #endregion
    #region 事件
    void Start()
    {
        //保存初始位置
        begin = gameObject.GetComponent<Transform>().position;
        //隨機狀態
        RandomAction();

        self = GetComponent<Transform>();
    }

    void Update()
    {
        switch (currentState)
        {
            //待機狀態
            case MonsterState.Stand:
                if (Time.time - lastAct > actResttime)
                {
                   canCharge = true;
                    RandomAction(); //時間到隨機切換      
                }
                //檢測用方法
                EnemyDistanceCheck();
                break;
            //走路狀態
            case MonsterState.Walk:
                transform.up = player.position - transform.position;//朝向
                transform.position += transform.up * Time.deltaTime * walkSpeed;
                //時間到切換狀態
                if (Time.time - lastAct > actResttime)
                {
                    RandomAction();
                }
                //檢查是否追擊
                WanderRadiusCheck();
                break;
            //追擊狀態
            case MonsterState.Chase:
                timer = 0;
                if (is_Running == false)
                {
                    is_Running = true;
                }
                if (isCharge == false)
                {
                    transform.up = player.position - transform.position;
                    transform.position += transform.up * Time.deltaTime * runSpeed;
                    canCharge = true;
                    //檢查是否再追擊範圍
                    ChaseRadiusCheck();
                    //檢查是否使用衝刺
                    chargeCheck();
                }
                break;
            case MonsterState.charge:
                timer += Time.deltaTime;
                if(timer>=0 && timer <= 1.5)
                {
            
                    transform.up = player.position - transform.position;
                    ChargeBegin = transform.position;
                    Chargepoint = transform.position + (player.position - transform.position) * 0.7f; 
                }
                if (timer >= 2 && timer <= 2.5)
                {
                    if (canCharge == true)
                    {
                        canCharge = false;

                    }
                    transform.position = Vector3.MoveTowards(transform.position, Chargepoint, chargeSpeed);
                }
                else if (timer >= 3 && timer <= 3.5)
                {
                    transform.position = Vector3.MoveTowards(transform.position, ChargeBegin, chargeSpeed);
                }
                else if (timer > 3.7)
                {
                    currentState = MonsterState.Chase;
                }
               
                
                break;

            //返回狀態
            case MonsterState.Return:
                transform.up = begin - transform.position;
                transform.position += transform.up * Time.deltaTime * runSpeed;
                //檢查是否回到原位
                ReturnCheck();
                break;

        }

        #endregion

    }
    /// <summary>
    /// 隨機交換狀態
    /// </summary>
    void RandomAction()
    {
        lastAct = Time.time;
        float number = Random.Range(0, actWeight[0] + actWeight[1]);
        if (number <= actWeight[0])
        {
            currentState = MonsterState.Stand;
        }

        if (actWeight[0] < number && number <= actWeight[0] + actWeight[1])
        {
            currentState = MonsterState.Walk;

        }
    }
    /// <summary>
    /// 檢查是否展開追擊
    /// </summary>
    void EnemyDistanceCheck()
    {
        enemyToPlayer = Vector2.Distance(player.transform.position, transform.position);
        if (enemyToPlayer < defend)
        {
            currentState = MonsterState.Chase;
        }

    }
    /// <summary>
    /// 檢測玩家距離與遊走範圍
    /// </summary>
    void WanderRadiusCheck()
    {
        enemyToPlayer = Vector2.Distance(player.transform.position, transform.position);
        enemyBegin = Vector2.Distance(transform.position, begin);
        if (enemyToPlayer < defend)
        {
            currentState = MonsterState.Chase;
        }
    }
    /// <summary>
    /// 檢查是否在追擊範圍內
    /// </summary>
    void ChaseRadiusCheck()
    {
        enemyBegin = Vector2.Distance(transform.position, begin);
        if (enemyBegin > chaseRadius)
        {
            currentState = MonsterState.Return;
        }
    }
    /// <summary>
    /// 回到原位後隨機狀態
    /// </summary>
    void ReturnCheck()
    {
        enemyBegin = Vector2.Distance(transform.position, begin);
        if (enemyBegin < 0.5f)
        {
            is_Running = false;
            RandomAction();
        }
    }
    /// <summary>
    /// 是否使用盾衝
    /// </summary>
    void chargeCheck()
    {
        enemyToPlayer = Vector2.Distance(player.position, transform.position);
        if (enemyToPlayer <= chargeRadius)
        {
            currentState = MonsterState.charge;

        }
    }


}
