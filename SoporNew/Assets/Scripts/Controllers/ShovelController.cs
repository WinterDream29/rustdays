using Assets.Scripts.Models;
using Assets.Scripts.Models.ResourceObjects;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class ShovelController : MonoBehaviour
    {
        public Animation Animation;
        public List<Texture> SandTextures;
        public List<Texture> ClayTextures;

        private GameManager _gameManager;

        private int _surfaceIndex = 0;

        private Terrain _terrain;
        private TerrainData _terrainData;
        private Vector3 _terrainPos;

        public void Mine(GameManager gameManager)
        {
            _gameManager = gameManager;
            _terrain = _gameManager.CurrentTerain;
            _terrainData = _terrain.terrainData;
            _terrainPos = _terrain.transform.position;

            Animation.Play("Mine");

            RaycastHit hit;
            Physics.Raycast(transform.position, -Vector3.up, out hit, 10);
            if (hit.collider != null && hit.collider.gameObject != null && hit.collider.transform.tag == "Terrain")

            {
                var setted = false;
                HolderObject item = null;

                _surfaceIndex = GetMainTexture(transform.position);
                var texName = _terrainData.splatPrototypes[_surfaceIndex].texture.name;

                var amount = Random.Range(1, 4);
                foreach (var sandTexture in SandTextures)
                {
                    if (sandTexture.name == texName)
                    {
                        item = HolderObjectFactory.GetItem(typeof(SandResource), amount);
                        setted = true;
                        break;
                    }
                }

                if (!setted)
                {
                    foreach (var clayTexture in ClayTextures)
                    {
                        if (clayTexture.name == texName)
                        {
                            if (Random.Range(0, 100) < 40)
                                item = HolderObjectFactory.GetItem(typeof(ClayResource), amount);
                            else
                                item = HolderObjectFactory.GetItem(typeof(GroundResource), amount);
                            setted = true;
                            break;
                        }
                    }
                }

                if (item != null)
                {

                    if (_gameManager.PlayerModel.Inventory.AddItem(item))
                        _gameManager.Player.MainHud.ShowAddedResource(item.Item.IconName, item.Amount, item.Item.LocalizationName);
                    else
                        _gameManager.Player.MainHud.ShowHudText(Localization.Get("no_place_in_inventory"), HudTextColor.Red);
                }
            }

            SoundManager.PlaySFX(WorldConsts.AudioConsts.ShovelDigging);
        }

        private float[] GetTextureMix(Vector3 worldPos)
        {
            int mapX = (int)(((worldPos.x - _terrainPos.x) / _terrainData.size.x) * _terrainData.alphamapWidth);
            int mapZ = (int)(((worldPos.z - _terrainPos.z) / _terrainData.size.z) * _terrainData.alphamapHeight);

            float[, ,] splatmapData = _terrainData.GetAlphamaps(mapX, mapZ, 1, 1);
            float[] cellMix = new float[splatmapData.GetUpperBound(2) + 1];

            for (int n = 0; n < cellMix.Length; n++)
            {
                cellMix[n] = splatmapData[0, 0, n];
            }
            return cellMix;
        }

        private int GetMainTexture(Vector3 worldPos)
        {
            float[] mix = GetTextureMix(worldPos);

            float maxMix = 0;
            int maxIndex = 0;

            for (int n = 0; n < mix.Length; n++)
            {
                if (mix[n] > maxMix)
                {
                    maxIndex = n;
                    maxMix = mix[n];
                }
            }
            return maxIndex;
        }
    }
}
