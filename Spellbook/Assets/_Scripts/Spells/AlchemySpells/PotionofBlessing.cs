﻿using System.Collections.Generic;
using UnityEngine;

// spell for Alchemy class
public class PotionofBlessing : Spell, ICombatSpell
{
    public PotionofBlessing()
    {
        iTier = 1;

        combatSpell = true;

        sSpellName = "Brew - Potion of Blessing";
        sSpellClass = "Alchemist";
        sSpellInfo = "Heal all allies by half of their max health.";

        requiredRunes.Add("Alchemist A Rune", 1);
        requiredRunes.Add("Illusionist A Rune", 1);
        requiredRunes.Add("Summoner B Rune", 1);
        ColorUtility.TryParseHtmlString("#FFC6D0", out colorPrimary);
        ColorUtility.TryParseHtmlString("#FF7295", out colorSecondary);
    }

    public void CombatCast()
    {
        throw new System.NotImplementedException();
    }

    public override void SpellCast(SpellCaster player)
    {
        //Nothing.
    }
    
}
