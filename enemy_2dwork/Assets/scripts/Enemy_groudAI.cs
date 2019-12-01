using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Enemy_groudAI : MonoBehaviour
{
    #region 屬性
    public Transform player;
    public Transform self;//敵人本身
    public Vector3 begin;
    [Header("遊走半徑")]
    public float wanderRadius = 6;//遊走半徑
    [Header("追擊半徑")]
    public float chaseRadius = 10;//追擊半徑
    [Header("自衛半徑")]
    public float defend = 5;//自衛半徑
    [Header("最小攻擊半徑")]
    public float minAttackRadius = 2;//最小攻擊半徑
    [Header("最大攻擊半徑")]
    public float maxAttackRadius = 4;//最大攻擊半徑
    [Header("遊走速度")]
    public float walkSpeed = 1;//遊走速度
    [Header("追擊速度")]
    public float runSpeed = 2;//追擊速度
    [Header("後退速度")]
    public float backSpeed = 1.5f;//後退速度
    [Header("轉向速度")]
    public float turnSpeed = 1;//轉向速度
    public enum MonsterState
    {
        Stand,
        Walk,
        Chase,
        attack,
        Return
    }
    public MonsterState currentState = MonsterState.Stand;//默認原地
    public float[] actWeight = { 3000, 4000 };
    [Header("狀態切換間隔")]
    public float actResttime = 2;//狀態切換間隔
    public float lastAct;//上次切換時間
    private float enemyToPlayer;//敵人跟玩家之間距離
    private float enemyBegin;//敵人與初始位置的距離
    private bool is_Running = false;
    #endregion

    #region 事件
    void Start()
    {
        //保存初始位置
        begin = self.GetComponent<Transform>().position;
        //隨機狀態
        RandomAction();
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
                
            case MonsterState.Walk:
                self.up = player.position - self.position;
                self.position += self.up * Time.deltaTime * walkSpeed;
                if (Time.time - lastAct > actResttime)
                {
                    RandomAction();
                }
                WanderRadiusCheck();
                break;

            case MonsterState.Chase:
                
                enemyToPlayer = Vector2.Distance(player.transform.position, self.position);
                if (!is_Running)
                {
                    is_Running = true;
                }
                if (enemyToPlayer > maxAttackRadius)
                {
                    self.up = player.position - self.position;
                    self.position += self.up * Time.deltaTime * runSpeed;
                }
                else if (minAttackRadius < enemyToPlayer && enemyToPlayer < maxAttackRadius)
                {
                    self.up = player.position - self.position;
                }
                else if (enemyToPlayer < minAttackRadius)
                {
                    self.up = player.position - self.position;
                    self.position -= self.up * Time.deltaTime * backSpeed;
                }

                ChaseRadiusCheck();

                break;
            case MonsterState.Return:
                self.up = begin - self.position;
                self.position += self.up * Time.deltaTime * runSpeed;
                ReturnCheck();
                break;

        }
        //self.rotation = new Quaternion(0 ,0 ,0, 0);
    }
        #endregion
    #region 方法
     /// <summary>
     /// 隨機狀態
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
     /// 追擊狀態
     /// </summary>
     void EnemyDistanceCheck()
     {
         enemyToPlayer = Vector2.Distance(player.transform.position, self.position);
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
         enemyToPlayer = Vector2.Distance(player.transform.position, self.position);
         enemyBegin = Vector2.Distance(self.position, begin);
         if (enemyToPlayer < defend)
         {
             currentState = MonsterState.Chase;
         }
     }

     /// <summary>
     /// 追擊範圍
     /// </summary>
     void ChaseRadiusCheck()
     {
         //enemyToPlayer = Vector2.Distance(player.transform.position, transform.position);
         enemyBegin = Vector2.Distance(self.position, begin);
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
         enemyBegin = Vector2.Distance(self.position, begin);
         if (enemyBegin < 0.5f)
         {
                is_Running = false;
                RandomAction();
            }
        }
    
        #endregion
 }
