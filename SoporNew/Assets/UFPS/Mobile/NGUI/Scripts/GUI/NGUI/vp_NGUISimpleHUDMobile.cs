/////////////////////////////////////////////////////////////////////////////////
//
//	vp_NGUISimpleHUDMobile.cs
//	© VisionPunk. All Rights Reserved.
//	https://twitter.com/VisionPunk
//	http://www.visionpunk.com
//
//	description:	a very primitive HUD displaying health, clips and ammo, along
//					with a soft red full screen flash for when taking damage
//
/////////////////////////////////////////////////////////////////////////////////

using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class vp_NGUISimpleHUDMobile : vp_SimpleHUDMobile
{

	protected UILabel m_AmmoLabelSprite = null;
	protected UILabel m_HealthLabelSprite = null;
	protected UILabel m_HintsLabelSprite = null;

	/// <summary>
	///
	/// </summary>
	protected override void Awake()
	{
	
		base.Awake();
		
		if(AmmoLabel != null)	m_AmmoLabelSprite = AmmoLabel.GetComponentInChildren<UILabel>();
		if(HealthLabel != null)	m_HealthLabelSprite = HealthLabel.GetComponentInChildren<UILabel>();
		if(HintsLabel != null)	m_HintsLabelSprite = HintsLabel.GetComponentInChildren<UILabel>();
		
		if(m_HintsLabelSprite != null)
		{
			m_HintsLabelSprite.text = "";
			m_HintsLabelSprite.color = Color.clear;
		}
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
	protected override void Update()
	{
	
		int maxAmmmo = 0;
		if(m_Inventory.CurrentWeaponStatus != null)
			maxAmmmo = m_Inventory.CurrentWeaponStatus.MaxAmmo;
			
		if(m_AmmoLabelSprite != null)
			m_AmmoLabelSprite.text = m_PlayerEventHandler.CurrentWeaponAmmoCount.Get() + " / " + (maxAmmmo * (m_PlayerEventHandler.CurrentWeaponClipCount.Get())).ToString();
			
		if(m_HealthLabelSprite != null)
			m_HealthLabelSprite.text = m_Health + "%";
	
	}
	
	
	/// <summary>
	/// updates the HUD message text and makes it fully visible
	/// </summary>
	protected override void OnMessage_HUDText(string message)
	{
	
		if(!ShowTips || m_HintsLabelSprite == null)
			return;

		m_PickupMessageMobile = (string)message;
		m_HintsLabelSprite.text = m_PickupMessageMobile;
		vp_NGUITween.ColorTo(m_HintsLabelSprite, Color.white, .25f, m_HUDTextTweenHandle, delegate {
			vp_NGUITween.ColorTo(m_HintsLabelSprite, m_InvisibleColorMobile, FadeDuration, m_HUDTextTweenHandle);
		});

	}


}

