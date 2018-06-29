using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data{

    public readonly static Data m_instance = new Data();   // データのインスタンスを取得する

    private static int m_score = 0; // スコアを格納する変数

    public int Score
    {
        get
        {
            return m_score;
        }
        set
        {
            m_score = value;
        }
    }


}
