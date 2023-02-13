using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Transform aPoint, bPoint;
    [SerializeField] private float speed;

    Vector3 target;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = aPoint.position;
        target = bPoint.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Di chuyển giữa vị trí transform.position và target
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        if (Vector2.Distance(transform.position, aPoint.position) < 0.1f) // Nếu khoảng cách transform và aPoint < 0.1 thì target = vị trí bPoint
        {
            target = bPoint.position;
        }
        else if (Vector2.Distance(transform.position, bPoint.position) < 0.1f) // Nếu khoảng cách transform và bPoint < 0.1 thì target = vị trí aPoint
        {
            target = aPoint.position;
        }
    }
    // Nếu va chạm với đối tượng có tag Player, đặt đối tượng vào trong moving platform
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.transform.SetParent(transform);
        }
    }
    // Nếu kết thúc với đối tượng có tag Player, setParent cho đối tượng là null
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.transform.SetParent(null);
        }
    }
}