using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IState
{
    float timer;
    float randomTime;
    // Bắt đầu patrol state
    public void OnEnter(Enemy enemy)
    {
        timer = 0;
        randomTime = Random.Range(3f, 6f);
    }
    // Thực hiện patrol state
    public void OnExecute(Enemy enemy)
    {
        timer += Time.deltaTime;
        if (enemy.Target != null)
        {
            // Nếu có mục tiêu, Enemy hướng về phía mục tiêu
            enemy.ChangeDirection(enemy.Target.transform.position.x > enemy.transform.position.x);
            if (enemy.isTargetInRange())
            {
                // Nếu mục tiêu trong phạm vi Enemy chuyển sang attack state
                enemy.ChangeState(new AttackState()); 
            }else
            {
                // Nếu không trong phạm vi Enemy di chuyển tới mục tiêu
                enemy.Moving();
            }           
        }else
        {
            if (timer < randomTime)
            {
                // Enemy di chuyển
                enemy.Moving();
            }
            else
            {
                // Enemy chuyển idle state
                enemy.ChangeState(new IdleState());
            }
        }
    }

    public void OnExit(Enemy enemy)
    {
        
    }

}
