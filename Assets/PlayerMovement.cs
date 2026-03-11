using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public float moveTime = 0.15f;    // 移動到下一格需要的時間 (越小越快)
    public float pauseTime = 0.1f;    // 抵達格子後的「停頓時間」 (越大越卡)
    public float gridSize = 1f;       // 格子大小
    private bool isMoving = false;

    void Update()
    {
        if (isMoving) return;

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        if (horizontal != 0)
        {
            StartCoroutine(MovePlayer(new Vector3(horizontal * gridSize, 0, 0)));
        }
        else if (vertical != 0)
        {
            StartCoroutine(MovePlayer(new Vector3(0, vertical * gridSize, 0)));
        }
    }

    IEnumerator MovePlayer(Vector3 direction)
    {
        isMoving = true;

        Vector3 startPos = transform.position;
        Vector3 targetPos = startPos + direction;

        float elapsedTime = 0;
        // 1. 平滑滑動到下一格
        while (elapsedTime < moveTime)
        {
            transform.position = Vector3.Lerp(startPos, targetPos, (elapsedTime / moveTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPos; // 確保對齊

        // 2. 【關鍵】抵達後強制停頓，產生卡頓節奏感
        yield return new WaitForSeconds(pauseTime);

        isMoving = false;
    }
}