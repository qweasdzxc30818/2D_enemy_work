using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;


public class WolfBossAI : MonoBehaviour
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
    [Header("自衛半徑")]
    public float defend = 5;//自衛半徑    
    [Header("衝刺半徑")]
    public float chargeRadius = 3;//衝刺半徑
    [Header("衝刺速度")]
    public float chargespeed = 1.63f;//衝刺速度    
    [Header("遊走速度")]
    public float walkSpeed = 1;//遊走速度
    [Header("追擊速度")]
    public float runSpeed = 2;//追擊速度
    [Header("轉向速度")]
    public float turnSpeed = 3;//轉向速度
    [Header("衝刺終點")]
    public Vector2 chargepoint;
    [Header("是否正在衝刺")]
    public bool ischarge = false;
    [Header("能否衝刺")]
    public bool CanCharge = true;
    [Header("計時器")]
    public float timer;
    [Header("呼叫計時器")]
    public float timer2;
    [Header("呼叫間隔"), Range(0f, 120f)]
    public float CallTime = 60f;
    [Header("原地呼叫時間") , Range(0f , 10f)]
    public float CallWait = 5f;
    [Header("是否正在呼叫")]
    public bool isCall = false;
    /// <summary>
    /// 敵人狀態
    /// </summary>
    public enum MonsterState
    {
        Stand,
        Charge,
        Walk,
        Chase,
        Call,
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
    //public Quaternion targetRotation = new Quaternion(0, 0, 0, 0);//目標朝向
    private float rotate;
    private bool is_Running = false;
    [Header("敵人本身")]
    public Transform self;
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
        CallCheck();
        timer2 += Time.deltaTime;
        switch (currentState)
        {
            
            //待機狀態
            case MonsterState.Stand:
                if (Time.time - lastAct > actResttime)
                {
                    CanCharge = true;
                    RandomAction(); //時間到隨機切換      
                }
                //檢測用方法
                EnemyDistanceCheck();
                break;
            //走路狀態
            case MonsterState.Walk:
                transform.up = -(player.position - transform.position);//朝向
                transform.position += -transform.up * Time.deltaTime * walkSpeed;
                //時間到切換狀態
                if (Time.time - lastAct > actResttime)
                {
                    RandomAction();
                }
                //檢查是否追擊
                EnemyDistanceCheck();
                break;
            //追擊狀態
            case MonsterState.Chase:
                timer = 0;
                if (is_Running == false && isCall == false)
                {
                    is_Running = true;
                }
                //若不是在衝刺中 追擊玩家
                if (ischarge == false && isCall == false)
                {
                    transform.up = -(player.position - transform.position);
                    transform.position += -transform.up * Time.deltaTime * runSpeed;
                    //檢查是否再追擊範圍
                    ChaseRadiusCheck();
                    //檢查是否使用衝刺
                    chargeRadiusCheck();
                }
                break;
            //衝刺狀態
            case MonsterState.Charge:
                transform.up = -(player.position - transform.position);
                timer += Time.deltaTime;//開始計時
                if (timer >= 2.5 && timer <= 2.8)
                {
                    chargepoint = transform.position + (player.position - transform.position) * 1.5f;
                }
                // 2.5~3秒間為玩家反應時間
                if (timer >= 3 && timer <= 3.5)
                {
                    if (CanCharge == true)//若可以衝刺
                    {

                        CanCharge = false;
                        Debug.Log(chargepoint);

                    }
                    //衝刺
                    transform.position = Vector3.MoveTowards(transform.position, chargepoint, chargespeed);

                }
                //冷卻時間
                if (timer >= 3.8)
                {
                    //轉為追擊狀態
                    currentState = MonsterState.Chase;
                    //重設能否衝刺
                    CanCharge = true;
                }
                break;
            case MonsterState.Call:
                if(timer2 >= CallTime + CallWait)
                {
                    timer2 = 0;
                    isCall = false;
                    currentState = MonsterState.Chase;
                }
                break;

            //返回狀態
            case MonsterState.Return:
                transform.up = -(begin - transform.position);
                transform.position += -transform.up * Time.deltaTime * runSpeed;
                //檢查是否回到原位
                ReturnCheck();
                break;

        }
        //衝次完冷卻
        if (CanCharge == false && isCall == false)
        {
            transform.rotation = new Quaternion(0, 0, 0, 0);
        }
     
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
        if (enemyToPlayer < defend && ischarge == false)
        {
            currentState = MonsterState.Chase;
            CanCharge = true;
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
    /// 檢查是否使用衝刺
    /// </summary>
    void chargeRadiusCheck()
    {
        enemyToPlayer = Vector2.Distance(player.position, transform.position);
        if (enemyBegin < chargeRadius)
        {

            currentState = MonsterState.Charge;
            timer = 0;
        }
    }
    void CallCheck()
    {
        if(timer2 >= CallTime)
        {
            isCall = true;
            currentState = MonsterState.Call;
        }
    }

   
}
