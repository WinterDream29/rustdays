/////////////////////////////////////////////////////////////////////////////////
//
//	vp_SimpleHUDMobile.cs
//	В© VisionPunk. All Rights Reserved.
//	https://twitter.com/VisionPunk
//	http://www.visionpunk.com
//
//	description:	a very primitive HUD displaying health, clips and ammo, along
//					with a soft red full screen flash for when taking damage
//
/////////////////////////////////////////////////////////////////////////////////

using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class vp_SimpleHUDMobile : vp_SimpleHUD
{

	public float FadeDuration = 3f;				// duration of the fade for HUD Messages
	public bool ShowTips = true;				// enable or disable the display of HUD Messages
	public GameObject AmmoLabel = null;			// a gameobject that has a TextMesh component for ammo label
	public GameObject HealthLabel = null;		// a gameobject that has a TextMesh component for Health label
	public GameObject HintsLabel = null;		// a gameobject that has a TextMesh component for Hints label
	
	private TextMesh m_AmmoLabel = null;		// cached TextMesh component for ammo label
	private TextMesh m_HealthLabel = null;		// cached TextMesh component for ammo label
	private TextMesh m_HintsLabel = null;		// cached TextMesh component for ammo label
	protected vp_UITween.Handle m_HUDTextTweenHandle = new vp_UITween.Handle();
	protected vp_SimpleInventory m_Inventory = null;	// a cached reference to the simple inventory script
	protected Color m_MessageColorMobile = new Color(2, 2, 0, 2);
	protected Color m_InvisibleColorMobile = new Color(1, 1, 0, 0);
	protected Color m_DamageFlashColorMobile = new Color(0.8f, 0, 0, 0);
	protected Color m_DamageFlashInvisibleColorMobile = new Color(1, 0, 0, 0);
	protected string m_PickupMessageMobile = "";
	protected static GUIStyle m_MessageStyleMobile = null;
	public static GUIStyle MessageStyleMobile
	{
		get
		{
			if (m_MessageStyleMobile == null)
			{
				m_MessageStyleMobile = new GUIStyle("Label");
				m_MessageStyleMobile.alignment = TextAnchor.MiddleCenter;
			}
			return m_MessageStyleMobile;
		}
	}

	protected vp_FPPlayerEventHandler m_PlayerEventHandler = null;
	protected int m_Health{
		get{
			int health = (int)(m_PlayerEventHandler.Health.Get() * 100.0f);
			return health < 0 ? 0 : health;
		}
	}

	/// <summary>
	///
	/// </summary>
	protected virtual void Awake()
	{
		
		m_PlayerEventHandler = transform.root.GetComponentInChildren<vp_FPPlayerEventHandler>();
		m_Inventory = transform.root.GetComponentInChildren<vp_SimpleInventory>();
		
		if(AmmoLabel != null)	m_AmmoLabel = AmmoLabel.GetComponentInChildren<TextMesh>();
		if(HealthLabel != null)	m_HealthLabel = HealthLabel.GetComponentInChildren<TextMesh>();
		if(HintsLabel != null)	m_HintsLabel = HintsLabel.GetComponentInChildren<TextMesh>();
		
		if(m_HintsLabel != null)
		{
			m_HintsLabel.text = "";
			m_HintsLabel.GetComponent<Renderer>().material.color = Color.clear;
		}
		
	}


	/// <summary>
	/// registers this component with the event handler (if any)
	/// </summary>
	protected override void OnEnable()
	{

		if (m_PlayerEventHandler != null)
			m_PlayerEventHandler.Register(this);

	}


	/// <summary>
	/// unregisters this component from the event handler (if any)
	/// </summary>
	protected override void OnDisable()
	{


		if (m_PlayerEventHandler != null)
			m_PlayerEventHandler.Unregister(this);

	}


	/// <summary>
	/// this draws a primitive HUD and also renders the current
	/// message, fading out in the middle of the screen
	/// </summary>
	protected override void OnGUI()
	{
	
		// show a red glow along the screen edges when damaged
		if (DamageFlashTexture != null && m_DamageFlashColorMobile.a > 0.01f)
		{
			m_DamageFlashColorMobile = Color.Lerp(m_DamageFlashColorMobile, m_DamageFlashInvisibleColorMobile, Time.deltaTime * 0.4f);
			GUI.color = m_DamageFlashColorMobile;
			GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), DamageFlashTexture);
			GUI.color = Color.white;
		}
	
	}
	
	
	/// <summary>
	/// 
	/// </summary>
	protected virtual void Update()
	{
	
		int maxAmmmo = 0;
		if(m_Inventory.CurrentWeaponStatus != null)
			maxAmmmo = m_Inventory.CurrentWeaponStatus.MaxAmmo;
			
		if(m_AmmoLabel != null)
			m_AmmoLabel.text = m_PlayerEventHandler.CurrentWeaponAmmoCount.Get() + "/" + (maxAmmmo * (m_PlayerEventHandler.CurrentWeaponClipCount.Get() + 1)).ToString();
			
		if(m_HealthLabel != null)
			m_HealthLabel.text = m_Health + "%";
	
	}
	
	
	/// <summary>
	/// updates the HUD message text and makes it fully visible
	/// then fades it out
	/// </summary>
	protected override void OnMessage_HUDText(string message)
	{

		if(!ShowTips || m_HintsLabel == null)
			return;

		m_PickupMessageMobile = (string)message;
		m_HintsLabel.text = m_PickupMessageMobile;
		vp_UITween.ColorTo(HintsLabel, Color.white, .25f, m_HUDTextTweenHandle, delegate {
			vp_UITween.ColorTo(HintsLabel, m_InvisibleColorMobile, FadeDuration, m_HUDTextTweenHandle);
		});

	}
	
	
	/// <summary>
	/// shows or hides the HUD full screen flash 
	/// </summary>
	protected virtual void OnMessage_HUDDamageFlash(float intensity)
	{

		if (DamageFlashTexture == null)
			return;

		if (intensity == 0.0f)
			m_DamageFlashColorMobile.a = 0.0f;
		else
			m_DamageFlashColorMobile.a += intensity;

	}


}

