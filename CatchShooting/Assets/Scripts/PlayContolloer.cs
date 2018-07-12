using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayContolloer : MonoBehaviour {

    private float m_time = 0;
    private int m_MaxTime = 60;

    public float _Time{
        get{
            return m_time;
        }
        set{
            m_time = value;
        }
    }

    public int MaxTime{
        get{
            return m_MaxTime;
        }
        set{
            m_MaxTime = value;
        }
    }
    void Start () {
        m_time = m_MaxTime;
	}
	
	void Update () {
        m_time = m_MaxTime - Time.time;

        if(m_time <= 0){
            var scene = FindObjectOfType<GameSystem>();
            scene.GameResult();
        }
	}
}
