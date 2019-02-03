/////////////////////////////////////////////////////////////////////////////////
//
//	vp_FPInputMobile.cs
//	© VisionPunk. All Rights Reserved.
//	https://twitter.com/VisionPunk
//	http://www.visionpunk.com
//
//	description:	This extends vp_FPInput in order to override the GetButton
//					methods and allow support for touch controls for vp_UI system
//
/////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections.Generic;

public class vp_FPInputMobile : vp_FPInput
{

	
	/// <summary>
	/// 
	/// </summary>
	protected virtual void Start()
	{
		
		m_FPCamera.SetRotation(m_FPCamera.Transform.eulerAngles, false, true);
		Player.Zoom.MinPause = .25f;
		Player.Zoom.MinDuration = .25f;
	
	}
	
	/// <summary>
	/// Handles interaction with the game world
	/// </summary>
	protected override void InputInteract()
	{
	
		if(!Player.CanInteract.Get())
			return;
			
		bool interact = vp_GlobalEventReturn<bool>.Send("SimulateTouchWithMouse") ? vp_Input.GetButtonAny("Interact") : vp_Input.GetButtonUp("Interact");
		
		if(interact)
			Player.Interact.TryStart();
		else
			Player.Interact.TryStop();
	
	}
	
	
	/// <summary>
	/// broadcasts a message to any listening components telling
	/// them to go into 'attack' mode. vp_FPShooter uses this to
	/// repeatedly fire the current weapon while the fire button
	/// is being pressed, but it could also be used by, for example,
	/// an animation script to make the player model loop an 'attack
	/// stance' animation.
	/// </summary>
	protected override void InputAttack()
	{

		// TIP: uncomment this to prevent player from attacking while running
		//if (Player.Run.Active)
		//	return;

		// if mouse cursor is visible, an extra click is needed
		// before we can attack
		
		if(Player.Reload.Active)
			return;
		
		if (vp_Input.GetButtonAny("Attack"))
		{
		    if (Player.CurrentWeaponAmmoCount.Get() == 0 && Player.CurrentWeaponClipType.Get() != "")
		    {
                if(!Player.Reload.TryStart())
                    Player.Attack.TryStart();
            }
            else
				Player.Attack.TryStart();
		}
        else
			Player.Attack.TryStop();
			
	}
	
	
	/// <summary>
	/// ask controller to jump when button is pressed (the current
	/// controller preset determines jump force).
	/// NOTE: if its 'MotorJumpForceHold' is non-zero, this
	/// also makes the controller accumulate jump force until
	/// button release.
	/// </summary>
	protected override void InputJump()
	{

		// TIP: to find out what determines if 'Jump.TryStart'
		// succeeds and where it is hooked up, search the project
		// for 'CanStart_Jump'

		if (vp_Input.GetButtonAny("Jump"))
			Player.Jump.TryStart();
		else
			Player.Jump.Stop();

	}
	
	
	/// <summary>
	/// when the reload button is pressed, broadcasts a message
	/// to any listening components asking them to reload
	/// NOTE: reload may not succeed due to ammo status etc.
	/// </summary>
	protected override void InputReload()
	{

		if (vp_Input.GetButtonAny("Reload"))
			Player.Reload.TryStart();
		
	}
	
	
	/// <summary>
	/// disallow zoom if these conditions are met
	/// </summary>
	protected virtual bool CanStart_Zoom()
	{
	
		if(!Player.CurrentWeaponWielded.Get())
			return false;
			
		if(Player.Reload.Active)
			return false;
		
		if(Player.CurrentWeaponClipType.Get() == "")
			return false;
			
		return true;
	
	}
	
	
	/// <summary>
	/// stop zooming if reload starts
	/// </summary>
	protected virtual void OnStart_Reload()
	{
	
		Player.Zoom.Stop();
	
	}
	
	
	/// <summary>
	/// zoom in using the zoom modifier key(s)
	/// </summary>
	protected override void InputZoom()
	{
	
		bool zoom = vp_GlobalEventReturn<bool>.Send("SimulateTouchWithMouse") ? vp_Input.GetButtonAny("Zoom") : vp_Input.GetButtonDown("Zoom");
		
		if(!zoom)
			return;
			
		if(Time.time < Player.Zoom.NextAllowedStartTime)
			return;
		
		if(Player.Zoom.Active)
			Player.Zoom.TryStop();
		else
			Player.Zoom.TryStart();

	}
	
	
	/// <summary>
	/// tell the player to enable or disable the 'Run' state.
	/// NOTE: since running is a state, it's not sent to the
	/// controller code (which doesn't know the state names).
	/// instead, the player class is responsible for feeding the
	/// 'Run' state to every affected component.
	/// </summary>
	protected override void InputRun()
	{

		if (vp_Input.GetButtonAny("Run"))
		{
			if(vp_GlobalEventReturn<bool>.Send("SimulateTouchWithMouse"))
				Player.InputMoveVector.Set(new Vector2(0, 1));
			else
				Player.InputMoveVector.Set(new Vector2(vp_Input.GetAxisRaw("Horizontal"), vp_Input.GetAxisRaw("Vertical")));
			Player.Run.TryStart();
		}
		else Player.Run.TryStop();

	}
	
	
	/// <summary>
	/// toggles the game's pause state on / off
	/// </summary>
	protected override void UpdatePause()
	{

		if(vp_Input.GetButtonAny("Pause"))
			Player.Pause.Set(!Player.Pause.Get());

	}
	
	
	/// <summary>
	/// this method handles toggling between mouse pointer and
	/// firing modes. it can be used to deal with screen regions
	/// for button menus, inventory panels et cetera.
	/// NOTE: if your game supports multiple screen resolutions,
	/// make sure your 'MouseCursorZones' are always adapted to
	/// the current resolution. see 'vp_FPSDemo1.Start' for one
	/// example of how to this
	/// </summary>
	protected override void UpdateCursorLock()
	{
	
		if(vp_GlobalEventReturn<bool>.Send("SimulateTouchWithMouse"))
			return;

		// store the current mouse position as GUI coordinates
		m_MousePos.x = Input.mousePosition.x;
		m_MousePos.y = (Screen.height - Input.mousePosition.y);

		// uncomment this line to print the current mouse position
		//Debug.Log("X: " + (int)m_MousePos.x + ", Y:" + (int)m_MousePos.y);

		// if 'ForceCursor' is active, the cursor will always be visible
		// across the whole screen and firing will be disabled
		if (ForceCursor)
		{
			Screen.lockCursor = false;
			return;
		}

		// see if any of the mouse buttons are being held down
		if (Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButton(2))
		{

			// if we have defined mouse cursor zones, check to see if the
			// mouse cursor is inside any of them
			if (MouseCursorZones.Length > 0)
			{
				foreach (Rect r in MouseCursorZones)
				{
					if (r.Contains(m_MousePos))
					{
						// mouse is being held down inside a mouse cursor zone, so make
						// sure the cursor is not locked and don't lock it this frame
						Screen.lockCursor = false;
						goto DontLock;
					}
				}
			}

			// no zones prevent firing the current weapon. hide mouse cursor
			// and lock it at the center of the screen
			Screen.lockCursor = true;

		}

	DontLock:

		// if user presses 'ENTER', toggle mouse cursor on / off
		if (vp_Input.GetButtonUp("Accept1") || vp_Input.GetButtonUp("Accept2"))
			Screen.lockCursor = !Screen.lockCursor;

	}
	
}

