﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vuforia;

// for scanning board spaces
public class CustomEventHandler : MonoBehaviour, ITrackableEventHandler
{
    protected TrackableBehaviour mTrackableBehaviour;
    protected TrackableBehaviour.Status m_PreviousStatus;
    protected TrackableBehaviour.Status m_NewStatus;

    private Coroutine coroutineReference;
    private bool CR_running;

    Player localPlayer;
    List<ItemObject> itemList;
    
    void Start()
    {
        localPlayer = GameObject.FindGameObjectWithTag("LocalPlayer").GetComponent<Player>();
        itemList = GameObject.Find("ItemList").GetComponent<ItemList>().listOfItems;

        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
    }

    void OnDestroy()
    {
        if (mTrackableBehaviour)
            mTrackableBehaviour.UnregisterTrackableEventHandler(this);
    }

    public void OnTrackableStateChanged(
        TrackableBehaviour.Status previousStatus,
        TrackableBehaviour.Status newStatus)
    {
        m_PreviousStatus = previousStatus;
        m_NewStatus = newStatus;

        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED ||
            newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found");
            // basically, wait 3 seconds before it'll start scanning the target
            coroutineReference = StartCoroutine(ScanTime());
        }
        else if (previousStatus == TrackableBehaviour.Status.TRACKED &&
                 newStatus == TrackableBehaviour.Status.NO_POSE)
        {
            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");
            OnTrackingLost();
        }
        else
        {
            // For combo of previousStatus=UNKNOWN + newStatus=UNKNOWN|NOT_FOUND
            // Vuforia is starting, but tracking has not been lost or found yet
            // Call OnTrackingLost() to hide the augmentations
            OnTrackingLost();
        }
    }
    protected virtual void OnTrackingFound()
    {

        // in board_space_handling region
        // only scan item if player hasn't scanned a space this turn
        if(!localPlayer.Spellcaster.scannedSpaceThisTurn)
            scanItem(mTrackableBehaviour.TrackableName);
    }

    protected virtual void OnTrackingLost()
    {
        if(CR_running)
        {
            StopCoroutine(coroutineReference);
        }
    }

    IEnumerator ScanTime()
    {
        Debug.Log("coroutine running");
        CR_running = true;
        yield return new WaitForSeconds(1f);
        CR_running = false;

        OnTrackingFound();
    }
    
    private void scanItem(string trackableName)
    {
        // checking quests that require space scan
        QuestTracker.instance.CheckSpaceQuest(trackableName);
        QuestTracker.instance.CheckErrandQuest(trackableName);

        // call function based on target name
        switch (trackableName)
        {
            case "mana":
                int m = (int)UnityEngine.Random.Range(100, 700);
                localPlayer.Spellcaster.CollectMana(m);
                break;

            case "item":
                // choose a random item to give to player from list
                ItemObject item = itemList[UnityEngine.Random.Range(0, itemList.Count - 1)];
                PanelHolder.instance.displayBoardScan("You found an Item!", "You got found a " + item.name + "!", item.sprite);
                SoundManager.instance.PlaySingle(SoundManager.itemfound);
                localPlayer.Spellcaster.AddToInventory(item);
                break;

            case "capital":
                SceneManager.LoadScene("ShopScene");
                break;

            #region town_spaces
            case "town_alchemist":
                Quest alchemyManaQuest = new AlchemyManaQuest(localPlayer.Spellcaster.NumOfTurnsSoFar);

                if (localPlayer.Spellcaster.activeQuests.Count > 0)
                {
                    foreach (Quest q in localPlayer.Spellcaster.activeQuests)
                    {
                        if (q.questName.Equals(alchemyManaQuest.questName))
                        {
                            PanelHolder.instance.displayEvent("Alchemy Town", "You're already on a quest for this town.");
                            break;
                        }
                        else
                        {
                            PanelHolder.instance.displayQuest(alchemyManaQuest);
                            break;
                        }
                    }
                }
                else
                {
                    PanelHolder.instance.displayQuest(alchemyManaQuest);
                }
                break;

            case "town_arcanist":
                Quest arcaneSpellQuest = new ArcaneSpellQuest(localPlayer.Spellcaster.NumOfTurnsSoFar);
                if (localPlayer.Spellcaster.activeQuests.Count > 0)
                {
                    foreach (Quest q in localPlayer.Spellcaster.activeQuests)
                    {
                        if (q.questName.Equals(arcaneSpellQuest.questName))
                        {
                            PanelHolder.instance.displayEvent("Arcane Town", "You're already on a quest for this town.");
                            break;
                        }
                        else
                        {
                            PanelHolder.instance.displayQuest(arcaneSpellQuest);
                            break;
                        }
                    }
                }
                else
                {
                    PanelHolder.instance.displayQuest(arcaneSpellQuest);
                }
                break;

            case "town_chronomancer":
                Quest timeMoveQuest = new TimeMoveQuest(localPlayer.Spellcaster.NumOfTurnsSoFar);
                if (localPlayer.Spellcaster.activeQuests.Count > 0)
                {
                    foreach (Quest q in localPlayer.Spellcaster.activeQuests)
                    {
                        if (q.questName.Equals(timeMoveQuest.questName))
                        {
                            PanelHolder.instance.displayEvent("Chronomancy Town", "You're already on a quest for this town.");
                            break;
                        }
                        else
                        {
                            PanelHolder.instance.displayQuest(timeMoveQuest);
                            break;
                        }
                    }
                }
                else
                {
                    PanelHolder.instance.displayQuest(timeMoveQuest);
                }
                break;

            case "town_elementalist":
                Quest elementalErrandQuest = new ElementalErrandQuest(localPlayer.Spellcaster.NumOfTurnsSoFar);
                if (localPlayer.Spellcaster.activeQuests.Count > 0)
                {
                    foreach (Quest q in localPlayer.Spellcaster.activeQuests)
                    {
                        if (q.questName.Equals(elementalErrandQuest.questName))
                        {
                            PanelHolder.instance.displayEvent("Elemental Town", "You're already on a quest for this town.");
                            break;
                        }
                        else
                        {
                            PanelHolder.instance.displayQuest(elementalErrandQuest);
                            break;
                        }
                    }
                }
                else
                {
                    PanelHolder.instance.displayQuest(elementalErrandQuest);
                }
                break;

            case "town_illusionist":
                Quest illusionSpaceQuest = new IllusionSpaceQuest(localPlayer.Spellcaster.NumOfTurnsSoFar);
                if (localPlayer.Spellcaster.activeQuests.Count > 0)
                {
                    foreach (Quest q in localPlayer.Spellcaster.activeQuests)
                    {
                        if (q.questName.Equals(illusionSpaceQuest.questName))
                        {
                            PanelHolder.instance.displayEvent("Trickster Town", "You're already on a quest for this town.");
                            break;
                        }
                        else
                        {
                            PanelHolder.instance.displayQuest(illusionSpaceQuest);
                            break;
                        }
                    }
                }
                else
                {
                    PanelHolder.instance.displayQuest(illusionSpaceQuest);
                }
                break;

            case "town_summoner":
                Quest summonManaQuest = new SummoningManaQuest(localPlayer.Spellcaster.NumOfTurnsSoFar);
                if (localPlayer.Spellcaster.activeQuests.Count > 0)
                {
                    foreach (Quest q in localPlayer.Spellcaster.activeQuests)
                    {
                        if (q.questName.Equals(summonManaQuest.questName))
                        {
                            PanelHolder.instance.displayEvent("Summoner Town", "You're already on a quest for this town.");
                            break;
                        }
                        else
                        {
                            PanelHolder.instance.displayQuest(summonManaQuest);
                            break;
                        }
                    }
                }
                else
                {
                    PanelHolder.instance.displayQuest(summonManaQuest);
                }
                break;
            #endregion

            #region glyph_spaces
            case "glyph_alchemist":
                int g1 = (int)UnityEngine.Random.Range(0, 2);
                if (g1 == 0)
                    localPlayer.Spellcaster.CollectGlyph("Alchemy D Glyph");
                else
                    localPlayer.Spellcaster.CollectGlyph("Alchemy C Glyph");
                break;
            case "glyph_arcanist":
                int g2 = (int)UnityEngine.Random.Range(0, 2);
                if (g2 == 0)
                    localPlayer.Spellcaster.CollectGlyph("Arcane D Glyph");
                else
                    localPlayer.Spellcaster.CollectGlyph("Arcane C Glyph");
                break;
            case "glyph_chronomancer":
                int g3 = (int)UnityEngine.Random.Range(0, 2);
                if (g3 == 0)
                    localPlayer.Spellcaster.CollectGlyph("Time D Glyph");
                else
                    localPlayer.Spellcaster.CollectGlyph("Time C Glyph");
                break;
            case "glyph_elementalist":
                int g4 = (int)UnityEngine.Random.Range(0, 2);
                if (g4 == 0)
                    localPlayer.Spellcaster.CollectGlyph("Elemental D Glyph");
                else
                    localPlayer.Spellcaster.CollectGlyph("Elemental C Glyph");
                break;
            case "glyph_illusionist":
                int g5 = (int)UnityEngine.Random.Range(0, 2);
                if (g5 == 0)
                    localPlayer.Spellcaster.CollectGlyph("Illusion D Glyph");
                else
                    localPlayer.Spellcaster.CollectGlyph("Illusion C Glyph");
                break;
            case "glyph_summoner":
                int g6 = (int)UnityEngine.Random.Range(0, 2);
                if (g6 == 0)
                    localPlayer.Spellcaster.CollectGlyph("Summoning D Glyph");
                else
                    localPlayer.Spellcaster.CollectGlyph("Summoning C Glyph");
                break;
            #endregion

            default:
                break;
        }
        localPlayer.Spellcaster.scannedSpaceThisTurn = true;
    }
}