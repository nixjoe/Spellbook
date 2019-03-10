﻿using System;
using System.Collections;
using UnityEngine;
using Vuforia;

public class CustomEventHandler : MonoBehaviour, ITrackableEventHandler
{
    protected TrackableBehaviour mTrackableBehaviour;
    protected TrackableBehaviour.Status m_PreviousStatus;
    protected TrackableBehaviour.Status m_NewStatus;

    private Coroutine coroutineReference;
    private bool CR_running;
    private bool spaceScanned = false;

    Player localPlayer;
    
    void Start()
    {
        localPlayer = GameObject.FindGameObjectWithTag("LocalPlayer").GetComponent<Player>();

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
        CR_running = true;
        yield return new WaitForSeconds(1.5f);
        CR_running = false;

        // only track once; after a space is scanned, do not scan anymore
        if(!spaceScanned)
        {
            OnTrackingFound();
        }
    }
    
    private void scanItem(string trackableName)
    {
        // call function based on target name
        switch (trackableName)
        {
            case "mana":
                int m = (int)UnityEngine.Random.Range(100, 700);
                localPlayer.Spellcaster.CollectMana(m);
                break;

            case "item":
                PanelHolder.instance.displayEvent("Item", "You got a [item name] item!");
                break;

            case "capital":
                PanelHolder.instance.displayEvent("Capital", "Display shop scene here.");
                break;

            #region city_spaces
            case "town_alchemist":
                Quest manaQuest = new AlchemyManaQuest(localPlayer.Spellcaster.NumOfTurnsSoFar);
                PanelHolder.instance.displayEvent(manaQuest.questName, manaQuest.questDescription + "\nTurn Limit: " + manaQuest.turnLimit
                                                    + "\n\nReward:\n" + manaQuest.DisplayReward());
                localPlayer.Spellcaster.activeQuests.Add(manaQuest);
                break;

            case "town_arcanist":
                PanelHolder.instance.displayEvent("Arcanist town", "Nothing really to see here...");
                break;

            case "town_chronomancer":
                PanelHolder.instance.displayEvent("Chronomancer town", "Nothing really to see here...");
                break;

            case "town_elementalist":
                PanelHolder.instance.displayEvent("Elementalist town", "Nothing really to see here...");
                break;

            case "town_illusionist":
                Quest spaceQuest = new IllusionSpaceQuest(localPlayer.Spellcaster.NumOfTurnsSoFar);
                PanelHolder.instance.displayEvent(spaceQuest.questName, spaceQuest.questDescription + "\nTurn Limit: " + spaceQuest.turnLimit
                                                    + "\n\nReward:\n" + spaceQuest.DisplayReward());
                localPlayer.Spellcaster.activeQuests.Add(spaceQuest);
                break;

            case "town_summoner":
                PanelHolder.instance.displayEvent("Summoner town", "Nothing really to see here...");
                break;
            #endregion

            #region glyph_spaces
            case "glyph_alchemist":
                int g1 = (int)UnityEngine.Random.Range(0, 2);
                if (g1 == 0)
                    localPlayer.Spellcaster.CollectGlyph("Alchemy D Glyph");
                else
                    localPlayer.Spellcaster.CollectGlyph("Alchemy C Glyph");
                QuestTracker.instance.CheckSpaceQuest(trackableName);
                break;
            case "glyph_arcanist":
                int g2 = (int)UnityEngine.Random.Range(0, 2);
                if (g2 == 0)
                    localPlayer.Spellcaster.CollectGlyph("Arcane D Glyph");
                else
                    localPlayer.Spellcaster.CollectGlyph("Arcane C Glyph");
                QuestTracker.instance.CheckSpaceQuest(trackableName);
                break;
            case "glyph_chronomancer":
                int g3 = (int)UnityEngine.Random.Range(0, 2);
                if (g3 == 0)
                    localPlayer.Spellcaster.CollectGlyph("Time D Glyph");
                else
                    localPlayer.Spellcaster.CollectGlyph("Time C Glyph");
                QuestTracker.instance.CheckSpaceQuest(trackableName);
                break;
            case "glyph_elementalist":
                int g4 = (int)UnityEngine.Random.Range(0, 2);
                if (g4 == 0)
                    localPlayer.Spellcaster.CollectGlyph("Elemental D Glyph");
                else
                    localPlayer.Spellcaster.CollectGlyph("Elemental C Glyph");
                QuestTracker.instance.CheckSpaceQuest(trackableName);
                break;
            case "glyph_illusionist":
                int g5 = (int)UnityEngine.Random.Range(0, 2);
                if (g5 == 0)
                    localPlayer.Spellcaster.CollectGlyph("Illusion D Glyph");
                else
                    localPlayer.Spellcaster.CollectGlyph("Illusion C Glyph");
                QuestTracker.instance.CheckSpaceQuest(trackableName);
                break;
            case "glyph_summoner":
                int g6 = (int)UnityEngine.Random.Range(0, 2);
                if (g6 == 0)
                    localPlayer.Spellcaster.CollectGlyph("Summoning D Glyph");
                else
                    localPlayer.Spellcaster.CollectGlyph("Summoning C Glyph");
                QuestTracker.instance.CheckSpaceQuest(trackableName);
                break;
            #endregion

            default:
                break;
        }
        spaceScanned = true;
    }
}