using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatPotion : Item
{
    private enum AttributeType
    {
        ATK,
        DEF,
        SPD
    }

    [SerializeField] private List<AttributeType> _attributeTypes;
    public List<float> Amount;
    public int turns;
    public bool active;
    public int turnsPassed;
    private Attributes _attributes;
    

    public override void Use()
    {
        _attributes = FindObjectOfType<Player>().Attributes;
        StartCoroutine(StatusEffect());

        base.Use();
    }

    public void PlayerTurnPass()
    {
        if (active) turnsPassed++;
    }

    private IEnumerator StatusEffect()
    {
        active = true;
        DoModifyStats(true);
        while (true)
        {
            if (turnsPassed >= turns){
                DoModifyStats(false);
                active = false;
                yield break;
            }

            yield return new WaitForSeconds(2);
        }
    }

    private void DoModifyStats(bool positive)
    {
        for (int i = 0; i < _attributeTypes.Count; i++)
        {
            var modifier = Amount[i];
            if (!positive) modifier = -modifier;
            
            switch (_attributeTypes[i])
            {
                case AttributeType.ATK:
                    _attributes.ATK += modifier;
                    break;
                case AttributeType.DEF:
                    _attributes.ATK += modifier;
                    break;
                case AttributeType.SPD:    
                    _attributes.SPD += modifier;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        } 
    }
    
}

