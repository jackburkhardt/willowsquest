using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Attributes))]
public class Battle : MonoBehaviour
{
    public Sprite EnemySprite;
    public Attributes EnemyAttributes;
    [SerializeField] private DifficultyLevel EnemyDifficulty;
    private bool _weakened = false;

    private Attributes _playerAttributes;
    private Dictionary<string, int> _playerCooldowns;

    [SerializeField] private float _textScrollDelay;
    [SerializeField] private Text  _displayText;
    [SerializeField] private Image _dialogueBackgroundImage;

    // Start is called before the first frame update
    void Start()
    {
        EnemySprite = GetComponent<SpriteRenderer>().sprite;
        EnemyAttributes = GetComponent<Attributes>();
        _playerAttributes = FindObjectOfType<Player>().Attributes;
    }

    public void OnBattleStart()
    {
        EnemyAttributes.ActiveEnemy = true;
        _playerCooldowns = new Dictionary<string, int>();
    }

    public IEnumerator OnBattleTurn(string message, Action<Dictionary<string, int>> postTurnAction = null)
    {
        message = message.ToLower();

        _updateCooldowns();

        // First the player does some action 
        bool flee = false;
        switch (message)
        {
            case "scratch":
                _doPlayerAttack("scratch");
                _playerCooldowns["scratch"] = 1;
                break;
            case "bite":
                _doPlayerAttack("bite");
                _playerCooldowns["bite"] = 3;
                break;
            case "kick":
                _doPlayerAttack("kick");
                _playerCooldowns["kick"] = 2;
                break;
            case "rest":
                float heal = Random.Range(5f, 25f);
                _playerAttributes.UpdateHealth(heal);
                _playerCooldowns["rest"] = 1;
                StartCoroutine(_displayDialogue(String.Format("You rested for {0:N0} HP!", heal)));
                break;
            case "flee":
                if (!_calculateMiss(_playerAttributes.SPD, EnemyAttributes.SPD, true))
                {
                    _endBattle();
                    flee = true;
                }
                else StartCoroutine(_displayDialogue("You failed to flee!"));
                _playerCooldowns["flee"] = 1;
                break;
            default:
                StartCoroutine(_displayDialogue("You were surprised by the enemy!"));
                break;
        }

        yield return new WaitForSeconds(2f);

        // If the enemy runs out of health, trigger destory and call back to UIManager to quit the battle
        if (EnemyAttributes.HP < 1)
        {
            _playerAttributes.EXP += EnemyAttributes.EXP;
            _endBattle();
            GameTracker.enemiesKilledSinceLastCheckpoint.Add(this.gameObject);
            
            Destroy(gameObject);
        }
        else if (!flee) _doEnemyAttack();

        yield return new WaitForSeconds(2f);

        postTurnAction?.Invoke(_playerCooldowns);
    }

    private void _doPlayerAttack(string type)
    {
        if (type.Equals("kick") && _calculateMiss(_playerAttributes.SPD, EnemyAttributes.SPD, true))
        {
            StartCoroutine(_displayDialogue("You missed!"));
            return;
        }

        float damage = (type.Equals("scratch")) ? 10 :
                       (type.Equals("kick"))    ? 20 :
                       (type.Equals("bite"))    ? 30 : 0;
        if (Mathf.Abs(damage) < 0.00001) 
        {
            Debug.Log("Invalid attack.");
            return;
        }

        float advantage = _calculateAdvantage(_playerAttributes, EnemyAttributes, true);
        if (_weakened) advantage += advantage;
        
        if (type.Equals("bite")) _weakened = true;

        StartCoroutine(_displayDialogue(String.Format("You attacked the enemy for {0:N0} HP!", damage + advantage)));
        EnemyAttributes.UpdateHealth(-(damage + advantage));
    }

    private void _doEnemyAttack()
    {
        float advantage = _calculateAdvantage(EnemyAttributes, _playerAttributes, false);
        float maxDamage = (EnemyDifficulty is DifficultyLevel.Easy)   ? 15 :
                          (EnemyDifficulty is DifficultyLevel.Medium) ? 25 : 50;
        float damage = Random.Range(5f, maxDamage);

        StartCoroutine(_displayDialogue(String.Format("The enemy attacked you for {0:N0} HP!", damage + advantage)));
        _playerAttributes.UpdateHealth(-(damage + advantage));
    }

    private IEnumerator _displayDialogue(string dialogue)
    {
        _displayText.text = "";
        foreach (var ch in dialogue)
        {
            _displayText.text += ch;
            yield return new WaitForSeconds(_textScrollDelay);
        }
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
            (EnemyDifficulty is DifficultyLevel.Easy)   ? 1.5f :
            (EnemyDifficulty is DifficultyLevel.Medium) ? 1f   : 0.5f :
            // If this is a an enemy's attack on the player
            (EnemyDifficulty is DifficultyLevel.Easy)   ? 0.5f :
            (EnemyDifficulty is DifficultyLevel.Medium) ? 1f   : 1.5f;
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
        EnemyAttributes.ActiveEnemy = false;
        _displayText.text = "";
        UIManager ui = FindObjectOfType<UIManager>();
        ui.QuitBattle();
    }

    public enum DifficultyLevel {
        Easy,
        Medium,
        Hard
    }
}