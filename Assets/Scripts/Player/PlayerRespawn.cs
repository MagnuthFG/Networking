using SF = UnityEngine.SerializeField;
using Unity.Netcode;
using UnityEngine;
using System.Collections;

public class PlayerRespawn : NetworkBehaviour
{
    [Header("Settings")]
    [SF] private int _maxRespawns = 3;
    [SF] private float _respawnDelay = 5f;
    [SF] private float _respawnRadius = 10f;
    [Space]
    [SF] private Health _health = null;

    private NetworkVariable<Vector2> _spawnPoint = new();
    private int _respanCount = 0;


    public override void OnNetworkSpawn(){
        if (IsClient){
            _spawnPoint.OnValueChanged += OnSpawnPointChanged;
        }
        if (IsServer || IsHost){
            _health.CurrentHealth.OnValueChanged += OnHealthChanged;
            _respanCount = _maxRespawns;
        }
    }

    private void OnSpawnPointChanged(Vector2 previous, Vector2 current){
        transform.position = current;
    }

    private void OnHealthChanged(int previous, int current){
        if (current == 0 && _respanCount > 0)
            StartCoroutine(RespawnTimer());
    }

    private IEnumerator RespawnTimer(){
        yield return new WaitForSeconds(
            _respawnDelay
        );
        _spawnPoint.Value = 
            Random.insideUnitCircle * 
            _respawnRadius;

        _health.Respawn();
        _respanCount--;
    }

}
