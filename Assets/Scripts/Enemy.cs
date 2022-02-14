using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Attributes))]
public class Enemy : MonoBehaviour
{
    public Sprite Sprite;
    public Attributes Attributes;
    public DifficultyLevel Difficulty;

    private Attributes _playerAttributes;
    private Dictionary<string, int> _playerCooldowns;

    private bool _weakened = false;

    // Start is called before the first frame update
    void Start()
    {
        Sprite = GetComponent<SpriteRenderer>().sprite;
        Attributes = GetComponent<Attributes>();

        _playerAttributes = FindObjectOfType<Player>().Attributes;
        _playerCooldowns = new Dictionary<string, int>();
    }

    public IEnumerator OnBattleTurn(string message, Action<List<string>> postTurnAction = null)
    {
        message = message.ToLower();

        _updateCooldowns();

        // First the player does some action 
        switch (message)
        {
            case "scratch":
                _doPlayerAttack("scratch");
                _playerCooldowns["scratch"] = 1;
                break;
            case "bite":
                _doPlayerAttack("bite");
                _playerCooldowns["bite"] = 6;
                break;
            case "kick":
                _doPlayerAttack("kick");
                _playerCooldowns["kick"] = 3;
                break;
            case "rest":
                float heal = Random.Range(5f, 25f);
                _playerAttributes.UpdateHealth(heal);
                _playerCooldowns["rest"] = 1;
                break;
            case "flee":
                if (_calculateMiss(_playerAttributes.SPD, Attributes.SPD, false)) _endBattle();
                _playerCooldowns["flee"] = 1;
                break;
            default:
                break;
        }

        yield return new WaitForSeconds(2f);

        // If the enemy runs out of health, trigger destory and call back to UIManager to quit the battle
        if (Attributes.HP <= 0)
        {
            _playerAttributes.EXP += Attributes.EXP;
            _endBattle();
            UIManager ui = FindObjectOfType<UIManager>();
            ui.QuitBattle();
            GameTracker.enemiesKilledSinceLastCheckpoint.Add(this.gameObject);
            
            Destroy(gameObject);
        }

        _doEnemyAttack();
        postTurnAction?.Invoke(new List<string>(_playerCooldowns.Keys));
    }

    private void _doPlayerAttack(string type)
    {
        if (type.Equals("kick") && _calculateMiss(_playerAttributes.SPD, Attributes.SPD, true)) return;

        float damage = (type.Equals("scratch")) ? 10 :
                       (type.Equals("kick"))    ? 20 :
                       (type.Equals("bite"))    ? 30 : 0;
        if (Mathf.Abs(damage) < 0.00001) 
        {
            Debug.Log("Invalid attack.");
            return;
        }

        float advantage = _calculateAdvantage(_playerAttributes, Attributes, true);
        if (_weakened) advantage += advantage;
        
        if (type.Equals("bite")) _weakened = true;

        Attributes.UpdateHealth(-(damage + advantage));
    }

    private void _doEnemyAttack()
    {
        float advantage = _calculateAdvantage(Attributes, _playerAttributes, false);
        float maxDamage = (Difficulty is DifficultyLevel.Easy)   ? 15 :
                          (Difficulty is DifficultyLevel.Medium) ? 25 : 50;
        float damage = Random.Range(5f, maxDamage);
        _playerAttributes.UpdateHealth(-(damage + advantage));
    }

    private float _calculateAdvantage(Attributes attack, Attributes defend, bool player)
    {
        float multiplier = _difficultyMultiplier(player);

        return multiplier * Mathf.Max(attack.ATK - defend.DEF, 0);
    }

    private bool _calculateMiss(float attackSPD, float defendSPD, bool player)
    {
        // Inverse difficulty multiplier
        // For easy enemies, we want high advantage and low miss rate
        // For hard enemies, we want low advantage and high miss rate
        float multiplier = _difficultyMultiplier(!player);
        float chance = Mathf.Clamp(multiplier * (defendSPD / (attackSPD + defendSPD)), 0, 1f);

        return Random.Range(0f, 1f) < chance;
    }

    private float _difficultyMultiplier(bool player)
    {
        return 
            // If this ia a player's attack on the enemy
            (player) ? 
            (Difficulty is DifficultyLevel.Easy)   ? 1.5f :
            (Difficulty is DifficultyLevel.Medium) ? 1f   : 0.5f :
            // If this is a an enemy's attack on the player
            (Difficulty is DifficultyLevel.Easy)   ? 0.5f :
            (Difficulty is DifficultyLevel.Medium) ? 1f   : 1.5f;
    }

    private void _updateCooldowns()
    {
        Dictionary<string, int> cooldowns = new Dictionary<string, int>();
        foreach (var item in _playerCooldowns)
        {
            int cooldown = item.Value - 1;
            if (cooldown > 0) cooldowns[item.Key] = cooldown;
        }
        _playerCooldowns = cooldowns;
    }

    private void _endBattle()
    {
        UIManager ui = FindObjectOfType<UIManager>();
        ui.QuitBattle();
    }

    public enum DifficultyLevel {
        Easy,
        Medium,
        Hard
    }
}
