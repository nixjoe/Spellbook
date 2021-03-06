﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Grace Ko
/// singleton used to track player's quests
/// instantiated in MainPageHandler.setUpMainPage();
/// </summary>
public class QuestTracker : MonoBehaviour
{
    public static QuestTracker instance = null;

    Player localPlayer;

    void Awake()
    {
        //Check if there is already an instance of QuestTracker
        if (instance == null)
            //if not, set it to this.
            instance = this;
        //If instance already exists:
        else if (instance != this)
            //Destroy this, this enforces our singleton pattern so there can only be one instance of QuestTracker.
            Destroy(gameObject);

        //Set QuestTracker to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        localPlayer = GameObject.FindGameObjectWithTag("LocalPlayer").GetComponent<Player>();
    }

    // AlchemyManaQuest and SummoningManaQuest - Checked in Spellcaster.cs in CollectMana()
    public void CheckManaQuest(int mana)
    {
        foreach(Quest q in localPlayer.Spellcaster.activeQuests.ToArray())
        {
            if (q.questType.Equals("Collect Mana"))
            {
                q.manaTracker += mana;
                Debug.Log("Quest mana tracker: " + q.manaTracker);

                if (q.manaTracker >= q.manaRequired)
                {
                    q.questCompleted = true;
                }
                if (q.questCompleted)
                {
                    SoundManager.instance.PlaySingle(SoundManager.questsuccess);
                    PanelHolder.instance.displayNotify(q.questName + " Completed!",
                                                        "You completed the quest! You earned:\n\n" + q.DisplayReward(), "OK");
                    localPlayer.Spellcaster.activeQuests.Remove(q);
                    GiveRewards(q);
                }
            }
        }
    }

    // IllusionSpaceQuest - Checked in CustomEventHandler.cs in ScanItem()
    public void CheckSpaceQuest(string spaceName)
    {
        foreach (Quest q in localPlayer.Spellcaster.activeQuests.ToArray())
        {
            if (q.questType.Equals("Specific Space"))
            {
                if (spaceName.Equals(q.spaceName))
                {
                    ++q.spacesLanded;
                }

                if (q.spacesLanded >= q.spacesRequired)
                {
                    q.questCompleted = true;
                }
                if (q.questCompleted)
                {
                    SoundManager.instance.PlaySingle(SoundManager.questsuccess);
                    PanelHolder.instance.displayNotify(q.questName + " Completed!",
                                                        "You completed the quest! You earned:\n\n" + q.DisplayReward(), "OK");
                    localPlayer.Spellcaster.activeQuests.Remove(q);
                    GiveRewards(q);
                }
            }
        }
    }

    // TimeMoveQuest - Checked in DiceRoll.cs in Roll()
    public void CheckMoveQuest(int moveSpaces)
    {
        foreach (Quest q in localPlayer.Spellcaster.activeQuests.ToArray())
        {
            if (q.questType.Equals("Movement"))
            {
                q.spacesTraveled += moveSpaces;
                if (q.spacesTraveled >= q.spacesRequired)
                {
                    q.questCompleted = true;
                }
                if (q.questCompleted)
                {
                    SoundManager.instance.PlaySingle(SoundManager.questsuccess);
                    PanelHolder.instance.displayNotify(q.questName + " Completed!",
                                                        "You completed the quest! You earned:\n\n" + q.DisplayReward(), "OK");
                    localPlayer.Spellcaster.activeQuests.Remove(q);
                    GiveRewards(q);
                }
            }
        }
    }

    // ElementalErrandQuest -  checked in CustomEventHandler.cs in ScanItem()
    public void CheckErrandQuest(string spaceName)
    {
        foreach (Quest q in localPlayer.Spellcaster.activeQuests.ToArray())
        {
            if (q.questType.Equals("Errand"))
            {
                if (spaceName.Equals(q.spaceName))
                {
                    // check if player has the item for the errand
                    if(localPlayer.Spellcaster.inventory.Contains(q.item))
                    {
                        q.questCompleted = true;
                    }
                }
                if (q.questCompleted)
                {
                    SoundManager.instance.PlaySingle(SoundManager.questsuccess);
                    PanelHolder.instance.displayNotify(q.questName + " Completed!",
                                                        "You completed the quest! You earned:\n\n" + q.DisplayReward(), "OK");
                    localPlayer.Spellcaster.activeQuests.Remove(q);
                    // localPlayer.Spellcaster.inventory.Remove(q.item);
                    GiveRewards(q);
                }
            }
        }
    }

    // ArcaneSpellQuest - checked in SpellCastHandler.cs in Update()
    public void CheckSpellQuest(Spell spell)
    {
        foreach (Quest q in localPlayer.Spellcaster.activeQuests.ToArray())
        {
            if (q.questType.Equals("Spell"))
            {
                if (q.spellsCast.Contains(spell))
                {
                    // if player has cast this spell before
                    q.questCompleted = true;
                }
                else
                {
                    q.spellsCast.Add(spell);
                }
                if (q.questCompleted)
                {
                    SoundManager.instance.PlaySingle(SoundManager.questsuccess);
                    PanelHolder.instance.displayNotify(q.questName + " Completed!",
                                                        "You completed the quest! You earned:\n\n" + q.DisplayReward(), "OK");
                    localPlayer.Spellcaster.activeQuests.Remove(q);
                    GiveRewards(q);
                }
            }
        }
    }

    // give player rewards when quest is completed
    public void GiveRewards(Quest q)
    {
        int i = 0;
        foreach (KeyValuePair<string, List<string>> kvp in q.rewards)
        {
            foreach(string s in kvp.Value)
            {
                // calls switch statement in another method b/c we don't want to break loop
                string r = CheckRewards(kvp.Key, kvp.Value[i]);
                ++i;
            }
        }
    }

    // checks w/ switch statement to give rewards (from dictionary's list value)
    private string CheckRewards(string key, string value)
    {
        switch(key)
        {
            case "Glyph":
                localPlayer.Spellcaster.glyphs[value] += 1;
                return value;
            default:
                return value;
        }
    }
}
