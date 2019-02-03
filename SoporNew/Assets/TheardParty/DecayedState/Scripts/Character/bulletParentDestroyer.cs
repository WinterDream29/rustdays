using UnityEngine;
using System.Collections;

public class bulletParentDestroyer : MonoBehaviour {
	public float BulletLife = 3;
	private float _currentLife = 0;

	
	void FixedUpdate(){
		_currentLife += Time.deltaTime;
		if (_currentLife > BulletLife) Destroy(gameObject);
	}
}
