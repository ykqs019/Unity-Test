using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicTileScroller : MonoBehaviour
{
    [Header("타일 설정")]
    public GameObject[] tilePrefab;         // 무작위로 사용할 타일 프리팹 목록 (흰색 블록)
    public Transform spawn;                 // 타일이 생성될 기준 위치

    [Header("스크롤 설정")]
    public float tileWidth = 6f;            // 타일 하나의 가로 폭
    public int creatCount = 6;              // 게임 시작 시 미리 생성할 타일 개수
    public float scrollSpeed = 2f;          // 타일 이동 속도
    public float disablePositionX = -15f;   // 타일이 화면 밖으로 사라지는 기준 X 좌표


    private List<GameObject> spawnedTiles = new List<GameObject>();
    void Start()
    {
        for (int i = 0; i < creatCount; i++)
        {
            SpawnNewTile(i);
        }
    }

   

    // Update is called once per frame
    void Update()
    {
        // 3. 타일 스크롤 및 파괴/재생성 로직

        // 주의: 리스트에서 제거가 일어날 수 있으므로 뒤에서부터 검사합니다.
        for (int i = spawnedTiles.Count - 1; i >= 0; i--)
        {
            GameObject tile = spawnedTiles[i];

            // 타일 이동: 매 프레임 왼쪽으로 이동
            tile.transform.position += Vector3.left * scrollSpeed * Time.deltaTime;

            // 파괴 조건 확인
            if (tile.transform.position.x < disablePositionX)
            {
                // 타일이 화면 밖으로 완전히 나갔다면
                Destroy(tile);
                spawnedTiles.RemoveAt(i);

                // 새로운 타일(또는 깃발)을 오른쪽 끝에 추가
                SpawnTileAtEnd();
            }
        }
    }

    void SpawnNewTile(int index)
    {
        // 랜덤 타일을 골라 복사합니다.
        int randIndex = Random.Range(0, tilePrefab.Length);
        GameObject newTile = Instantiate(tilePrefab[randIndex]);

        // 위치 지정: spawnPoint를 기준으로 index * tileWidth 만큼 오른쪽으로 이동
        newTile.transform.position = spawn.position + new Vector3(index * tileWidth, 0, 0);

        spawnedTiles.Add(newTile);
    }

    void SpawnTileAtEnd()
    {
        // 무작위 타일을 생성합니다.
        int randIndex = Random.Range(0, tilePrefab.Length);
        GameObject newTile = Instantiate(tilePrefab[randIndex]);

        // 새 타일 위치: 현재 가장 오른쪽 타일의 위치를 기준으로 설정
        Vector3 lastPos = spawnedTiles.Count > 0
            ? spawnedTiles[spawnedTiles.Count - 1].transform.position
            : spawn.position; // 리스트가 비었다면 spawnPoint를 기준으로

        // 가장 오른쪽 타일의 오른쪽에 tileWidth만큼 더해 붙여서 배치
        newTile.transform.position = lastPos + new Vector3(tileWidth, 0, 0);

        spawnedTiles.Add(newTile);
    }
}
