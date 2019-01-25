﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 A base class that all SpellCaster types/classes will inherit from.
     */
public abstract class SpellCaster 
{
    public float fMaxHealth;
    public float fCurrentHealth;

    public int iBasicAttackStrength;
    public int numMana;
    public int numSpellPieces;

    public string classType;

    private List<string> spellsAllowed;
    private List<string> spellsCollected;
    public Chapter chapter;

    // TODO:
    //private string backGroundStory; 
    //private Inventory inventory;
    //public Image icon;
    //Implement:
    //Object DeleteFromInventory(string itemName, int count); 
    //void AddToSpellBook(Spell spell, int pageNum);

    // Virtual Functions
    public abstract void SpellCast();


    // CTOR
    public SpellCaster()
    {
        //fMaxHealth = 20.0f;     //Commented out in case Spellcasters have different max healths.
        numSpellPieces = 0;
        numMana = 0;
        spellsAllowed = new List<string>();
        spellsCollected = new List<string>();
        chapter = new Chapter();
    }

    void AddToInventory(string item, int count)
    {
        //inventory.add(item, count);
    }

    void TakeDamage(int dmg)
    {
        fCurrentHealth -= dmg;
    }

    void HealDamage(int heal)
    {
        fCurrentHealth += heal;
        if(fCurrentHealth > fMaxHealth)
        {
            fCurrentHealth = fMaxHealth;
        }
    }
}
