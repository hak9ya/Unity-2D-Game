using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMove : MonoBehaviour
{
    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer spriteRenderer;
    CapsuleCollider2D capsuleCollider;
    public int nextMove;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();

        Invoke("Think", 2); // 주어진 시간이 지난 뒤, 지정된 함수를 실행
    }

    void FixedUpdate()
    {
        // 몬스터 움직임
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);

        // 오브젝트 체크
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.3f, rigid.position.y);
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
        // RayCastHit = Ray에 닿은 오브젝트
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platform"));

        if (rayHit.collider == null)
            Turn();

    }

    //재귀 함수
    void Think()
    {
        nextMove = Random.Range(-1, 2); // 랜덤으로 행동하게 만듬, AI 필수

        float nextThinkTime = Random.Range(2f, 5f);
        Invoke("Think", 5);

        // 애니메이션 변경
        anim.SetInteger("WalkSpeed", nextMove);
        
        // 방향전환
        if (nextMove != 0)
            spriteRenderer.flipX = nextMove == 1;

    }

    void Turn()
    {
        nextMove = nextMove * -1;
        spriteRenderer.flipX = nextMove == 1;
        CancelInvoke();
        Invoke("Think", 2);
    }

    public void OnDamaged()
    {
        // 투명도
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        // Y축 뒤집기
        spriteRenderer.flipY = true;

        // 충돌 이벤트 끔
        capsuleCollider.enabled = false;

        // 밟혔을 때 물리작용
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);

        // 시간차 두고 사라짐
        Invoke("DeActive", 2);
    }

    void DeActive()
    {
        gameObject.SetActive(false);
    }
}
