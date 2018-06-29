using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// 敵の出現位置の種類
public enum RESPAWN_TYPE
{
    UP, // 上
    RIGHT, // 右
    DOWN, // 下
    LEFT, // 左
    SIZEOF, // 敵の出現位置の数
}

public class Enemy : MonoBehaviour {

    public Vector2 m_respawnPosInside; // 敵の出現位置（内側）
    public Vector2 m_respawnPosOutside; // 敵の出現位置（外側）
    public float m_speed; // 移動する速さ
    public int m_hpMax; // HP の最大値
    public int m_exp; // この敵を倒した時に獲得できる経験値
    public int m_catchExp;  // この敵を捕まえた時に獲得できる経験値
    public int m_damage; // この敵がプレイヤーに与えるダメージ

    private int m_hp; // HP
    private Vector3 m_direction; // 進行方向

    public Explosion m_explosionPrefab; // 爆発エフェクトのプレハブ

    public bool m_isFollow; // プレイヤーを追尾する場合 true
    public bool m_isEscape; // プレイヤーから逃げる場合 true
    private bool m_isAwake = true; // 敵が起きていたらtrue気絶していたらfalse

    public Gem[] m_gemPrefabs; // 宝石のプレハブを管理する配列
    public float m_gemSpeedMin; // 生成する宝石の移動の速さ（最小値）
    public float m_gemSpeedMax; // 生成する宝石の移動の速さ（最大値）

    public AudioClip m_deathClip; // 敵を倒した時に再生する SE

    private int m_count = 0;    // 敵の気絶時間をカウント
    private int m_num;  // 気絶する時間をランダムで決める

    public int m_arivaCount;    // 気絶回数が一定を超えたら死亡する

    public int m_downScore; // 気絶したときにもらえるスコア
    public int m_catchScore;　// 捕まえた時にもらえるスコア

    // 逃げるモードの変数
    public float m_escapeDistance = 50;
    //private bool m_isEscapeMode;

    private Sprite m_awakeSprite;  // 
    public Sprite m_downSprite;    // 気絶時に切り替える画像

    Data m_score = Data.m_instance; // スコアを取得

    // 敵が生成された時に呼び出される関数
    private void Start()
    {
        // HP を初期化する
        m_hp = m_hpMax;

        // 敵の気絶時間を３つ（3から５秒）からランダムに決める
        m_num = Random.Range(0, 3);

        m_awakeSprite = GetComponent<SpriteRenderer>().sprite;
    }

    // 毎フレーム呼び出される関数
    private void Update()
    {
        // 気絶していなっかたった場合
        if (m_isAwake)
        {
            var renderer = GetComponent<SpriteRenderer>();
            renderer.sprite = m_awakeSprite;

            // プレイヤーを追尾する場合
            if (m_isFollow)
                {
                    GetComponent<FollowEnemy>().Follow(m_speed);
                    return;
                }

                // プレイヤーから逃げる敵の場合
                if (m_isEscape)
                {
                    m_direction = GetComponent<EscapeEnemy>().Escape(m_escapeDistance, m_direction);
                }

                // まっすぐ移動する
                transform.localPosition += m_direction * m_speed;


            }
            else
            {
                var renderer = GetComponent<SpriteRenderer>();
                renderer.sprite = m_downSprite;

                m_count++;

                switch (m_num)
                {
                    case 0:
                        if (m_count % 180 == 0)
                        {
                            m_isAwake = true;
                            m_hp = m_hpMax;
                        }
                        break;
                    case 1:
                        if (m_count % 240 == 0)
                        {
                            m_isAwake = true;
                            m_hp = m_hpMax;
                        }
                        break;
                    case 2:
                        if (m_count % 300 == 0)
                        {
                            m_isAwake = true;
                            m_hp = m_hpMax;
                        }
                        break;

                }
        }

        // 気絶回数が一定を超えたら敵を消す
        if(m_arivaCount <= 0)
        {
            // 超えて敵が死亡した場合スコアを減らす
            m_score.Score -= 10;

            // 敵を消す
            Destroy(gameObject);
        }

        // 一定エリアの外に出たら消す
        if (Utils.IsOverArea(transform.localPosition))
        {
            Destroy(gameObject);
        }

    }

    // 敵が出現する時に初期化する関数
    public void Init(RESPAWN_TYPE respawnType)
    {
        Vector2 pos = Vector3.zero;

        // 指定された出現位置の種類に応じて、出現位置と進行方向を決定する
        switch (respawnType)
        {
            // 出現位置が上の場合
            case RESPAWN_TYPE.UP:
                pos.x = Random.Range(
                    -m_respawnPosInside.x, m_respawnPosInside.x);
                pos.y = m_respawnPosOutside.y;
                break;

            // 出現位置が右の場合
            case RESPAWN_TYPE.RIGHT:
                pos.x = m_respawnPosOutside.x;
                pos.y = Random.Range(
                    -m_respawnPosInside.y, m_respawnPosInside.y);
                break;

            // 出現位置が下の場合
            case RESPAWN_TYPE.DOWN:
                pos.x = Random.Range(
                    -m_respawnPosInside.x, m_respawnPosInside.x);
                pos.y = -m_respawnPosOutside.y;
                break;

            // 出現位置が左の場合
            case RESPAWN_TYPE.LEFT:
                pos.x = -m_respawnPosOutside.x;
                pos.y = Random.Range(
                    -m_respawnPosInside.y, m_respawnPosInside.y);
                break;
        }

        m_direction = MoveSlanting(pos, respawnType);
        // 位置を反映する
        transform.localPosition = pos;
    }

    // 他のオブジェクトと衝突した時に呼び出される関数
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // プレイヤーと衝突した場合
        if (collision.name.Contains("Player"))
        {
            if(m_isAwake)
            {
                // プレイヤーにダメージを与える
                var player = collision.GetComponent<Player>();
                player.Damage(m_damage);
                return;
            }
        }

        // 敵が気絶していて籠に衝突した場合
        if(collision.name.Contains("Basket"))
        {
            if(!m_isAwake)
            {
                // 捕まえた時に経験値をプレイヤーが獲得
                var player = FindObjectOfType<Player>();
                player.AddExp(m_catchExp);

                // 捕まえた時にスコアを獲得
                m_score.Score += m_catchScore;

                // 敵を削除する
                Destroy(gameObject);
            }
        }

        // 弾と衝突した場合
        if (collision.name.Contains("Shot"))
        {
            // 弾が当たった場所に爆発エフェクトを生成する
            Instantiate(
                m_explosionPrefab,
                collision.transform.localPosition,
                Quaternion.identity);

            // 弾を削除する
            Destroy(collision.gameObject);

            if(m_isAwake)
            {
                // 敵の HP を減らす
                m_hp--;

                // 敵の HP がまだ残っている場合はここで処理を終える
                if (0 < m_hp) return;

                // 敵を倒した時の SE を再生する
                var audioSource = FindObjectOfType<AudioSource>();
                audioSource.PlayOneShot(m_deathClip);

                // 敵を気絶状態にする
                m_isAwake = false;
                m_arivaCount -= 1;

                // 敵が気絶したときにスコアを獲得
                m_score.Score += m_downScore;

                // 敵が死亡した場合は宝石を散らばらせる
                var exp = m_exp;

                while (0 < exp)
                {
                    // 生成可能な宝石を配列で取得する
                    var gemPrefabs = m_gemPrefabs.Where(c => c.m_exp <= exp).ToArray();

                    // 生成可能な宝石の配列から、生成する宝石をランダムに決定する
                    var gemPrefab = gemPrefabs[Random.Range(0, gemPrefabs.Length)];

                    // 敵の位置に宝石を生成する
                    var gem = Instantiate(
                        gemPrefab, transform.localPosition, Quaternion.identity);

                    // 宝石を初期化する
                    gem.Init(m_exp, m_gemSpeedMin, m_gemSpeedMax);

                    // まだ宝石を生成できるかどうか計算する
                    exp -= gem.m_exp;
                }

            }

        }
    }

    // 敵の移動の方向を決める関数
    public Vector2 MoveSlanting(Vector2 position, RESPAWN_TYPE respawnType)
    {
        Vector2 direction = Vector2.zero;

        switch (respawnType)
        {
            // 出現位置が上だった時
            case RESPAWN_TYPE.UP:
                // 中心より右側か左側か
                if(position.x < 0)
                {
                    direction = new Vector2(Random.Range(0.0f, 1.0f), -1.0f);
                }
                else
                {
                    direction = new Vector2(Random.Range(-1.0f, 0.0f), -1.0f);
                }
                break;
            // 出現位置が右だった時
            case RESPAWN_TYPE.RIGHT:
                if (position.y < 0)
                {
                    direction = new Vector2(-1.0f, Random.Range(0.0f, 1.0f));
                }
                else
                {
                    direction = new Vector2(-1.0f, Random.Range(-1.0f, 0.0f));
                }
                break;
            case RESPAWN_TYPE.DOWN:
                // 中心より右側か左側か
                if (position.x < 0)
                {
                    direction = new Vector2(Random.Range(0.0f, 1.0f), 1.0f);
                }
                else
                {
                    direction = new Vector2(Random.Range(-1.0f, 0.0f), 1.0f);
                }
                break;
            case RESPAWN_TYPE.LEFT:
                if (position.y < 0)
                {
                    direction = new Vector2(1.0f, Random.Range(0.0f, 1.0f));
                }
                else
                {
                    direction = new Vector2(1.0f, Random.Range(-1.0f, 0.0f));
                }
                break;

        }
        return direction;
    }
}
