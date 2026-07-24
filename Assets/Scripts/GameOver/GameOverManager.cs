using UnityEngine;

//ステータス処理用のenum
enum GameStatus
{
    Title,
    Playing,
    GameClear,
    GameOver
}

public class gameR : MonoBehaviour
{
    //GameStatus型の変数
    GameStatus gameStatus;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameStatus = GameStatus.Title;
    }

    public void GameOver()
    {
        if (gameStatus == GameStatus.GameOver)
            return;

        gameStatus = GameStatus.GameOver;
        Debug.Log("ゲームオーバー");
    }

    // Update is called once per frame
    void Update()
    {
        PlayStatus();
    }

    //ステータス管理
    void PlayStatus()
    {
        switch (gameStatus)
        {
            case GameStatus.Title:
                break;

            case GameStatus.Playing:
                break;

            case GameStatus.GameClear:
                break;

            //case GameStatus.GameOver:
            //    Debug.Log("ゲームオーバー");
            //    break;

            default:
                break;
        }
    }
}

