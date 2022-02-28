using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Attributes))]
public class Battle : MonoBehaviour
{
    public string EnemyName;
    public Sprite EnemySprite;

    [SerializeField] private Attributes _enemyAttributes;
    [SerializeField] private DifficultyLevel _enemyDifficulty;
    private bool _enemyBlock;

    private float _difficultyMultiplier;
    private static float _moodMultiplier = 2f;

    private Attributes _playerAttributes;
    private Dictionary<string, int> _playerCooldowns;

    private AudioManager _audioManager;

    [SerializeField] private float _textScrollDelay;
    [SerializeField] private Text _displayText;
    [SerializeField] private Image _dialogueBackgroundImage;

    private static Dictionary<string, string> _missDialogues = new Dictionary<string, string>()
    {
        {"scratch", "You missed! Your claws weren't quick enough that time..."},
        {"tease",   "Sticks and stones may break their bones but words will never hurt the them! Your teasing had no effect..."},
        {"taunt",   "They scoff at your trash-talk. Your taunt had no effect..."},
        {"sing",    "They interrupt your melody. You failed to make yourself happy..."},
        {"rest",    "You're too shaken up to rest right now..."},
        {"flee",    "They won't let you flee!"},
    };
    private static Dictionary<string, string> _playerMoveDialogues = new Dictionary<string, string>()
    {
        {"scratch", "You scratched the enemy for {0:N0} HP!"},
        {"tease",   "Your teasing made them sad!"},
        {"taunt",   "Your taunt made them angry!"},
        {"sing",    "You made yourself happy with your favorite song!"},
        {"rest",    "You rested for {0:N0} HP!"},
        {"flee",    "You fleed successfully!"},
    };
    private static Dictionary<string, string> _enemyMoveDialogues = new Dictionary<string, string>()
    {
        {"hit",   "The enemy hit you for {0:N0} HP!"},
        {"block", "The enemy prepares for your next hit!"},
        {"tease", "The enemy's teasing makes you sad!"},
        {"taunt", "The enemy's taunt makes you angry!"},
    };

    // Start is called before the first frame update
    void Start()
    {
        EnemySprite = GetComponent<SpriteRenderer>().sprite;
        _enemyAttributes = GetComponent<Attributes>();
        _playerAttributes = FindObjectOfType<Player>().Attributes;
        _difficultyMultiplier = _calculateDifficulty();
        _audioManager = FindObjectOfType<AudioManager>();
    }

    public void InitializeEnemy()
    {
        _enemyAttributes.HP = 100f;
        _enemyAttributes.MD = Mood.Angry;
    }

    public void OnBattleStart()
    {
        _enemyAttributes.ActiveEnemy = true;
        _playerCooldowns = new Dictionary<string, int>();
    }

    public IEnumerator OnBattleTurn(string message, Action<Dictionary<string, int>> postTurnAction = null)
    {
        message = message.ToLower();

        _updateCooldowns();

        bool flee = _doPlayerMove(message.ToLower());

        yield return new WaitForSeconds(2f);

        if (_enemyAttributes.HP < 1)
        {
            _playerAttributes.EXP += _enemyAttributes.EXP;
            _playerAttributes.UpdateText();

            _endBattle();

            GameTracker.enemiesKilledSinceLastCheckpoint.Add(this.gameObject);

            Destroy(gameObject);
        }
        else if (!flee) _doEnemyMove();

        yield return new WaitForSeconds(2f);

        if (_playerAttributes.HP < 1) _endBattle();
        else postTurnAction?.Invoke(_playerCooldowns);
    }


    bool _doPlayerMove(string type)
    {
        if (_handleMiss(type)) return false;

        int cooldown;
        bool flee = false;

        switch (type)
        {
            case "scratch":
                if (_enemyBlock)
                {
                    _enemyBlock = false;
                    StartCoroutine(_displayDialogue("The enemy blocked your attack!"));
                    break;
                }

                float damage = 15 + _calculateBonus(true);
                if (_playerAttributes.MD is Mood.Angry) damage *= _moodMultiplier;
                else if (_playerAttributes.MD is Mood.Sad) damage /= _moodMultiplier;
                if (_enemyAttributes.MD is Mood.Angry) damage *= _moodMultiplier;
                else if (_enemyAttributes.MD is Mood.Sad) damage /= _moodMultiplier;
                _enemyAttributes.UpdateHealth(-damage);

                StartCoroutine(_displayDialogue(String.Format(_playerMoveDialogues[type], damage)));
                break;
            case "tease":
                _enemyAttributes.MD = Mood.Sad;
                _enemyBlock = false;

                StartCoroutine(_displayDialogue(_playerMoveDialogues[type]));
                break;
            case "taunt":
                _enemyAttributes.MD = Mood.Angry;
                _enemyBlock = false;

                StartCoroutine(_displayDialogue(_playerMoveDialogues[type]));
                break;
            case "sing":
                _playerAttributes.MD = Mood.Happy;

                StartCoroutine(_displayDialogue(_playerMoveDialogues[type]));
                break;
            case "rest":
                float heal = Random.Range(10f, 33f);
                _playerAttributes.UpdateHealth(heal);

                StartCoroutine(_displayDialogue(String.Format(_playerMoveDialogues[type], heal)));
                break;
            case "flee":
                flee = true;

                StartCoroutine(_displayDialogue(_playerMoveDialogues[type]));
                break;
            default:
                StartCoroutine(_displayDialogue("You were surprised by the enemy!"));
                break;
        }

        if (!type.Equals("scratch"))
        {
            _playerCooldowns.TryGetValue(type, out cooldown);
            _playerCooldowns[type] = cooldown + 1;
        }

        return flee;
    }

    bool _handleMiss(string type)
    {
        if (_calculateMiss())
        {
            StartCoroutine(_displayDialogue(_missDialogues[type]));
            return true;
        }
        return false;
    }

    void _doEnemyMove()
    {
        float selection = Random.Range(0f, 1f);

        // 60% attacking, 20% blocking, 10% teasing and taunting each
        string move =
            (selection < 0.6f) ? "hit" :
            (selection < 0.8f) ? "block" :
            (selection < 0.9f) ? "tease" :
         /* (selection <= 1f) */ "taunt";

        switch (move)
        {
            case "hit":
                float maxDamage = (_enemyDifficulty is DifficultyLevel.Easy) ? 15 :
                                  (_enemyDifficulty is DifficultyLevel.Medium) ? 25 : 50;
                float damage = Random.Range(10f, maxDamage) + _calculateBonus(false);

                if (_enemyAttributes.MD is Mood.Angry) damage *= _moodMultiplier;
                else if (_enemyAttributes.MD is Mood.Sad) damage /= _moodMultiplier;
                if (_playerAttributes.MD is Mood.Angry) damage *= _moodMultiplier;
                else if (_playerAttributes.MD is Mood.Sad) damage /= _moodMultiplier;

                StartCoroutine(_displayDialogue(String.Format(_enemyMoveDialogues["hit"], damage)));
                _playerAttributes.UpdateHealth(-damage);
                break;
            case "block":
                if (_enemyBlock) goto case "hit";

                StartCoroutine(_displayDialogue(String.Format(_enemyMoveDialogues["block"])));
                _enemyBlock = true;
                break;
            case "tease":
                if (_playerAttributes.MD is Mood.Sad) goto case "taunt";

                StartCoroutine(_displayDialogue(_enemyMoveDialogues["tease"]));
                _playerAttributes.MD = Mood.Sad;
                break;
            case "taunt":
                if (_playerAttributes.MD is Mood.Angry) goto case "tease";

                StartCoroutine(_displayDialogue(_enemyMoveDialogues["taunt"]));
                _playerAttributes.MD = Mood.Angry;
                break;
        }

        _playerAttributes.UpdateText();
    }

    IEnumerator _displayDialogue(string dialogue)
    {
        _displayText.text = "";
        foreach (var ch in dialogue)
        {
            _displayText.text += ch;
            yield return new WaitForSeconds(_textScrollDelay);
        }
    }

    float _calculateBonus(bool player)
    {
        Attributes attack = player ? _playerAttributes : _enemyAttributes;
        Attributes defend = player ? _enemyAttributes : _playerAttributes;

        float chance = (attack.MD is Mood.Happy) ? 0.25f : 0.125f;
        bool result = Random.Range(0f, 1f) < chance;

        return _difficultyMultiplier * Mathf.Max(attack.ATK - defend.DEF, 0) * ((result) ? 1.3f : 1f);
    }

    bool _calculateMiss()
    {
        Attributes attack = _playerAttributes;
        Attributes defend = _enemyAttributes;

        // Inverse difficulty multiplier
        // For easy enemies, we want high advantage and low miss rate
        // For hard enemies, we want low advantage and high miss rate
        float multiplier = 0.2f * _difficultyMultiplier;
        float chance = Mathf.Clamp(multiplier * (defend.SPD / (attack.SPD + defend.SPD)) * ((attack.MD is Mood.Happy) ? 1.1f : 1f), 0, 1f);

        return Random.Range(0f, 1f) < chance;
    }

    float _calculateDifficulty()
    {
        return
            (_enemyDifficulty is DifficultyLevel.Easy) ? 0.80f :
            (_enemyDifficulty is DifficultyLevel.Medium) ? 1.00f :
         /* (EnemyDifficulty is DifficultyLevel.Hard) */  1.25f;
    }

    void _updateCooldowns()
    {
        Dictionary<string, int> cooldowns = new Dictionary<string, int>();
        foreach (var item in _playerCooldowns)
        {
            int cooldown = item.Value - 1;
            if (cooldown > 0) cooldowns[item.Key] = cooldown;
        }
        _playerCooldowns = cooldowns;
    }

    void _endBattle()
    {
        _enemyAttributes.ActiveEnemy = false;
        _playerAttributes.MD = Mood.Happy;
        _displayText.text = "";

        _audioManager.StopBattleMusic();

        UIManager ui = FindObjectOfType<UIManager>();
        ui.QuitBattle();
    }

    public enum DifficultyLevel
    {
        Easy,
        Medium,
        Hard
    }

    public string EnemyName1 => EnemyName;

    public Sprite EnemySprite1 => EnemySprite;

    public Attributes EnemyAttributes => _enemyAttributes;

    public DifficultyLevel EnemyDifficulty => _enemyDifficulty;
}
