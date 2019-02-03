/////////////////////////////////////////////////////////////////////////////////
//
//	vp_UITouchWeaponSwitcher.cs
//	© VisionPunk. All Rights Reserved.
//	https://twitter.com/VisionPunk
//	http://www.visionpunk.com
//
//	description:	This class handles all the touch events and weapon switching
//					for the players weapons.
//
/////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(BoxCollider))]
public class vp_UITouchWeaponSwitcher : vp_UIControl
{

	public Transform WeaponScroller = null;						// GameObject that will scroll, should contain the item game objects.
	public bool ChangeOnRelease = false;						// Should the weapon change when touch released or only when threshold reached
	public float ChangeWeaponThreshold = 100f;					// Movement from center point where the weapon will be changed
	public float ItemWidth = 135f;								// Movement from center point where the weapon will be changed
	public float Angle = 13f;									// angle the scroller will move
	public float ItemYOffset = 11f;								// offset each item will be set to from the previous item
	public float WieldTouchDelay = .15f;						// delay after a touch and then release in which equipping and unequipping triggers
	
	protected Vector3 m_CachedScrollerPosition = Vector3.zero;	// cached scroller default position
	protected Vector3 m_NewPosition = Vector3.zero;				// position to move scroller to next
	protected vp_SimpleInventory m_Inventory = null;			// cached simple inventory
	protected Vector3 m_TouchDownPosition = Vector3.zero;		// cached position of first touch
	protected float m_LastMoveTime = 0;							// time of last movement
	protected Dictionary<string, WeaponScrollerItem> m_Weapons = new Dictionary<string, WeaponScrollerItem>();			// dictionary of weapons
	protected Dictionary<string, WeaponScrollerItem> m_EquippedWeapons = new Dictionary<string, WeaponScrollerItem>();	// dictionary of available weapons
	protected WeaponScrollerItem m_CurrentItem = null;			// cached current item
	protected float m_TouchTime = 0;
	protected int m_LastWeaponID = 0;
	
	// returns the current items center point
	protected Vector3 m_CurrentItemDefaultPosition
	{
		get{
			return new Vector3( m_CachedScrollerPosition.x - m_CurrentItem.Transform.localPosition.x, m_CachedScrollerPosition.y - m_CurrentItem.Transform.localPosition.y, m_CachedScrollerPosition.z );
		}
	}
	
	
	/// <summary>
	/// class to hold information about the current item
	/// </summary>
	public class WeaponScrollerItem
	{
	
		public string Name = "";
		public int Index = 0;
		public int WeaponId = -1;
		public Transform Transform = null;
	
	}
	
	
	/// <summary>
	/// 
	/// </summary>
	protected override void Awake()
	{
	
		base.Awake();
	
		m_Inventory = Manager.Player.GetComponent<vp_SimpleInventory>();
	
	}
	
	
	/// <summary>
	/// 
	/// </summary>
	protected override void Start()
	{
	
		base.Start();
	
		m_CachedScrollerPosition = WeaponScroller.localPosition;
		m_NewPosition = m_CachedScrollerPosition;
		
		foreach(vp_SimpleInventory.InventoryItemStatus weapon in m_Inventory.Weapons)
		{
			Transform t = WeaponScroller.Find(weapon.Name);
			if(t == null)
				continue;
			
			m_Weapons.Add(weapon.Name, new WeaponScrollerItem{ Name = weapon.Name, Transform = t, WeaponId = m_Inventory.Weapons.IndexOf(weapon) });
			if(weapon.Have == 0)
				vp_Utility.Activate(t.gameObject, false);
		}
		
		OnEnable();
	
	}
	
	
	/// <summary>
	/// Updates the scroller items.
	/// </summary>
	protected virtual void UpdateScrollerItems()
	{
	
		float xOffset = 0;
		float yOffset = 0;
		int i = 0;
		foreach(vp_SimpleInventory.InventoryItemStatus weapon in m_Inventory.Weapons)
		{
			
			if(weapon.Have == 1)
			{
				m_EquippedWeapons[weapon.Name].Index = i;
				m_EquippedWeapons[weapon.Name].Transform.localPosition = new Vector3(xOffset, yOffset, m_EquippedWeapons[weapon.Name].Transform.localPosition.z);
				xOffset += (m_Weapons[weapon.Name].Transform.localScale.x*2)+(ItemWidth*.5f);
				yOffset += ItemYOffset;
				i++;
			}
				
		}
	
	}
	
	
	/// <summary>
	/// registers this component with the event handler (if any)
	/// </summary>
	protected virtual void OnEnable()
	{
	
		if(!m_Initialized)
			return;

		base.OnEnable();

	}
	
	
	/// <summary>
	/// reset scroller if player dies
	/// </summary>
	protected virtual void OnStart_Dead()
	{
	
		m_CurrentItem = null;
		m_EquippedWeapons.Clear();
		m_Inventory.CurrentWeaponStatus = null;
	
		foreach(WeaponScrollerItem weapon in m_Weapons.Values)
			vp_Utility.Activate( weapon.Transform.gameObject, false );
	
	}
	
	
	/// <summary>
	/// update the scroller with the current weapon
	/// </summary>
	protected virtual void OnStop_SetWeapon()
	{
	
		if(Manager.Player.Dead.Active)
			return;
			
		int id = (int)Manager.Player.SetWeapon.Argument;
		if(id != 0)
			m_LastWeaponID = id;
			
		WeaponScrollerItem item;
		m_Weapons.TryGetValue(Manager.Player.CurrentWeaponName.Get(), out item);
		if(item != null)
		{
			if(!m_EquippedWeapons.ContainsKey(item.Name))
			{
				m_EquippedWeapons.Add(item.Name, item);
				UpdateScrollerItems();
			}
			m_CurrentItem = m_EquippedWeapons[item.Name];
			vp_Utility.Activate(item.Transform.gameObject , true);
			CurrentItemPosition();
		}
	
	}
	
	
	/// <summary>
	/// handle touch event when touch begins
	/// </summary>
	protected override void TouchesBegan( vp_Touch touch )
	{
	
		if(LastFingerID != -1)
    		return;
    		
    	if(WeaponScroller == null)
    		return;
    		
    	if(Physics.RaycastAll(m_Camera.ScreenPointToRay(touch.Position)).Where(hit => hit.collider == m_Collider).ToList().Count == 0)
    		return;
	
		m_TouchDownPosition = m_Camera.ScreenToWorldPoint( touch.Position );
		LastFingerID = touch.FingerID;
		m_TouchTime = Time.time + WieldTouchDelay;
	
	}
	
	
	/// <summary>
	/// handles touch events when the touch is moved
	/// </summary>
	protected virtual void TouchesMoved( vp_Touch touch )
    {
    
    	if ( LastFingerID != touch.FingerID )
    		return;
    		
    	if(Manager.Player.CurrentWeaponID.Get() == 0)
    		return;
    		
    	Vector3 touchPosition = m_Camera.ScreenToWorldPoint( touch.Position ) - m_TouchDownPosition;
		m_NewPosition = new Vector3(m_CachedScrollerPosition.x - m_CurrentItem.Transform.localPosition.x + (((m_CurrentItem.Transform.localScale.x*2)+(ItemWidth*.5f) * m_EquippedWeapons.Count) * touchPosition.x), m_CachedScrollerPosition.y - m_CurrentItem.Transform.localPosition.y + (touchPosition.x * Angle), m_CachedScrollerPosition.z);
		
		if((m_NewPosition-m_CurrentItemDefaultPosition).magnitude > ChangeWeaponThreshold)
			SetWeapon(touch);
			
		m_LastMoveTime = Time.time + .5f;
    
    }
	
	
	/// <summary>
	/// handle touch event when this component
	/// is no longer being touched
	/// </summary>
	public override void TouchesFinished( vp_Touch touch )
	{
	
		if ( LastFingerID != touch.FingerID )
    		return;
    		
    	base.TouchesFinished(touch);
    		
    	if(Manager.Player.CurrentWeaponID.Get() > 0)
		{
       		if(ChangeOnRelease)
	    		SetWeapon(touch);
	    	else
	    		CurrentItemPosition();
	    }
	    	
	    if(Time.time < m_TouchTime)
    		Manager.Player.SetWeapon.TryStart(Manager.Player.CurrentWeaponID.Get() == 0 ? m_LastWeaponID : 0);
	
	}
	
	
	/// <summary>
	/// sets the next position to be the current
	/// items default position
	/// </summary>
	protected virtual void CurrentItemPosition()
	{
	
		m_NewPosition = m_CurrentItemDefaultPosition;
		m_LastMoveTime = Time.time + .5f;
	
	}
	
	
	/// <summary>
	/// Finds position of touch in relation
	/// to where the touch started and decides to
	/// switch to the next or previous weapon if
	/// applicable
	/// </summary>
	protected virtual void SetWeapon( vp_Touch touch )
	{
		LastFingerID = -1;
	
		if(m_EquippedWeapons.Count > 1)
    	{
	    	if(m_TouchDownPosition.x > m_Camera.ScreenToWorldPoint( touch.Position ).x) // next weapon
	    	{
	    		if(m_CurrentItem.Index <= m_EquippedWeapons.Count - 2)
	    			Manager.Player.SetNextWeapon.Try();
	    		else
	    			CurrentItemPosition();
	    	}
	    	else
	    	{
	    		if(m_CurrentItem.Index > 0)
	    			Manager.Player.SetPrevWeapon.Try();
	    		else
	    			CurrentItemPosition();
	    	}
	    }
	    else
	    {
	    	CurrentItemPosition();
	    }	
	
	}
	
	
	/// <summary>
	/// 
	/// </summary>
	protected virtual void Update()
	{
	
		if(WeaponScroller == null)
    		return;
	
		if(Time.time > m_LastMoveTime)
			return;
	
		WeaponScroller.localPosition = Vector3.Lerp( WeaponScroller.localPosition, m_NewPosition, Time.deltaTime * 20 );
	
	}
	
}
