//Animator와 SpriteRenderer를 함께 활용해서
//2D 캐릭터의 이동 + 점프 + 애니메이션 전환을 자연스럽게 처리하는 예시 스크립트

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Penguin : MonoBehaviour
{
    public GameObject fx;
    [Header("게임 결과 등록")]
    public GameObject gameClear;
    public GameObject gameOver;

    int item_count = 0;
    public TextMeshProUGUI item_txt;
    //"체력바 설정, 게임 종료 설정, 이동설정"
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "item")
        {
            Destroy(collision.gameObject);
            item_count++;
           GameObject G = Instantiate(fx, collision.transform.position, collision.transform.rotation);
            Destroy(G, 1.0f);
        }
        if (collision.tag == "Enemy")
        {
            Destroy(collision.gameObject);
            hp_cur = hp_cur - 250;
            if (hp_cur <= 0)
            {
                //게임종료
                gameOver.SetActive(true);
                Time.timeScale = 0.0f;
            }
        }
        if (collision.tag == "Finish")
        {
            //게임종료
            gameClear.SetActive(true);
            GameObject G = Instantiate(fx, collision.transform.position, collision.transform.rotation);
            Destroy(G, 1.0f);
        }
    }
    [Header("체력바 설정")]
    public Slider hp_bar;
    int hp_max = 1000;
    int hp_cur = 1000;

    [Header("이동 설정")]
    public float moveSpeed = 5f;                //
    public float jumpForce = 7f;                //

    [Header("점프 체크")]
    public Transform groundCheck;               //Transform
    public float groundCheckRadius = 0.2f;      //
    public LayerMask groundLayer;               //Ground Layer

    private Rigidbody2D rb;             
    public Animator animator;                   //Animator
    public SpriteRenderer spriteRenderer;       //SpriteRenderer

    private bool isGrounded;
    private float moveInput;
    private bool isJumping;

    void Start()
    {
        Time.timeScale = 1.0f;
        rb = GetComponent<Rigidbody2D>();
        //animator = GetComponent<Animator>();
        //spriteRenderer = GetComponent<SpriteRenderer>();
    }
    bool pause = false; // 테스트

    // 업데이트 -> |스프라이트 방향 전환| |애니메이션 파라미터 설정| |hp바 업데이트||
    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Escape))
        {
            pause = !pause;
            if(pause) Time.timeScale = 0.0f;
            else
                Time.timeScale = 1.0f;
        }*/

            

        // 좌우 입력
        moveInput = Input.GetAxisRaw("Horizontal");

        // 점프 입력
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            isJumping = true;
        }

        // 스프라이트 방향 전환
        if (moveInput != 0)
        {
            spriteRenderer.flipX = moveInput < 0;
        }

        // hp바 업데이트
        hp_bar.value = (float)hp_cur / hp_max;

        item_txt.text = item_count.ToString();

        if(Input.anyKeyDown)

        // 애니메이션 파라미터 설정 ***
        animator.SetFloat("Speed", Mathf.Abs(moveInput)); // 이동 속도
        animator.SetFloat("yVelocity", rb.velocity.y);    // 점프 중 판단용
        animator.SetBool("isGrounded", isGrounded);       // 지면 여부
    }

    void FixedUpdate()
    {
        // 이동 처리
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        // 점프 처리
        if (isJumping)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isJumping = false;
        }

        // 바닥 체크 ***
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}

/*
유니티 설정 방법

Player 오브젝트에 추가

	Rigidbody2D 추가 → Gravity Scale을 적절히 설정 (예: 3)
	BoxCollider2D 또는 CapsuleCollider2D 추가
	위 스크립트 추가

groundCheck 설정

	Player의 하단에 빈 오브젝트(groundCheck) 생성
	스크립트의 groundCheck 필드에 드래그해서 연결
	groundLayer를 “Ground” 태그나 새 레이어로 지정하고, 실제 땅 오브젝트에 같은 레이어를 설정


Input

	기본적으로 Horizontal (A/D 또는 ←/→), Jump (스페이스) 입력을 사용


유니티 에니메이션 설정 방법
 
    // 애니메이션 파라미터 설정
    Float - Speed      // 이동 속도
    Float - yVelocity  // 점프 중 판단용
    Bool - isGrounded  // 지면 여부

    // 전환 : 조건
    
    idle -> walk :  Speed > 0.1
    walk -> idle :  Speed <= 0.1 
    idle -> jump : isGrounded == false &&  yVelocity > 0
    //any -> fall : isGrounded == false && yVelocity < 0
    //fall -> idle
    jump -> idle : isGrounded == true

*/