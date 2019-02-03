using UnityEngine;
using System.Collections;

public class CrossHair : MonoBehaviour {
	private GameObject Player;
	private GameObject ShotGun;
	public GameObject projectile;
	public AudioClip shotGunShot;
	private CharacterControl ptrCharacterControl;
	public GameObject crossHairTexture;
	public bool crossHair =true;
	public GameObject bullet;
	public GameObject gunTip;
	private Rect crossHairPosition;
	public Camera GUICam;
	public float cursorZMAx;
	public float cursorZMin;

	private float shootTime = 0f;

	void Start (){
		ShotGun = GameObject.Find("remmington");
		Player = GameObject.FindGameObjectWithTag("Player");
		ptrCharacterControl = Player.GetComponent<CharacterControl> ();
	}

	// Update is called once per frame
	void Update () {
		crossHairTexture.transform.LookAt(GUICam.transform);
		Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width/2, Screen.height/2));
		RaycastHit hit;
		Debug.DrawLine (gunTip.transform.position, crossHairTexture.transform.position, Color.green);
		if (ptrCharacterControl.usingRifle) {
						crossHairTexture.GetComponent<Renderer>().enabled = true;		
				} else {
						crossHairTexture.GetComponent<Renderer>().enabled = false;	
				}

		if (Physics.Raycast (ray, out hit, 100)) {
			Debug.DrawLine (ray.origin, hit.point, Color.black);
			crossHairTexture.transform.position = hit.point;
			gunTip.transform.LookAt (hit.point);
			}
		else{
			crossHairTexture.transform.position = ray.origin + (ray.direction * 20);
			gunTip.transform.LookAt(ray.origin + (ray.direction * 20));
		}
		if (ptrCharacterControl.currentWeapon.magazine > 0 && ptrCharacterControl.rifleAiming) {
			if (Input.GetMouseButton (0)) {
				if (shootTime <= Time.time) {
					shootTime = Time.time + ptrCharacterControl.currentWeapon.fireRate;
					Instantiate(projectile, ptrCharacterControl.currentWeapon.weaponMuzzlePoint.position, ptrCharacterControl.currentWeapon.weaponMuzzlePoint.rotation);

					Player.GetComponent<Animator>().SetTrigger("SGSReload");// lunch shotgun shoot reload animation
					ShotGun.GetComponent<Animator>().SetTrigger("reload");//lunch shotgun reload animation
					ShotGunShot();
					ptrCharacterControl.currentWeapon.magazine -= 1;
					if (ptrCharacterControl.currentWeapon.magazine < 0) {
						ptrCharacterControl.currentWeapon.magazine = 0;
					}
					if (ptrCharacterControl.currentWeapon.magazine == 0) {
						ptrCharacterControl._animator.SetBool ("ReloadPistol", true);	
					}
				}				
			}
		}
	}
	private void ShotGunShot(){
		GameObject go = new GameObject("Audio");
		go.transform.position = ptrCharacterControl.currentWeapon.weaponMuzzlePoint.position;
		go.transform.parent = ptrCharacterControl.currentWeapon.weaponMuzzlePoint;
		//Create the source
		AudioSource source = go.AddComponent<AudioSource>();
		source.clip = shotGunShot;
		source.Play();
		Destroy(go, source.clip.length);
	}
}
