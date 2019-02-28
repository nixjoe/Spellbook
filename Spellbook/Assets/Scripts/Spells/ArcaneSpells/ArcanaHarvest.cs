﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// example spell for Arcanist class
public class ArcanaHarvest : Spell
{
    public ArcanaHarvest()
    {
        iTier = 3;
        iManaCost = 400;
        iCoolDown = 0;
        iTurnsActive = 2;

        sSpellName = "Arcana Harvest";
        sSpellClass = "Arcanist";
        sSpellInfo = "Until your next turn, earn double resources (mana, glyphs). Can cast on an ally.";

        requiredGlyphs.Add("Arcane D Glyph", 1);
    }

    public override void SpellCast(SpellCaster player)
    {
        bool canCast = false;
        // checking if player can actually cast the spell
        foreach (KeyValuePair<string, int> kvp in requiredGlyphs)
        {
            if (player.glyphs[kvp.Key] >= 1)
                canCast = true;
        }
        if (canCast && player.iMana > iManaCost)
        {
            // subtract mana and glyph costs
            player.iMana -= iManaCost;
            foreach (KeyValuePair<string, int> kvp in requiredGlyphs)
                player.glyphs[kvp.Key] -= 1;
            
            PanelHolder.instance.displayNotify("You cast " + sSpellName, "You will receive double mana/glyphs until your next turn.");
            player.activeSpells.Add(this);
        }
        else if (player.iMana < iManaCost)
        {
            PanelHolder.instance.displayNotify("Not enough mana!", "You don't have enough mana to cast this spell.");
        }
        else
        {
            PanelHolder.instance.displayNotify("Not enough glyphs!", "You don't have enough glyphs to cast this spell.");
        }
    }
}