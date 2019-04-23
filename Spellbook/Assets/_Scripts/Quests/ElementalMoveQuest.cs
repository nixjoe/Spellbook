﻿using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ElementalMoveQuest : Quest
{
    public ElementalMoveQuest(int turnStarted)
    {
        questName = "Elemental Move Quest";
        questType = "Move";
        questFlavor = "Being a fighter means being quick on your feet.";
        questTask = "Travel 15 spaces.";

        startTurn = turnStarted;
        turnLimit = 3;

        spacesTraveled = 0;
        spacesRequired = 15;

        rewards.Add("Rune", "Elementalist B Rune");
        rewards.Add("Class Rune", "B Rune");

        consequenceMana = 500;

        questCompleted = false;
    }

    // return a string that contains the rewards of the quest
    public override string DisplayReward()
    {
        StringBuilder sb = new StringBuilder();

        foreach (KeyValuePair<string, string> kvp in rewards)
        {
            sb.Append(kvp.Value);
            sb.Append("\n");
        }

        return sb.ToString();
    }
}