using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy_ArmorAI : MonoBehaviour
{
    #region 欄位
    [Header("玩家")]
    public Transform player;//玩家
    [Header("敵人初始位置")]
    public Vector3 begin;//敵人初始位置    
    [Header("遊走半徑")]
    public float wanderRadius = 6;//遊走半徑
    [Header("追擊半徑")]
    public float chaseRadius = 10;//追擊半徑
    [Header("攻擊半徑")]
    public float AttackRadius = 2;
    [Header("自衛半徑")]
    public float defend = 5;//自衛半徑    
    [Header("Buff時間")]
    public float Bufftime = 5;
    [Header("Buff速度")]
    public float BuffSpeed = 5;
    [Header("遊走速度")]
    public float walkSpeed = 1;//遊走速度
    [Header("追擊速度")]
    public float runSpeed = 2;//追擊速度
    [Header("狀態2追擊速度")]
    public float runSpeed2 = 3;
    [Header("轉向速度")]
    public float turnSpeed = 1;//轉向速度
    [Header("是否有盔甲")]
    public bool isArmor = true;
    [Header("計時器")]
    public float timer = 0;
    [Header("發射間隔")]
    public float BulletTime = 0.5f;
    [Header("第一次發射延遲")]
    public float BulletFirst = 0.2f;
    private float angle = 0;
    /// <summary>
    /// 敵人狀態
    /// </summary>
    public enum MonsterState
    {
        Stand,
        Walk,
        Chase,
        Attack,
        Return
    }
    [Header("當前狀態")]
    public MonsterState currentState = MonsterState.Stand;//默認原地
    public float[] actWeight = { 3000, 4000 };
    [Header("下個動作的切換間隔")]
    public float actResttime = 2;//狀態切換間隔
    [Header("上次切換時間")]
    public float lastAct;//上次切換時間

    private float enemyToPlayer;//敵人跟玩家之間距離
    private float enemyBegin;//敵人與初始位置的距離 
    private float rotate;
    private bool is_Running = false;
    public bool isAttack = false;
    [Header("敵人本身")]
    public Transform self;
    [Header("接收盔甲")]
    public GameObject Armor;
    public int currentArmor;
    public GameObject Bullet;
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
                
                
                if (is_Running == false)
                {
                    is_Running = true;
                }
                if(isAttack == false)
                {
                    if (currentArmor <= 100 && currentArmor > 0)
                    {
                        timer = 0;
                        transform.up = player.position - transform.position;
                        transform.position += transform.up * Time.deltaTime * runSpeed;
                        ChaseRadiusCheck();
                         AttackCheck();
                    }
                    else if (currentArmor <= 0)
                    {
                        timer += Time.deltaTime;
                        if(timer>=0 && timer <= Bufftime)
                        {
                            transform.up = player.position - transform.position;
                            transform.position += transform.up * Time.deltaTime * BuffSpeed;
                        }
                        else if(timer > Bufftime)
                        {
                            transform.up = player.position - transform.position;
                            transform.position += transform.up * Time.deltaTime * runSpeed2;
                        }
                    }
                }
                
                break;

            case MonsterState.Attack:
                timer += Time.deltaTime;
                
                if (timer >= 1)
                {
                    CancelInvoke();
                    isAttack = false;
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
        currentArmor = Armor.GetComponent<ArmorBar>().currentArmor_2;

    }
    #endregion

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
    /// 檢查是否攻擊
    /// </summary>
    void AttackCheck()
    {
        enemyToPlayer = Vector2.Distance(player.transform.position, transform.position);
        if (enemyToPlayer < AttackRadius)
        {
            isAttack = true;
            currentState = MonsterState.Attack;
            InvokeRepeating("BulletON", BulletFirst, BulletTime);
        }
    }
    void BulletON()
    {
        for(angle = 0; angle <= 330; angle+=30)
        {
            Instantiate(Bullet, transform.position, Quaternion.Euler(0, 0, angle));
        }
       
    }
    
}
