using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{

    // 移動可能な範囲
    public static Vector2 m_moveLimit = new Vector2(4.7f, 3.4f);

    // 敵の移動範囲
    public static Vector2 m_moveOverLimit = new Vector2(7.0f, 6.0f);

    // 指定エリアの範囲
    public static Vector2 m_AreaLimit = new Vector2(1.0f, 1.0f);

    // 指定された位置を移動可能な範囲に収めた値を返す
    public static Vector3 ClampPosition(Vector3 position){
        return new Vector3(
            Mathf.Clamp(position.x, -m_moveLimit.x, m_moveLimit.x),
            Mathf.Clamp(position.y, -m_moveLimit.y, m_moveLimit.y),
            0
        );
    }

    // 指定された 2 つの位置から角度を求めて返す
    public static float GetAngle(Vector2 from, Vector2 to){
        var dx = to.x - from.x;
        var dy = to.y - from.y;
        var rad = Mathf.Atan2(dy, dx);
        return rad * Mathf.Rad2Deg;
    }

    // 指定された角度（ 0 ～ 360 ）をベクトルに変換して返す
    public static Vector3 GetDirection(float angle){
        return new Vector3(
            Mathf.Cos(angle * Mathf.Deg2Rad),
            Mathf.Sin(angle * Mathf.Deg2Rad),
            0
        );
    }

    // 指定されたい位置から出ているかを判断する
    public static bool IsOverArea(Vector2 position){
        if (position.x < -m_moveOverLimit.x || position.x > m_moveOverLimit.x || position.y < -m_moveOverLimit.y || position.y > m_moveOverLimit.y){
            return true;
        }
        else{
            return false;
        }
    }

    // 指定されたエリア内に入っているかどうかを判断する
    public static bool IsInnerArea(Vector2 position1, Vector2 position2){
        Vector2 area = position1 + m_AreaLimit;
        if(position2.x < area.x || position2.x > -area.x || position2.y < area.y || position2.y < -area.y){
            return true;
        }
        else{
            return false;
        }
    }
}
