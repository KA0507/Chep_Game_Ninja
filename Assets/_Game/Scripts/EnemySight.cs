using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySight : MonoBehaviour
{
    public Enemy enemy;
    // Nếu va chạm với đối tượng có tag là Player, thay đổi target enemy là vị trí của nhân vật
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            enemy.SetTarget(collision.GetComponent<Character>());
        }
    }
    // Nếu kết thúc va chạm với đối tượng có tag là Player, thay đổi target enemy là null
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            enemy.SetTarget(null);
        }
    }
}
