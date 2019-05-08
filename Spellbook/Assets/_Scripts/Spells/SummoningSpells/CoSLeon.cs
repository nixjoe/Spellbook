﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// spell for Summoner class
public class CoSLeon : Spell
{
    public CoSLeon()
    {
        iTier = 3;
        iManaCost = 1800;

        combatSpell = false;

        sSpellName = "Call of the Sun - Leon's Shining";
        sSpellClass = "Summoner";
        sSpellInfo = "All allies gain a charge on each of their combat spells.";

        requiredRunes.Add("Elementalist A Rune", 1);
        requiredRunes.Add("Summoner A Rune", 1);
        requiredRunes.Add("Chronomancer A Rune", 1);
    }

    public override void SpellCast(SpellCaster player)
    {
        // subtract mana
        player.iMana -= iManaCost;

        PanelHolder.instance.displayNotify("You cast " + sSpellName, "", "MainPlayerScene");
        player.activeSpells.Add(this);

        player.numSpellsCastThisTurn++;
        SpellTracker.instance.lastSpellCasted = this;
    }
}
