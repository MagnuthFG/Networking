using SF = UnityEngine.SerializeField;
using Unity.Netcode;
using UnityEngine;

public class HealthPack : NetworkBehaviour
{
    [SF] private int _healAmount = 25;
    [SF] private GameObject _healthPrefab = null;

    void OnTriggerEnter2D(Collider2D other){
        if (!IsServer && !IsHost) return;

        if (other.TryGetComponent<Health>(out var health)){
            health.Heal(_healAmount);
        }
        var netObj = GetComponent<NetworkObject>();

        if (netObj != null && netObj.IsSpawned == true)
            netObj.Despawn();
    }
}
