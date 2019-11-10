using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy_AI : MonoBehaviour
{
    public GameObject playerUnit;
    public Vector3 begin;
    public float wanderRadius;
    public float chaseRadius;
    public float defend;
    public float walkSpeed;
    public float runSpeed;
    public float turnSpeed;
    public enum MonsterState
    {
        Stand,
        Check,
        Walk,
        Chase,
        Return
    }
    public MonsterState currentState = MonsterState.Stand;
    public float[] actWeight = { 3000, 3000, 4000 };
    public float actResttime;
    public float lastAct;

    private float enemyToPlayer;
    private float enemyBegin;
    private Quaternion targetRotation;
    private bool is_Running = false;




    void Start()
    {
        playerUnit = GameObject.FindGameObjectWithTag("player");
        begin = gameObject.GetComponent<Transform>().position;
        RandomAction();
    }
    void RandomAction()
    {
        lastAct = Time.time;
        float number = Random.Range(0, actWeight[0] + actWeight[1] + actWeight[2]);
        if (number <= actWeight[0])
        {
            currentState = MonsterState.Stand;
        }
        else if (actWeight[0] < number && number <= actWeight[0] + actWeight[1])
        {
            currentState = MonsterState.Check;
        }
        if (actWeight[0] + actWeight[1] < number && number <= actWeight[0] + actWeight[1] + actWeight[2])
        {
            currentState = MonsterState.Walk;
            //targetRotation = Quaternion.Euler(0, 0, Random.Range(1, 5) * 90);

        }
    }

    
    void Update()
    {
        switch (currentState)
        {
            case MonsterState.Stand:
                if (Time.time - lastAct >actResttime)
                {
                    RandomAction();         
                }
                EnemyDistanceCheck();
                break;
            case MonsterState.Walk:
                transform.Translate(Vector2.one * Time.deltaTime * walkSpeed);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed);
                


                if (Time.time - lastAct > actResttime)
                {
                    RandomAction();         
                }
                //該狀態下的檢測指令
                WanderRadiusCheck();
                break;
            case MonsterState.Chase:
                if (!is_Running)
                {
                    is_Running = true;
                }
                transform.Translate(Vector3.forward * Time.deltaTime * runSpeed);
                targetRotation = Quaternion.LookRotation(playerUnit.transform.position - transform.position, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed);
                ChaseRadiusCheck();
                break;
            case MonsterState.Return:
                targetRotation = Quaternion.LookRotation(begin - transform.position, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed);
                transform.Translate(Vector2.up * Time.deltaTime * runSpeed);
                ReturnCheck();
                break;
        }
    }
    void EnemyDistanceCheck()
    {
        enemyToPlayer = Vector2.Distance(playerUnit.transform.position, transform.position);
        if (enemyToPlayer < defend)
        {
            currentState = MonsterState.Chase;
        }

    }
    void WanderRadiusCheck()
    {
        enemyToPlayer = Vector2.Distance(playerUnit.transform.position, transform.position);
        enemyBegin = Vector2.Distance(transform.position, begin);
        if(enemyToPlayer < defend)
        {
            currentState = MonsterState.Chase;
        }
        else if (enemyToPlayer > wanderRadius)
        {
            targetRotation = Quaternion.LookRotation(begin - transform.position, Vector2.up);
        }

    }
    void ChaseRadiusCheck()
    {
        enemyToPlayer = Vector2.Distance(playerUnit.transform.position, transform.position);
        enemyBegin = Vector2.Distance(transform.position, begin);
        if (enemyBegin> chaseRadius)
        {
            currentState = MonsterState.Return;
        }
    }

    void ReturnCheck()
    {
        enemyBegin = Vector2.Distance(transform.position, begin);
        if (enemyBegin < 0.5f)
        {
            is_Running = false;
            RandomAction();
        }
    }

}
