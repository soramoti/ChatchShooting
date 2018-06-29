using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreText : MonoBehaviour {

    public Text m_scoreText;    // スコアのテキスト

    void Update()
    {
        var score = Data.m_instance;

        // スコアのテキストを表示する
        m_scoreText.text = score.Score.ToString();

    }
}
