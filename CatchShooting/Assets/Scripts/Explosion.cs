using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

	void Start () {
        // 演出が完了したら削除する
        var particleSystem = GetComponent<ParticleSystem>();
        Destroy(gameObject, particleSystem.main.duration);
    }
	

}
