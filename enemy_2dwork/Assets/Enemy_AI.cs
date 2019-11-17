using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy_AI : MonoBehaviour
{
    #region 欄位
    public GameObject playerUnit;
    public Transform player;//玩家
    public Vector3 begin;//敵人初始位置
    public float wanderRadius;//遊走半徑
    public float chaseRadius;//追擊半徑
    public float defend;//自衛半徑
    public float chargeRadius;//衝刺半徑
    public float chargespeed;//衝刺速度
    public float walkSpeed;//遊走速度
    public float runSpeed;//追擊速度
    public float turnSpeed;//轉向速度
    public Vector2 chargepoint;
    public bool a = true;
    public bool ischarge = false;
    public bool b = true;
    public float timer;
    public enum MonsterState
    {
        Stand,
        Walk,
        Chase,
        charge,
        Return
    }
    public MonsterState currentState = MonsterState.Stand;//默認原地
    public float[] actWeight = { 3000, 4000 };
    public float actResttime ;//狀態切換間隔
    public float lastAct;//上次切換時間

    private float enemyToPlayer;//敵人跟玩家之間距離
    private float enemyBegin;//敵人與初始位置的距離
    public Quaternion targetRotation = new Quaternion(0,0,0,0);//目標朝向
    private float rotate;
    private bool is_Running = false;
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
        switch (currentState)
        {
            //待機狀態
            case MonsterState.Stand:
                if (Time.time - lastAct >actResttime)
                {
                    b = true;
                    RandomAction(); //時間到隨機切換      
                }
                //檢測用方法
                EnemyDistanceCheck();
                break;
            case MonsterState.Walk:
                //Debug.Log(targetRotation);
                //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed);
                transform.up = player.position - transform.position;
                transform.position += transform.up * Time.deltaTime * walkSpeed;
                //transform.Rotate(0, 0, rotate);



                if (Time.time - lastAct > actResttime)
                {
                    RandomAction();         
                }
                WanderRadiusCheck();
                break;
                
            case MonsterState.Chase:
                if (!is_Running)
                {
                    is_Running = true;
                }
                if (ischarge == false)
                {
                    a = true;
                    transform.up = player.position - transform.position;
                    transform.position += transform.up * Time.deltaTime * runSpeed;
                    ChaseRadiusCheck();
                    chargeRadiusCheck();
                }
                //targetRotation = Quaternion.LookRotation(Vector2.zero , playerUnit.transform.position - transform.position);
                //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed);
                break;
            case MonsterState.charge:
                
                transform.up = player.position - transform.position;
                timer += Time.deltaTime;
                if(timer >= 3 && timer<=5)
                {
                    if (b == true)
                    {
                        chargepoint = transform.position+(player.position - transform.position) * 2;
                        b = false;
                        Debug.Log(chargepoint);
                        
                    }
                    transform.position = Vector3.MoveTowards(transform.position, chargepoint, chargespeed);
                    
                }
                
                ChaseRadiusCheck();
                /*if (a == true)
                {
                    StartCoroutine(Charge());
                    currentState = MonsterState.Stand;
                }
                */
                break;
                
            case MonsterState.Return:
                transform.up = begin - transform.position;
                transform.position += transform.up * Time.deltaTime * runSpeed;
                //targetRotation = Quaternion.LookRotation(begin - transform.position, Vector3.up);
                //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed);
                //transform.position += transform.up * Time.deltaTime * runSpeed;
                ReturnCheck();
                break;
                
        }
    }
    #endregion
    
    /// <summary>
    /// 隨機交換狀態
    /// </summary>
    void RandomAction()
    {
        lastAct = Time.time;
        float number = Random.Range(0, actWeight[0] + actWeight[1] );
        if (number <= actWeight[0])
        {
            currentState = MonsterState.Stand;
        }
     

        
        if (actWeight[0]  < number && number <= actWeight[0] + actWeight[1] )
        {
            //rotate = Random.Range(-360,360);
            //targetRotation = Quaternion.Euler(0, 0, Random.Range(1, 5)*90 );
            currentState = MonsterState.Walk;
            

        }
    }

    /// <summary>
    /// 追擊狀態
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
        if(enemyToPlayer < defend)
        {
            currentState = MonsterState.Chase;
        }
      /*  else if (enemyToPlayer > wanderRadius)
        {
            targetRotation = Quaternion.LookRotation(begin - transform.position, Vector2.up);
        }
       */
    }

    /// <summary>
    /// 追擊範圍
    /// </summary>
    void ChaseRadiusCheck()
    {
        //enemyToPlayer = Vector2.Distance(player.transform.position, transform.position);
        enemyBegin = Vector2.Distance(transform.position, begin);
        if (enemyBegin> chaseRadius)
        {
            currentState = MonsterState.Return;
            ischarge = true;
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

    void chargeRadiusCheck()
    {
        enemyToPlayer = Vector2.Distance(player.position, transform.position);
        if (enemyBegin < chargeRadius   )
        {
            
            currentState = MonsterState.charge;
            timer = 0;
        }
    }

    private IEnumerator Charge()
    {
        ischarge = false;
        
        yield return new WaitForSeconds(3);


        a = false;
        currentState = MonsterState.Stand;
    }
}

