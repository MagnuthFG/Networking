using SF = UnityEngine.SerializeField;
using Unity.Netcode;
using UnityEngine;

public class AmmoPack : NetworkBehaviour
{
    [SF] private int _ammoCount = 5;
    [SF] private Vector2Int _minMaxSpawnX = new Vector2Int(-4, 4);
    [SF] private Vector2Int _minMaxSpawnY = new Vector2Int(-1, 1);
    [SF] private GameObject _ammoPrefab = null;

    void OnTriggerEnter2D(Collider2D other){
        if (!IsServer && !IsHost) return;

        if (other.TryGetComponent<FiringAction>(out var action)){
            action.AddAmmo(_ammoCount);
        }
        var position = new Vector3(
            Random.Range(_minMaxSpawnX.x, _minMaxSpawnX.y),
            Random.Range(_minMaxSpawnY.x, _minMaxSpawnY.y), 0
        );
        GameObject newMine = Instantiate(
            _ammoPrefab, position,  Quaternion.identity
        );
        var netObj = newMine.GetComponent<NetworkObject>();
            netObj.Spawn();

        netObj = GetComponent<NetworkObject>();

        if (netObj != null && netObj.IsSpawned == true)
            netObj.Despawn();
    }
}
