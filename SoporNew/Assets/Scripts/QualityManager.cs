using System;
using Assets.Scripts.Models.Events;
using UnityEngine;
using UnityStandardAssets.Water;

namespace Assets.Scripts
{
    public enum QualityType
    {
        Low = 0,
        Medium = 1,
        Hight = 2,
        Ultra = 3
    }
    public static class QualityManager
    {
        public static QualityType CurrentQuality;
        public static WaterQuality CurrentWaterQuality;

        public static Action<QualityType> OnChangeQuality;

        public static void SetQuality(GameManager gameManager, int value)
        {
            CurrentQuality = (QualityType)value;
            QualitySettings.SetQualityLevel(value);
            var rays = gameManager.Player.FpsCamera.GetComponent<TOD_Rays>();
            var scattering = gameManager.Player.FpsCamera.GetComponent<TOD_Scattering>();

            gameManager.World.Grass.SetActive(true);

            switch (CurrentQuality)
            {
                case QualityType.Low:
                    gameManager.World.Sky.CloudQuality = TOD_CloudQualityType.Low;
                    gameManager.World.Sky.MeshQuality = TOD_MeshQualityType.Low;
                    gameManager.World.Sky.StarQuality = TOD_StarQualityType.Low;
                    if (rays != null)
                        rays.enabled = false;
                    if (scattering != null)
                        scattering.enabled = false;
                    //gameManager.World.WaterController.waterQuality = WaterQuality.Low;
                    gameManager.Terrain1.basemapDistance = 20;
                    //gameManager.Terrain1.detailObjectDensity = 0.0f;
                    gameManager.Terrain2.basemapDistance = 20;
                    //gameManager.Terrain2.detailObjectDensity = 0.0f;
                    gameManager.World.Grass.SetActive(false);
                    break;
                case QualityType.Medium:
                    gameManager.World.Sky.CloudQuality = TOD_CloudQualityType.Medium;
                    gameManager.World.Sky.MeshQuality = TOD_MeshQualityType.Low;
                    gameManager.World.Sky.StarQuality = TOD_StarQualityType.Low;
                    //gameManager.World.WaterController.waterQuality = WaterQuality.Medium;
                    gameManager.Terrain1.basemapDistance = 40;
                    //gameManager.Terrain1.detailObjectDensity = 0.0f;
                    gameManager.Terrain1.detailObjectDistance = 30.0f;
                    gameManager.Terrain2.basemapDistance = 40;
                    //gameManager.Terrain2.detailObjectDensity = 0.0f;
                    gameManager.Terrain2.detailObjectDistance = 30.0f;
                    if (rays != null)
                        rays.enabled = false;
                    if (scattering != null)
                        scattering.enabled = false;
                    break;
                case QualityType.Hight:
                    gameManager.World.Sky.CloudQuality = TOD_CloudQualityType.High;
                    gameManager.World.Sky.MeshQuality = TOD_MeshQualityType.Medium;
                    gameManager.World.Sky.StarQuality = TOD_StarQualityType.Low;
                    //gameManager.World.WaterController.waterQuality = WaterQuality.High;
                    gameManager.Terrain1.basemapDistance = 50;
                    //gameManager.Terrain1.detailObjectDensity = 0.9f;
                    gameManager.Terrain1.detailObjectDistance = 40.0f;
                    gameManager.Terrain2.basemapDistance = 50;
                    //gameManager.Terrain2.detailObjectDensity = 0.9f;
                    gameManager.Terrain2.detailObjectDistance = 40.0f;
                    if (rays != null)
                        rays.enabled = false;
                    if (scattering != null)
                        scattering.enabled = false;
                    break;
                case QualityType.Ultra:
                    gameManager.World.Sky.CloudQuality = TOD_CloudQualityType.High;
                    gameManager.World.Sky.MeshQuality = TOD_MeshQualityType.Medium;
                    gameManager.World.Sky.StarQuality = TOD_StarQualityType.Low;
                    //gameManager.World.WaterController.waterQuality = WaterQuality.High;
                    gameManager.Terrain1.basemapDistance = 50;
                    //gameManager.Terrain1.detailObjectDensity = 1.0f;
                    gameManager.Terrain1.detailObjectDistance = 50.0f;
                    gameManager.Terrain2.basemapDistance = 50;
                    //gameManager.Terrain2.detailObjectDensity = 1.0f;
                    gameManager.Terrain2.detailObjectDistance = 50.0f;
                    if (rays != null)
                        rays.enabled = true;
                    if (scattering != null)
                        scattering.enabled = false;
                    break;
            }

            PlayerPrefs.SetInt("QualityLevel", value);

            if (OnChangeQuality != null)
                OnChangeQuality(CurrentQuality);
        }

        public static void SetQuality(GameManager gameManager)
        {
            var videoQualityValue = 1;

            if (PlayerPrefs.HasKey("QualityLevel"))
            {
                videoQualityValue = PlayerPrefs.GetInt("QualityLevel");
            }
            else
            {
#if UNITY_ANDROID
            if (SystemInfo.systemMemorySize < 1100 || !SystemInfo.supportsShadows)
            {
                videoQualityValue = 0;
            }
            else if (SystemInfo.systemMemorySize < 2500)
            {
                videoQualityValue = 1;
            }
            else if (SystemInfo.systemMemorySize > 3900)
            {
                videoQualityValue = 2;
            }
            else
            {
                videoQualityValue = 1;
            }
#endif
#if UNITY_IOS
               // Debug.Log("UnityEngine.iOS.Device.generation " + UnityEngine.iOS.Device.generation);
                switch (UnityEngine.iOS.Device.generation)
                {
                    case UnityEngine.iOS.DeviceGeneration.iPadAir2:
                    case UnityEngine.iOS.DeviceGeneration.iPadMini4Gen:
                    case UnityEngine.iOS.DeviceGeneration.iPadPro10Inch1Gen:
                    case UnityEngine.iOS.DeviceGeneration.iPadPro1Gen:
                    case UnityEngine.iOS.DeviceGeneration.iPhone6S:
                    case UnityEngine.iOS.DeviceGeneration.iPhone6SPlus:
                    case UnityEngine.iOS.DeviceGeneration.iPhone7:
                    case UnityEngine.iOS.DeviceGeneration.iPhone7Plus:
                        videoQualityValue = 3;
                        break;

                    case UnityEngine.iOS.DeviceGeneration.iPad1Gen:
                    case UnityEngine.iOS.DeviceGeneration.iPadMini1Gen:
                    case UnityEngine.iOS.DeviceGeneration.iPhone3G:
                    case UnityEngine.iOS.DeviceGeneration.iPhone3GS:
                    case UnityEngine.iOS.DeviceGeneration.iPhone4:
                    case UnityEngine.iOS.DeviceGeneration.iPhone4S:
                    case UnityEngine.iOS.DeviceGeneration.iPhone5:
                        videoQualityValue = 0;
                        break;
                    case UnityEngine.iOS.DeviceGeneration.iPhone5S:
                    case UnityEngine.iOS.DeviceGeneration.iPhone6:
                    case UnityEngine.iOS.DeviceGeneration.iPhone6Plus:
                    case UnityEngine.iOS.DeviceGeneration.iPadAir1:
                        videoQualityValue = 2;
                        break;
                    default:
                        videoQualityValue = 1;
                        break;
                }
#endif
                PlayerPrefs.SetInt("QualityLevel", videoQualityValue);
            }

            SetQuality(gameManager, videoQualityValue);

            if(PlayerPrefs.HasKey("WaterQuality"))
                SetWaterQuality(gameManager, PlayerPrefs.GetInt("WaterQuality"));
            else
                SetWaterQuality(gameManager, 0);
        }

        public static void SetWaterQuality(GameManager gameManager, int value)
        {
            CurrentWaterQuality = (WaterQuality) value;
            gameManager.World.WaterController.waterQuality = CurrentWaterQuality;
            PlayerPrefs.SetInt("WaterQuality", value);
        }
    }
}