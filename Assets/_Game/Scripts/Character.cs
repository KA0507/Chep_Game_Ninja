using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] protected HealthBar healthBar;
    [SerializeField] protected CombatText combatTextPrefab;

    private float hp;
    private string currentAnimName;

    public bool isDead => hp <= 0; // Nếu hp <= 0 isDead = true, hp > = isDead = false

    private void Start()
    {
        OnInit();
    }

    // Đặt giá trị ban đầu
    public virtual void OnInit()
    {
        hp = 100;
        healthBar.OnInit(100, transform);
    }
    public virtual void OnDespawn()
    {

    }

    // Chuyển sang anim die
    protected virtual void OnDeath()
    {
        ChangeAnim("die");
        Invoke(nameof(OnDespawn), 2f);
    }

    // Thay đổi anim
    protected void ChangeAnim(string animName)
    {
        if (currentAnimName != animName)
        {
            /*if (this is Player)
            {
                Debug.Log(animName);
            }*/
            anim.ResetTrigger(currentAnimName);
            currentAnimName = animName;
            anim.SetTrigger(currentAnimName);
        }
    }

    // Kiểm tra isDead, thay đổi heathBar, 
    public void OnHit(float damage)
    {
        if (!isDead)
        {
            hp -= damage;
            if (isDead) // Nếu chết hp = 0, gọi hàm OnDeath
            {
                hp = 0;
                OnDeath();
            }
            healthBar.SetNewHp(hp);
            // Tạo combatTextPrefab hiển thị damage
            Instantiate(combatTextPrefab, transform.position + Vector3.up, Quaternion.identity).OnInit(damage);
        }    
    }  
}
