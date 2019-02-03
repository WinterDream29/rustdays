using Assets.Scripts;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class InWaterEffect : MonoBehaviour
{
    public GameManager GameManager;

    public BlurOptimized blurScript;
    public ColorCorrectionCurves ColorScript;

    private LayerMask playerLayer = 1 << vp_Layer.LocalPlayer;
    private vp_FPCamera m_camera = null;

    private float _soundDelay;

    void OnTriggerEnter(Collider col)
    {
        if ((playerLayer.value & 1 << col.gameObject.layer) == 0)
            return;
        m_camera = col.gameObject.GetComponentInChildren<vp_FPCamera>();
        TurnOn();
    }
    void OnTriggerExit(Collider col)
    {
        if ((playerLayer.value & 1 << col.gameObject.layer) == 0)
            return;
        m_camera = col.gameObject.GetComponentInChildren<vp_FPCamera>();
        TurnOff();
    }

    void TurnOff()
    {
        blurScript = m_camera.GetComponentInChildren<BlurOptimized>();
        //blurScript.enabled = false;
        //ColorScript = m_camera.GetComponentInChildren<ColorCorrectionCurves>();
        //ColorScript.enabled = false;

        GameManager.PlayerModel.SetUnderWater(false);
    }

    private void TurnOn()
    {
        blurScript = m_camera.GetComponentInChildren<BlurOptimized>();
        //blurScript.enabled = true;
        //ColorScript = m_camera.GetComponentInChildren<ColorCorrectionCurves>();
        //ColorScript.enabled = true;

        GameManager.PlayerModel.SetUnderWater(true);
        if (_soundDelay <= 0.0f)
        {
            _soundDelay = 1.0f;
            //SoundManager.PlaySFX("breathing_underwater");
        }
    }

    void Update()
    {
        if(_soundDelay > 0.0f)
            _soundDelay -= Time.deltaTime;
    }
}
