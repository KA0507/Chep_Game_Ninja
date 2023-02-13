using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Kunai kunaiPrefab;
    [SerializeField] private Transform throwPoint;
    [SerializeField] private GameObject attackArea;

    [SerializeField] private float speed = 5;
    [SerializeField] private float jumpForce = 350;

    private bool isGrounded = true;
    private bool isAttack = false;
    public bool isJumping = false;

    private float horizontal;

    private int coin = 0;

    private Vector3 savePoint;

    // Khởi tạo giá trị 0 với khóa là coin
    private void Awake()
    {
        // coin = PlayerPrefs.GetInt("coin", 0);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Update");
        //Debug.LogError("Update");

        if (isDead)
        {
            return;
        }

        isGrounded = CheckGrounded();
        // Nhập phím di chuyển
        horizontal = Input.GetAxisRaw("Horizontal");

        // Kiểm tra có trên mặt đất không
        if (isGrounded)
        {           
            // Bấm phím space để nhảy
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                Jump();
            }
            // Nếu đang jump thì return
            if (isJumping)
            {
                return;
            }

            // change anim run
            if (Mathf.Abs(horizontal) > 0.1f && !isAttack)
            {
                ChangeAnim("run");
            }

            // Bấm phím z và trên mặt đất để attack
            if (Input.GetKey(KeyCode.Z) && isGrounded)
            {
                Attack();
            }

            // Bấm phím x và trên mặt đất để throw
            if (Input.GetKeyDown(KeyCode.X) && isGrounded)
            {
                Throw();
            }

        }
        
        // check falling
        if (!isGrounded && rb.velocity.y < 0)
        {
            ChangeAnim("fall");
            isJumping = false;
        }
        // Nếu đang attack nhân vật không di chuyển
        if (isAttack)
        {
            rb.velocity = Vector2.zero;
            return;
        }
        // Moving
        if (Mathf.Abs(horizontal) > 0.1f)
        {
            // Di chuyển theo chiều ngang
            rb.velocity = new Vector2(horizontal * speed * Time.deltaTime, rb.velocity.y);
            // horizontal > 0 -> tra ve 0, neu horizontal <= 0 -> tra ve 180
            transform.rotation = Quaternion.Euler(new Vector3(0, horizontal > 0 ? 0 : 180, 0));
            // transform.localScale = new Vector3(horizontal, 1, 1);
        }
        // Chuyển snag anim idle
        else if (isGrounded)
        {
            ChangeAnim("idle");
            rb.velocity = Vector2.zero;
        }
    }
    // Đặt giá trị ban đầu
    public override void OnInit()
    {
        base.OnInit();
        isAttack = false;

        transform.position = savePoint;
        ChangeAnim("idle");
        DeActiveAttack();

        SavePoint();
        UIManager.instance.SetCoin(0);

    }

    // Gọi hàm OnDespawn lớp Character và OnInit
    public override void OnDespawn()
    {
        base.OnDespawn();
        OnInit();
    }

    // Gọi hàm OnDeath lớp Character
    protected override void OnDeath()
    {
        base.OnDeath();
    }
    // Kiểm tra có trên mặt đất không
    private bool CheckGrounded()
    {
        // Tạo tia raycast, nếu tia raycast va chạm trả về true
        Debug.DrawLine(transform.position, transform.position + Vector3.down * 1.1f, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.1f, groundLayer);
        return hit.collider != null;
    }

    // Thực hiện anim attack
    public void Attack()
    {
        ChangeAnim("attack");
        isAttack = true;
        Invoke(nameof(ResetAttack), 0.5f); // Gọi hàm ResetAttack sau 0.5s
        ActiveAttack();
        Invoke(nameof(DeActiveAttack), 0.5f); // Gọi hàm DeActiveAttack sau 0.5s
    }
    
    // Thực hiện anim Throw
    public void Throw()
    {
        ChangeAnim("throw");
        isAttack = true;
        Invoke(nameof(ResetAttack), 0.5f); // Gọi hàm ResetAttack sau 0.5s
        // Tạo kunaiPrefab có vị trí và góc xoay bằng throwPoint
        Instantiate(kunaiPrefab, throwPoint.position, throwPoint.rotation); 

    }

    // Không tấn công chuyển sang anim idle
    private void ResetAttack()
    {
        isAttack = false;
        ChangeAnim("idle");
    }
    // Thực hiện anim jump
    public void Jump()
    {
        isJumping = true;
        ChangeAnim("jump");
        rb.AddForce(jumpForce * Vector2.up);
    }
    // Lưu vị trí hiện tại
    internal void SavePoint()
    {
        savePoint = transform.position;
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

    // Đặt hướng di chuyển
    public void SetMove(float horizontal)
    {
        this.horizontal = horizontal;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Nếu va chạm đối tượng có tag coin, coin = coin + 1, 
        if (collision.tag == "Coin")
        {
            coin++;
            /*Thay đổi giá trị với khóa là coin
            PlayerPrefs.SetInt("coin", coin);*/
            UIManager.instance.SetCoin(coin); // Thay đổi coin

            Destroy(collision.gameObject); 
        }
        // Nếu va chạm đối tượng có tag DeathZone, chuyển anim die, gọi hàm OnInit sau 1s
        if (collision.tag == "DeathZone")
        {
            ChangeAnim("die");
            Invoke(nameof(OnInit), 1f);
        }
    }
}
