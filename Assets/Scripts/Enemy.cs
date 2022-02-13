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
        _playerCooldowns = FindObjectOfType<Player>().Cooldowns;
    }

    public IEnumerator OnBattleTurn(string message)
    {
        message = message.ToLower();

        // First the player does some action 
        switch (message)
        {
            case "scratch":
            case "bite":
            case "kick":
                _doPlayerAttack(message);
                break;
            default:
            case "rest":
                float heal = Random.Range(5f, 25f);
                _playerAttributes.UpdateHealth(heal);
                break;
            case "flee":
                if (_calculateMiss(_playerAttributes.SPD, Attributes.SPD, false)) _endBattle();
                break;
        }

        yield return new WaitForSeconds(1f);

        // If the enemy runs out of health, trigger destory and call back to UIManager to quit the battle
        if (Attributes.HP <= 0)
        {
            _playerAttributes.EXP += Attributes.EXP;
            _endBattle();
            Destroy(gameObject);
        }

        _doEnemyAttack();
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
