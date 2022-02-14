using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class RestStop : MonoBehaviour
{

    private bool used = false;

    public bool Used => used;

    private Player _player;

    private GameTracker _gameTracker;
    
    [SerializeField] private Sprite _usedSprite;
    
    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<Player>();
        _gameTracker = FindObjectOfType<GameTracker>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!used) Rest();
    }

    private void Rest()
    {
        used = true;
        _player.Attributes.HP = 100;
        _player.Attributes.MD = Mood.Happy;
        gameObject.GetComponent<SpriteRenderer>().sprite = _usedSprite;
        Save();
    }

    private void Save()
    {
        GameTracker.enemiesKilledSinceLastCheckpoint.Clear();
        GameTracker.itemsUsedSinceLastCheckpoint.Clear();
    }
}
