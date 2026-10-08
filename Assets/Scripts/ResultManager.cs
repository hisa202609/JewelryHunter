using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ResultManager : MonoBehaviour
{   
    //パターン1
    public TextMeshProUGUI scoreText; // コンポーネントから取得する場合
    //パターン2
    public GameObject scoreTextObject;　//ゲームオブジェクトから取得する場合

    public string sceneName; // 遷移するシーン名

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // パターン1
        scoreText.text = GameManager.totalScore.ToString(); // コンポーネントからtextを指定
        // パターン2
        scoreTextObject.GetComponent<TextMeshProUGUI>().text = GameManager.totalScore.ToString(); // ゲームオブジェクトからtextを指定
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //シーンを読み込む
    public void Load()
    { 
     SceneManager.LoadScene(sceneName);
    }
}
