using Assets.Scripts;
using System.Collections.Generic;
using UnityEngine;

public class CraftResourceZone : MonoBehaviour
{
    public TerrainId TerrainId = TerrainId.Terrain1;
    public MeshRenderer ZoneRender;
    public List<GameObject> ResourcePrefabs;
    public int MinSpawnAmount;
    public int MaxSpawnAmount;
    public float Yoffset;
    public float AppearPercent;
	[Range(-90, 90)]
	public float XRotationMin = 0.0f;
	[Range(-90, 90)]
    public float XRotationMax = 0.0f;
	[Range(-90, 90)]
	public float YRotationMin = 0.0f;
	[Range(-90, 90)]
    public float YRotationMax = 0.0f;
	[Range(-90, 90)]
	public float ZRotationMin = 0.0f;
	[Range(-90, 90)]
    public float ZRotationMax = 0.0f;

    protected GameManager _gameManager;
    protected Terrain CurrentTerrain;

    public void Init(GameManager gameManager)
    {
        _gameManager = gameManager;
        CurrentTerrain = TerrainId == TerrainId.Terrain1 ? _gameManager.Terrain1 : _gameManager.Terrain2;
        ZoneRender.enabled = false;

        SpawnResources();
    }

    protected virtual void SpawnResources()
    {
        Vector3 zoneCenter = transform.position;
        int spawnAmount = Random.Range(MinSpawnAmount, MaxSpawnAmount);
        for (int i = 0; i < spawnAmount; i++)
        {
            var go = ResourcePrefabs[Random.Range(0, ResourcePrefabs.Count)];

            if (AppearPercent < Random.Range(0, 100)) continue;
            Vector3 spawnPos = zoneCenter + new Vector3(Random.Range(transform.localScale.x / 2, -transform.localScale.x / 2), 0.0f, Random.Range(transform.localScale.z / 2, -transform.localScale.z / 2));
			//spawnPos.y = Terrain.activeTerrain.SampleHeight(spawnPos) + Yoffset;
            spawnPos.y = CurrentTerrain.SampleHeight(spawnPos) + Yoffset;
			var spawnedGo = Instantiate(go, spawnPos, Quaternion.Euler(Random.Range(XRotationMin, XRotationMax), Random.Range(YRotationMin, YRotationMax), Random.Range(ZRotationMin, ZRotationMax))) as GameObject;
        }
    }
}
