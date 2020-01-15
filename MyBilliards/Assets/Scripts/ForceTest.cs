using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceTest : MonoBehaviour
{
    float distance;
    GameObject ball;
    GameObject que;
    // 에디터 안 Inspector에서 편집 가능한 속도 조정 변수 생성
    public float speed;
    // Rigidbody 형태의 변수 생성
    private Rigidbody rb;

    // 스크립트가 활성화된 첫 프레임에 호출
    void Start()
    {
        que = GameObject.Find("GeoSphere004");
        ball = GameObject.Find("GeoSphere001");
        // 현재 오브젝트의 Rigidbody를 참조
        rb = GetComponent<Rigidbody>();
    }

    // 프레임을 렌더링 하기 전에 호출
    void Update()
    {

    }

    // 물리효과 계산을 수행하기 전에 호출
    void FixedUpdate()
    {
        if(Input.GetKey(KeyCode.Space)) // 스페이스바 눌렀을 때 타격 공과 큐의 거리 계산 => 거리만큼을 힘으로 쓸려고
            distance = Vector3.Distance(que.transform.position, ball.transform.position);
      

        // 플레이어의 Rigidbody에서 movement 값만큼 힘을 가해서 이동시킴
        rb.AddForce(Vector3.left * distance* 20f);
    }
}
