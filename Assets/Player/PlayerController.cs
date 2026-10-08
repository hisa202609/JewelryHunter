using UnityEngine;

public enum GameState           // ゲームの状態
{
    InGame,                     // ゲーム中
    GameClear,                  // ゲームクリア
    GameOver,                   // ゲームオーバー
    GameEnd,                    // ゲーム終了
}

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rbody;              // Rigidbody2D型の変数
    float axisH = 0.0f;             // 入力
    public float speed = 3.0f;      // 移動速度   
    public float jump = 9.0f;       // ジャンプ力
    public LayerMask groundLayer;   // 着地できるレイヤー
    bool goJump = false;            // ジャンプ開始フラグ
    bool onGround = false;          // 地面フラグ

    // ゲームの状態（テキストは誤植なのでここは次の記述が正解）
    public static GameState gameState = GameState.InGame;

    // アニメーション対応
    Animator animator; //アニメーター
    public string stopAnime = "PlayerStop";
    public string moveAnime = "PlayerMove";
    public string jumpAnime = "PlayerJump";
    public string deadAnime = "PlayerOver";
    public string goalAnime = "PlayerGoal";
    string nowAnime = ""; //現在のアニメーション
    string oldAnime = ""; //前のアニメーション

    //カメラ制御
    public float camLeft = 0.0f; //カメラの左端
    public float camRight = 0.0f; //カメラの右端
    public float camTop = 0.0f; //カメラの上端
    public float camBottom = 0.0f; //カメラの下端

    // 多重スクロール
    public GameObject subScreen;    // サブスクリーン

    // 強制スクロール
    public bool isForceScrollX = false; // 強制スクロールフラグ
    public float forceScrollSpeedX = 0.5f; // 1秒間に移動する距離
    public bool isForceScrollY = false; // 強制スクロールフラグ
    public float forceScrollSpeedY = 0.5f; // 1秒間に移動する距離

    public int score = 0; // スコア


    void Start()
    {
        gameState = GameState.InGame;

        rbody = this.GetComponent<Rigidbody2D>();   // Rigidbody2Dを取ってくる
        animator = this.GetComponent<Animator>();         // Animatorを取ってくる
        nowAnime = stopAnime; //初期アニメーションは停止
        oldAnime = stopAnime; //停止から開始する

    }

    void Update()
    {
        if (gameState != GameState.InGame) // ゲームの状態を初期化する
        {
            return; //このフレームをキャンセル
        }

        //地上判定
        onGround = Physics2D.CircleCast(transform.position,     // 発射位置
                                        0.2f,                   // 円の半径
                                        Vector2.down,           // 発射方向
                                        0.0f,                   // 発射距離
                                        groundLayer);           // 検出するレイヤー
        if (Input.GetButtonDown("Jump"))    // キャラクターをジャンプさせる
        {
            goJump = true; // ジャンプフラグを立てる
        }

        axisH = Input.GetAxisRaw("Horizontal");     //水平方向の入力をチェックする


        if (axisH > 0.0f)                           // 向きの調整
        {
            transform.localScale = new Vector2(1, 1);   // 右移動
        }
        else if (axisH < 0.0f)
        {
            transform.localScale = new Vector2(-1, 1); // 左右反転させる
        }
        // アニメーションの更新
        if (onGround)   // 地面の上
        {
            if (axisH == 0)     // 停止
            {
                nowAnime = stopAnime;
            }
            else                // 移動
            {
                nowAnime = moveAnime;
            }
        }
        else            // 空中
        {
            nowAnime = jumpAnime;
        }
        if (nowAnime != oldAnime)   // アニメーションが変わったら
        {
            oldAnime = nowAnime;       // 現在のアニメーションを保存する
            animator.Play(nowAnime);  // アニメーションを再生する
        }

        // カメラの制御
        float x;
        float y;
        if (isForceScrollX) // 強制スクロールが有効な場合
        {
            x = Camera.main.transform.position.x + (forceScrollSpeedX * Time.deltaTime); // 強制スクロールの速度を加算する
        }
        else
        {
            x = Mathf.Clamp(transform.position.x, camLeft, camRight); // カメラの位置を制限する
        }
        if (isForceScrollY) // 強制スクロールが有効な場合
        {
            y = Camera.main.transform.position.y + (forceScrollSpeedY * Time.deltaTime); // 強制スクロールの速度を加算する
        }
        else
        {
            y = Mathf.Clamp(transform.position.y, camBottom, camTop); // カメラの位置を制限する
        }

        //カメラに与えるべき理想の値を変数に代入
        Vector3 camPos = new Vector3(x, y, -10); // カメラの位置を設定する
        Camera.main.transform.position = camPos; // カメラの位置を設定する

        // 多重スクロール
        if (subScreen != null) // サブスクリーンが設定されている場合
        {
            y = subScreen.transform.position.y; // サブスクリーンの位置を設定する
            Vector3 subPos = new Vector3(x / 2.0f, y, subScreen.transform.position.z); // サブスクリーンの位置を設定する
            subScreen.transform.position = subPos;
        }
        
    }

    void FixedUpdate()
    {
        if (gameState != GameState.InGame) // ゲームの状態を初期化する
        {
            return; //このフレームをキャンセル
        }

        if (onGround || axisH != 0)     // 地面の上 or 速度が 0 ではない
        {
            //速度を更新する
            rbody.linearVelocity = new Vector2(axisH * speed, rbody.linearVelocity.y);
        }
        if (onGround && goJump)         // 地面の上でジャンプキーが押された
        {
            // ジャンプさせる
            Vector2 jumpPw = new Vector2(0, jump);      //ジャンプさせるベクトルを作る
            rbody.AddForce(jumpPw, ForceMode2D.Impulse);   //瞬間的な力を加える
            goJump = false;
        }
    }

    // 接触開始
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Goal")
        {
            Goal();         // ゴール！！
        }
        else if (collision.gameObject.tag == "Dead")
        {
            GameOver();     // ゲームオーバー
        }
        else if (collision.gameObject.tag == "ScoreItem")
        {
            // スコアアイテムに接触した場合
            ScoreItem item = collision.gameObject.GetComponent<ScoreItem>();
            score = item.itemdata.value;
            Destroy(collision.gameObject);
        }
    }
    // ゴール
    public void Goal()
    {
        animator.Play(goalAnime);  // ゴールアニメーションを再生する
        gameState = GameState.GameClear; // ステータス変更
        GameStop(); // ゲーム停止
    }
    // ゲームオーバー
    public void GameOver()
    {
        animator.Play(deadAnime);  // ゲームオーバーアニメーションを再生する
        gameState = GameState.GameOver;　// ステータス変更
        GameStop(); // ゲーム停止
        // ゲームオーバー演出
        GetComponent<CapsuleCollider2D>().enabled = false;　        // 当たり判定を無効化する
        rbody.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);     // 上に少し跳ね上がる
    }

    // ゲーム停止
    void GameStop()
    {
        rbody.linearVelocity = new Vector2(0, 0); // 速度を 0 にする
    }
}
