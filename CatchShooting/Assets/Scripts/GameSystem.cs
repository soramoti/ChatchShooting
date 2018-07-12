using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSystem : MonoBehaviour {
    //　スタートボタンを押したら実行する
    public void GameStart(){
        SceneManager.LoadScene("PlayScene");
    }

    //　ゲーム終了ボタンを押したら実行する
    public void GameEnd(){
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
    }

    // タイトルボタンを押したらタイトルへ戻る
    public void GameTitle(){
        SceneManager.LoadScene("TitleScene");
    }

    // リザルトシーンへ移動する
    public void GameResult(){
        SceneManager.LoadScene("ResultScene");
    }

}
