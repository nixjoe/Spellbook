﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Vuforia;

public class CustomEventHandler : MonoBehaviour, ITrackableEventHandler
{
    protected TrackableBehaviour mTrackableBehaviour;
    protected TrackableBehaviour.Status m_PreviousStatus;
    protected TrackableBehaviour.Status m_NewStatus;

    [SerializeField] private GameObject panel;
    private bool panelOpen = false;

    private Coroutine coroutineReference;
    private bool CR_running;

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
        if(!panelOpen)
        {
            OnTrackingFound();
        }
    }

    private void scanItem(string trackableName)
    {
        // call function based on target name
        switch (trackableName)
        {
            case "spellpiece":
                string collectedPiece = localPlayer.Spellcaster.CollectRandomSpellPiece();
                PanelHolder.instance.displayEvent("You found the " + collectedPiece + "!");
                break;
            case "mana":
                int manaCount = (int)UnityEngine.Random.Range(100, 1000);
                if (localPlayer.Spellcaster.activeSpells.Contains("Arcana Harvest"))
                {
                    manaCount *= 2;
                    localPlayer.Spellcaster.CollectMana(manaCount);
                    PanelHolder.instance.displayEvent("Because of Arcana Harvest, you found double mana and received " + manaCount + " mana!");
                    localPlayer.Spellcaster.activeSpells.Remove("Arcana Harvest");
                    break;
                }
                else
                {
                    localPlayer.Spellcaster.CollectMana(manaCount);
                    PanelHolder.instance.displayEvent("You found " + manaCount + " mana!");
                    break;
                }
            case "glyph":
                string collectedGlyph = localPlayer.Spellcaster.CollectRandomGlyph();
                if(localPlayer.Spellcaster.activeSpells.Contains("Arcana Harvest"))
                {
                    localPlayer.Spellcaster.glyphs[collectedGlyph] += 1;
                    PanelHolder.instance.displayEvent("Because of Arcana Harvest, you found double glyphs and found 2 " + collectedGlyph + "!");
                    localPlayer.Spellcaster.activeSpells.Remove("Arcana Harvest");
                    break;
                }
                else
                {
                    PanelHolder.instance.displayEvent("You found the " + collectedGlyph + "!");
                    break;
                }
            case "event":
                EventSpaceManager.instance.executeEventSpace();
                break;
            case "city":
                PanelHolder.instance.displayEvent("You landed in the city. Nothing really to see here...");
                break;
        }
    }
}