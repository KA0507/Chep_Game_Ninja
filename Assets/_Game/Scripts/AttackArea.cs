using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
    // Đối tượng va chạm có tag là Player hoặc Enemy, gọi hàm OnHit
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" || collision.tag == "Enemy")
        {
            collision.GetComponent<Character>().OnHit(30f);
        }
    }
}
