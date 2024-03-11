using SF = UnityEngine.SerializeField;
using UnityEngine.Events;
using Unity.Netcode;
using UnityEngine;

public class Health : NetworkBehaviour
{
    [Header("Settings")]
    [SF] private int _maxHealth = 100;
    [Space]
    [SF] private UnityEvent m_OnDeath;
    [SF] private UnityEvent m_OnRespawn;

    public NetworkVariable<int> CurrentHealth = new();
    

    public override void OnNetworkSpawn(){
        CurrentHealth.OnValueChanged += OnHealthChanged;

        if (!IsServer && !IsHost) return;
        CurrentHealth.Value = _maxHealth;
    }

    public void TakeDamage(int damage){
        int health = CurrentHealth.Value;
        if (health == 0) return;

        damage = damage < 0 ? -damage : damage;
        health = Mathf.Clamp(health - damage, 0, health);

        CurrentHealth.Value = health;
    }

    public void Respawn(){
        CurrentHealth.Value = _maxHealth;
    }

    private void OnHealthChanged(int previous, int current){
        if (current == 0)
            m_OnDeath.Invoke();

        else if (previous == 0 && current == _maxHealth)
            m_OnRespawn.Invoke();
    }

}