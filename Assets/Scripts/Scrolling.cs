using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scrolling : MonoBehaviour
{
    [Header("스크롤 설정")]
    public float ScrollSpeed = 1.0f; // 블록이 움직일 속도
    /*public float tileWidth = 5f; // 블록의 가로길이를 조절하는것으로 현재는 비활성화 시킴.*/

    [Header("루프 설정")]
    public float disablePositionX = -15f; // 블록이 화면밖으로 사라지는 좌표
    public float restartPositionX = 40f; // 블록이 다시 나타날 좌표(현재 타일 6개 기준.(폭 5 타일 3개)(폭 8 타일 3개) 총 합 39

    // Update is called once per frame
    void Update()
    {
        // 타일이동
        transform.position += Vector3.left * ScrollSpeed * Time.deltaTime;
        // 리셋 확인
        if (transform.position.x <= restartPositionX)
        {
            // 타일을 restartPositionX 위치로 워프시켜 무한 루프를 만든다
            Vector3 newPos = transform.position;
            newPos.x = restartPositionX;
            transform.position = newPos;
        }
    }
}
