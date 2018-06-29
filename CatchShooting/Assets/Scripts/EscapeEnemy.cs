using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// プレイヤーから逃げる敵
public class EscapeEnemy : MonoBehaviour
{

    public Vector3 Escape(float escapeDistance, Vector3 direction)
    {
        // プレイヤーと敵の距離の計算
        var player = FindObjectOfType<Player>();
    
        // 一定距離に近づいたら 逃げるモードになる
        var distance = Vector2.Distance(player.transform.position, transform.position);
    
        if (distance < escapeDistance) // 調整必要
        {
        
            // プレイヤーと敵の向きを判定して
            // 逆ベクトルを作って正規化して単位ベクトルにする
            
            var dir = player.transform.position - transform.position;
            dir.Normalize();
            direction = -(dir * 0.5f);
            return direction;

        }
        else
        {
            return direction;
        }
    }
}
