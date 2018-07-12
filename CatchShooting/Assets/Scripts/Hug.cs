using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hug : MonoBehaviour {

    public Image m_timeGauge; // 時間ゲージ
    public Image m_expGauge; // 経験値ゲージ

    public Text m_timeText; // 時間のテキスト
    public Text m_levelText;// レベルのテキスト

    private void Update(){
        // プレイヤーを取得する
        var player = Player.m_instance;
        var time = FindObjectOfType<PlayContolloer>();

        // 時間のゲージの表示を更新する
        m_timeGauge.fillAmount = time._Time / time.MaxTime;

        m_timeText.text = ((int)time._Time).ToString();

        // 経験値のゲージの表示を更新する
        var exp = player.m_exp;
        var prevNeedExp = player.m_prevNeedExp;
        var needExp = player.m_needExp;
        m_expGauge.fillAmount =
            (float)(exp - prevNeedExp) / (needExp - prevNeedExp);

        // レベルのテキストの表示を更新する
        m_levelText.text = player.m_level.ToString();

    }
}
