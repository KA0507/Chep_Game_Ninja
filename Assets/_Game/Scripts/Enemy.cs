using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Enemy : Character
{
    [SerializeField] private float attackRange;
    [SerializeField] private float moveSpeed;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GameObject attackArea;


    private IState currentState;
    private bool isRight = true;

    private Character target;
    public Character Target => target;
    private void Update()
    {
        // Nếu currentState khác null và không chết thực hiện state
        if (currentState != null && !isDead)
        {
            currentState.OnExecute(this);
        }
    }
    // Gọi hàm OnInit lớp Character, chuyển sang state idle, hủy khu vực tấn công
    public override void OnInit()
    {
        base.OnInit();
        ChangeState(new IdleState());
        DeActiveAttack();
    }
    // Xóa healthBar, xóa Enemy
    public override void OnDespawn()
    {
        base.OnDespawn();
        Destroy(healthBar.gameObject);
        Destroy(gameObject);
    }
    // Xóa state, gọi hàm OnDeath lớp Character
    protected override void OnDeath()
    {
        ChangeState(null);
        base.OnDeath();
    }

    // Thay đổi state của Enemy
    public void ChangeState(IState newState)
    {
        if (currentState != null)
        {
            currentState.OnExit(this);
        }
        currentState = newState;
        if (currentState != null)
        {
            currentState.OnEnter(this);
        }
    }
    // Đặt mục tiêu và thay đổi state
    internal void SetTarget(Character character)
    {
        this.target = character;
        if (isTargetInRange())
        {
            ChangeState(new AttackState());
        }else if (Target != null)
        {
            ChangeState(new PatrolState());
        }else
        {
            ChangeState(new IdleState());
        }
    }
    // Chuyển sang anim run, di chuyển Enemy
    public void Moving()
    {
        ChangeAnim("run");
        rb.velocity = transform.right * moveSpeed;
    }

    // Dừng di chuyển, chuyển sang anim idle
    public void StopMoving()
    {
        ChangeAnim("idle");
        rb.velocity = Vector2.zero;
    }

    // Chuyển sang anim attack
    public void Attack()
    {
        ChangeAnim("attack");
        ActiveAttack();
        Invoke(nameof(DeActiveAttack), 0.5f);
    }

    // Kiểm tra mục tiêu có trong khoảng cách không
    public bool isTargetInRange()
    {
        // Nếu có mục tiêu và khoảng cách nhỏ hơn attackRange trả về true và ngược lại
        if (target != null && Vector2.Distance(target.transform.position, transform.position) <= attackRange)
        {
            return true;
        }else
        {
            return false;
        }         
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Nếu va chạm với đối tượng có tag EnemyWall đổi hướng Enemy
        if (collision.tag == "EnemyWall")
        {
            ChangeDirection(!isRight);
        }
    }
    // Đổi hướng Enemy
    public void ChangeDirection(bool isRight)
    {
        this.isRight = isRight;
        transform.rotation = isRight ? Quaternion.Euler(Vector3.zero) : Quaternion.Euler(Vector3.up * 180);
    }
    // Kich hoat khu vục tấn công
    private void ActiveAttack()
    {
        attackArea.SetActive(true);
    }

    // Hủy khu vực tấn công
    private void DeActiveAttack()
    {
        attackArea.SetActive(false);
    }
}
