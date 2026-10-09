using UnityEngine;

public class MovingBlock : MonoBehaviour
{
    public float moveX = 0.0f;          //X移動距離
    public float moveY = 2.0f;          //Y移動距離
    public float times = 3.0f;          //時間
    public float wait = 0.0f;           //停止時間
    public bool isMoveWhenOn = false;   //乗った時に動くフラグ
    public bool isCanMove = true;       //動くフラグ
    Vector3 startPos;                   //初期位置
    Vector3 endPos;                     //移動位置
    bool isReverse = false;             //反転フラグ
    float movep = 0;                    //移動補完値

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPos = transform.position;                                 //初期位置
        endPos = new Vector2(startPos.x + moveX, startPos.y + moveY);  //移動位置

        //テキストをみて完成させましょう
        if (isMoveWhenOn)
        {
            isCanMove = false;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (isCanMove)
        {
            //テキストをみて完成させましょう
            float distance = Vector2.Distance(startPos, endPos);
            float ds = distance / times;
            float df = ds * Time.deltaTime;
            movep += df / distance;
            if(isReverse)
            {
                transform.position = Vector2.Lerp(endPos, startPos, movep);
            }
            else
            {
                transform.position = Vector2.Lerp(startPos, endPos, movep);
            }
            if(movep >= 1.0f)
            {
                movep = 0.0f;
                isReverse = !isReverse;
                isCanMove = false;
                if(isMoveWhenOn == false)
                {
                    Invoke("Move", wait);
                }
            }

        }
    }

    //移動フラグを立てる
    public void Move()
    {
        //テキストをみて完成させましょう
        isCanMove = true;
    }

    //移動フラグを下ろす
    public void Stop()
    {
        //テキストをみて完成させましょう
        isCanMove = false;
    }

    //接触開始
    void OnCollisionEnter2D(Collision2D collision)
    {
        //テキストをみて完成させましょう
        if (collision.gameObject.tag == "Player")
        {
            collision.transform.SetParent(transform);
            if (isMoveWhenOn)
            {
                isCanMove = true;
            }
        }
    }

    //接触終了
    void OnCollisionExit2D(Collision2D collision)
    {
        //テキストをみて完成させましょう
        if (collision.gameObject.tag == "Player")
        {
            collision.transform.SetParent(null);
        }
    }

    //移動範囲表示
    void OnDrawGizmosSelected()
    {
        Vector2 fromPos;
        if (startPos == Vector3.zero)
        {
            fromPos = transform.position;
        }
        else
        {
            fromPos = startPos;
        }
        //移動線
        Gizmos.DrawLine(fromPos, new Vector2(fromPos.x + moveX, fromPos.y + moveY));
        //スプライトのサイズ
        Vector2 size = GetComponent<SpriteRenderer>().size;
        //初期位置
        Gizmos.DrawWireCube(fromPos, new Vector2(size.x, size.y));
        //移動位置
        Vector2 toPos = new Vector3(fromPos.x + moveX, fromPos.y + moveY);
        Gizmos.DrawWireCube(toPos, new Vector2(size.x, size.y));
    }
}
