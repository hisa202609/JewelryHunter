using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 3.0f;          // 移動速度
    public bool isToRight = false;      // true=右向き　false=左向き
    public float revTime = 0;           // 反転時間
    public LayerMask groundLayer;       // 地面レイヤー
    bool onGround = false;              // 地面フラグ
    float time = 0;

    public float checkDistance = 0.6f; // 前方確認距離
    public float rayLength = 1.0f;     // Rayの長さ

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (isToRight)
        {
            transform.localScale = new Vector2(-1, 1);// 向きの変更
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 地上判定
        onGround = Physics2D.CircleCast(transform.position,    // 発射位置
                                        0.2f,                  // 円の半径
                                        Vector2.down,          // 発射方向
                                        0.0f,                  // 発射距離
                                        groundLayer);          // 検出するレイヤー
                                                               // 進行方向を求める
        float direction = isToRight ? 1.0f : -1.0f;

        // 敵の少し前からRayを飛ばす
        Vector2 rayStart = (Vector2)transform.position + new Vector2(direction * checkDistance, 0);

        // 真下にRaycast
        RaycastHit2D hit = Physics2D.Raycast(
            rayStart,
            Vector2.down,
            rayLength,
            groundLayer);

        // 前方に地面がなければ方向転換
        if (onGround && hit.collider == null)
        {
            isToRight = !isToRight;
            time = 0;

            if (isToRight)
            {
                transform.localScale = new Vector2(-1, 1);
            }
            else
            {
                transform.localScale = new Vector2(1, 1);
            }
        }


        if (revTime > 0)
        {
            time += Time.deltaTime;
            if (time >= revTime)
            {
                isToRight = !isToRight;
                time = 0;
                if (isToRight)
                {
                    transform.localScale = new Vector2(-1,1);
                }
                else
                {
                    transform.localScale = new Vector2(1,1);
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (onGround)
        {
            Rigidbody2D rbody = GetComponent<Rigidbody2D>();
            if (isToRight)
            {
                rbody.linearVelocity = new Vector2(speed, rbody.linearVelocity.y);
            }
            else
            {
                rbody.linearVelocity = new Vector2(-speed, rbody.linearVelocity.y);
            }
        }
    }

    // 接触
    private void OnTriggerEnter2D(Collider2D collision)
    {
        isToRight = !isToRight;
        time = 0;
        if (isToRight)
        {
            transform.localScale = new Vector2(-1,1);
        }
        else
        {
            transform.localScale = new Vector2(1,1);
        }
    }
}
