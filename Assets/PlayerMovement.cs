using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public float moveTime = 0.3f;    // 每一格滑動的時間
    public float pauseTime = 0.2f;   // 到達後的停頓時間
    public float gridSize = 1f;
    private bool isMoving = false;
    private Animator anim;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (anim != null) anim.speed = 0;
    }

    void Update()
    {
        if (isMoving) return;
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        if (h != 0)
        {
            spriteRenderer.flipX = (h > 0); // 依原圖方向調整 (h > 0 或 h < 0)
            StartCoroutine(MovePlayer(new Vector3(h * gridSize, 0, 0), "Walk_Side"));
        }
        else if (v != 0)
        {
            spriteRenderer.flipX = false;
            StartCoroutine(MovePlayer(new Vector3(0, v * gridSize, 0), "Walk_Front"));
        }
    }

    IEnumerator MovePlayer(Vector3 direction, string animName)
    {
        isMoving = true;
        if (anim != null) { anim.Play(animName); anim.speed = 1; }
        Vector3 startPos = transform.position;
        Vector3 targetPos = startPos + direction;
        float elapsedTime = 0;
        while (elapsedTime < moveTime)
        {
            transform.position = Vector3.Lerp(startPos, targetPos, (elapsedTime / moveTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPos;
        if (anim != null) anim.speed = 0;
        yield return new WaitForSeconds(pauseTime);
        isMoving = false;
    }
}