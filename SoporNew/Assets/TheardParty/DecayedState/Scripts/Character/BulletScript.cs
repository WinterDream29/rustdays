using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {
	public float shootForce = 400f;
	public float BulletLife = 3;
	private float _currentLife = 0;
	
	void OnEnable(){
		//Invoke ("Destroy", 3f);
		this.GetComponent<Rigidbody>().AddForce(transform.forward * shootForce);
	}
	void Destroy(){
		Destroy(gameObject);
	}
	void FixedUpdate(){
		_currentLife += Time.deltaTime;
		if (_currentLife > BulletLife) Destroy(gameObject);
	}
	void OnCollisionEnter(Collision col){
		Destroy(gameObject);
	}
}
