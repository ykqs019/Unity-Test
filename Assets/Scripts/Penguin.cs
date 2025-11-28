//Animator와 SpriteRenderer를 함께 활용해서
//2D 캐릭터의 이동 + 점프 + 애니메이션 전환을 자연스럽게 처리하는 예시 스크립트

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Penguin : MonoBehaviour
{
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
        rb = GetComponent<Rigidbody2D>();
        //animator = GetComponent<Animator>();
        //spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
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