using UnityEngine;
using UnityEngine.UI;               // UIを使うのに必要
using UnityEngine.SceneManagement; // シーンを使うのに必要
using TMPro; // TextMeshProを使うのに必要      

public class GameManager : MonoBehaviour
{
    public GameObject mainImage; // メイン画像
    public Sprite gameOverSpr; // ゲームオーバー画像
    public Sprite gameClearSpr; // ゲームクリア画像
    public GameObject panel; // パネル
    public GameObject restartButton; // リスタートボタン
    public GameObject nextButton; // 次のステージボタン

    Image titleImage; // タイトル画像

    public string nextSceneName; // 次のシーン名を設定するための変数

    //時間制限追加
    public GameObject timeBar; //時間表示イメージ
    public GameObject timeText; //時間テキスト
    TimeController timeCnt;        //TimeControllerコンポーネント

    //スコア追加
    public GameObject scoreText; // スコアテキスト
    public static int totalScore; // 合計スコア
    public int stageScore = 0; // ステージスコア

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke("InactiveImage", 1.0f); // 1秒後に画像を非表示にする
        panel.SetActive(false); // パネルを非表示にする

        //時間制限追加
        timeCnt = GetComponent<TimeController>();
        if (timeCnt != null)
        {
            if (timeCnt.gameTime == 0.0f)
            { 
                timeBar.SetActive(false);
            }
        }
        //スコア追加
        UpdateScore();
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.gameState == GameState.GameClear)
        {
            //ゲームクリア
            mainImage.SetActive(true);　//　画像表示
            panel.SetActive(true);      // ボタン表示
            //RESTARTボタンを無効化する
            Button bt = restartButton.GetComponent<Button>();
            bt.interactable = false;
            mainImage.GetComponent<Image>().sprite = gameClearSpr;　//画像をゲームクリアの画像に変更する
            PlayerController.gameState = GameState.GameEnd; //ゲームの状態をゲーム終了にする

            //時間制限追加
            if (timeCnt != null)
            {
                timeCnt.isTimeOver = true; //時間制限を止める
                //スコア追加
                //整数に代入することで小数点以下を切り捨てる
                int time = (int)timeCnt.displayTime; //残り時間をスコアにする
                totalScore += time * 10; //残り時間をスコアにする
            }
            //スコア追加
            totalScore += stageScore; //ステージスコアを合計スコアに加算する
            stageScore = 0; //ステージスコアをリセットする
            UpdateScore(); //スコアを更新する
        }
        else if (PlayerController.gameState == GameState.GameOver)
        {
            //ゲームオーバー
            mainImage.SetActive(true);  // 画像表示
            panel.SetActive(true);      // ボタン表示
            //RESTARTボタンを無効化する
            Button bt = nextButton.GetComponent<Button>();
            bt.interactable = false;
            mainImage.GetComponent<Image>().sprite = gameOverSpr;　//画像をゲームオーバーの画像に変更する
            PlayerController.gameState = GameState.GameEnd; //ゲームの状態をゲーム終了にする
            //時間制限追加
            if (timeCnt != null)
            {
                timeCnt.isTimeOver = true; //時間制限を止める
            }
        }
        else if (PlayerController.gameState == GameState.InGame)
        {
            //ゲーム中
            //プレイヤーのオブジェクトを取得する
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            //PlayerControllerのスクリプトを取得する
            PlayerController playerCnt = player.GetComponent<PlayerController>();
            //制限時間追加
            //タイムを更新する
            if (timeCnt != null)
            {
                if (timeCnt.gameTime > 0.0f)
                {
                    //整数に代入することで小数点以下を切り捨てる
                    int time = (int)timeCnt.displayTime; //残り時間をスコアにする
                    //タイム更新
                    timeText.GetComponent<TextMeshProUGUI>().text = time.ToString();
                    //タイムオーバー
                    if (time == 0)
                    {
                        playerCnt.GameOver(); //ゲームオーバーにする
                    }
                }
            }
            //スコア追加
            if (playerCnt.score != 0)
            {
                stageScore += playerCnt.score; //ステージスコアに加算する
                playerCnt.score = 0; //プレイヤースコアをリセットする
                UpdateScore(); //スコアを更新する
            }
        }
    }

    // 画像を非表示にする
    void InactiveImage()
    {
       mainImage.SetActive(false);
    }
    // スコアを更新する
    void UpdateScore()
    {
        int score = totalScore + stageScore; //合計スコアを計算する
        scoreText.GetComponent<TextMeshProUGUI>().text = score.ToString(); //スコアを表示する
    }

    // リスタートボタンが押されたときの処理
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // 現在のシーンをロードする
    }
    // 次のシーンに移動するボタンが押されたときの処理
    public void Next()
    {
        SceneManager.LoadScene(nextSceneName); // 次のシーンをロードする
    }

}
