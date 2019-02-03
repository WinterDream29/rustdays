using UnityEngine;
using System.Collections;
using Assets.Scripts;
using System.Collections.Generic;

public class SpawnZonesAgregator : MonoBehaviour
{
    private List<CraftResourceZone> _resourceSpawnZones = new List<CraftResourceZone>();
    private GameManager _gameManager;

    public void Init(GameManager gameManager)
    {
        _gameManager = gameManager;
        if(gameObject.activeSelf)
            StartCoroutine(InitZones());
    }

    private IEnumerator InitZones()
    {
        _resourceSpawnZones.AddRange(gameObject.GetComponentsInChildren<CraftResourceZone>());

        foreach (var resSpawnZone in _resourceSpawnZones)
        {
            resSpawnZone.Init(_gameManager);
            yield return new WaitForEndOfFrame();
        }

        yield break;
    }
}
