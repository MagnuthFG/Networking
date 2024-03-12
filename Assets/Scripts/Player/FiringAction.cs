using SF = UnityEngine.SerializeField;
using Unity.Netcode;
using UnityEngine;

public class FiringAction : NetworkBehaviour
{
    [Header("Settings")]
    [SF] private int _startingAmmoCount = 10;
    [SF] private int _maxAmmoCount = 15;
    [SF] private float _refireDelay = 1.5f;

    [Header("References")]
    [SF] PlayerController playerController;
    [SF] GameObject clientSingleBulletPrefab;
    [SF] GameObject serverSingleBulletPrefab;
    [SF] Transform bulletSpawnPoint;

    private NetworkManager _netManager = null;
    private NetworkVariable<int> _ammoCount = new();
    private NetworkVariable<float> _fireDelay = new();


    public override void OnNetworkSpawn(){
        _netManager = NetworkManager.Singleton;
        _ammoCount.Value = _startingAmmoCount;
    }

    private void OnEnable(){
        playerController.onFireEvent += Fire;
    }
    private void OnDisable(){
        playerController.onFireEvent -= Fire;
    }


    public void AddAmmo(int amount){
        var ammo = _ammoCount.Value;
        
        amount = amount < 0 ? -amount : amount;
        ammo = Mathf.Clamp(ammo + amount, ammo, _maxAmmoCount);

        _ammoCount.Value = ammo;
    }

    private void Fire(bool isShooting){
        if (!isShooting) return;
        float time = GetNetworkTime();

        if (_ammoCount.Value > 0 &&
            time > _fireDelay.Value){
            ShootLocalBullet();
        }
    }

    [ServerRpc]
    private void ShootBulletServerRpc(){
        GameObject bullet = Instantiate(
            serverSingleBulletPrefab, 
            bulletSpawnPoint.position, 
            bulletSpawnPoint.rotation
        );
        Physics2D.IgnoreCollision(
            bullet.GetComponent<Collider2D>(), 
            transform.GetComponent<Collider2D>()
        );
        ShootBulletClientRpc();

        _fireDelay.Value = 
            GetNetworkTime() + 
            _refireDelay;

        _ammoCount.Value--;
    }

    [ClientRpc]
    private void ShootBulletClientRpc(){
        if (IsOwner) return;

        GameObject bullet = Instantiate(
            clientSingleBulletPrefab, 
            bulletSpawnPoint.position, 
            bulletSpawnPoint.rotation
        );
        Physics2D.IgnoreCollision(
            bullet.GetComponent<Collider2D>(), 
            transform.GetComponent<Collider2D>()
        );
    }

    private void ShootLocalBullet(){
        GameObject bullet = Instantiate(
            clientSingleBulletPrefab, 
            bulletSpawnPoint.position, 
            bulletSpawnPoint.rotation
        );
        Physics2D.IgnoreCollision(
            bullet.GetComponent<Collider2D>(), 
            transform.GetComponent<Collider2D>()
        );
        ShootBulletServerRpc();
    }


    private float GetNetworkTime(){
        return _netManager.LocalTime.TimeAsFloat;
    }

}
