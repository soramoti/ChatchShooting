using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// プレイヤーを追尾する敵
public class FollowEnemy : MonoBehaviour {

    public void Follow (float speed) {
            // プレイヤーの現在位置へ向かうベクトルを作成する
            var angle = Utils.GetAngle(
            transform.localPosition,
            Player.m_instance.transform.localPosition);
            var direction = Utils.GetDirection(angle);

            // プレイヤーが存在する方向に移動する
            transform.localPosition += direction * speed;

            // プレイヤーが存在する方向を向く
            var angles = transform.localEulerAngles;
            angles.z = angle - 90;
            transform.localEulerAngles = angles;
    }

}
