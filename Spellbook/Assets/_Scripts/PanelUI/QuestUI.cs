﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// used for all quest purposes
public class QuestUI : MonoBehaviour
{
    [SerializeField] private Text titleText;
    [SerializeField] private Text infoText;
    [SerializeField] private Button singleButton;
    [SerializeField] private Button singleButton1;
    [SerializeField] private GameObject ribbon;

    public bool panelActive = false;
    public string panelID = "quest";

    private void DisablePanel()
    {
        gameObject.SetActive(false);
    }
    public void EnablePanel()
    {
        gameObject.SetActive(true);
    }

    // use this if quest rewards are glyphs
    public void DisplayQuestGlyphs(Quest quest)
    {
        titleText.text = quest.questName;
        infoText.text = quest.questDescription + "\nTurn Limit: " + quest.turnLimit;

        string reward1 = "", reward2 = "";
        // getting the glyph rewards of the quest and loading its image
        foreach(KeyValuePair<string, List<string>> kvp in quest.rewards)
        {
            if(kvp.Key.Equals("Glyph"))
            {
                reward1 = kvp.Value[0];
                reward2 = kvp.Value[1];
            }
        }

        // setting panel images to glyphs to display rewards
        gameObject.transform.Find("image_reward1").GetComponent<Image>().sprite = Resources.Load<Sprite>("GlyphArt/" + reward1);
        gameObject.transform.Find("image_reward2").GetComponent<Image>().sprite = Resources.Load<Sprite>("GlyphArt/" + reward2);

        singleButton.onClick.AddListener(() => buttonClicked("accept", quest));
        singleButton1.onClick.AddListener(() => buttonClicked("deny", quest));

        // if current scene is Vuforia, change everything to image
        if (SceneManager.GetActiveScene().name.Equals("VuforiaScene"))
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            gameObject.GetComponent<Image>().enabled = true;

            foreach(Transform t in ribbon.transform)
            {
                t.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                t.gameObject.GetComponent<Image>().enabled = true;
            }
        }

        gameObject.SetActive(true);

        if (!PanelHolder.panelQueue.Peek().Equals(panelID))
        {
            DisablePanel();
        }
    }

    private void buttonClicked(string input, Quest q)
    {
        gameObject.SetActive(false);
        
        GameObject player = GameObject.FindGameObjectWithTag("LocalPlayer");

        // add quest to player's list of active quests if they accept
        if (input.Equals("accept"))
        {
            player.GetComponent<Player>().Spellcaster.activeQuests.Add(q);
            SoundManager.instance.PlaySingle(SoundManager.questaccept);
        }
        else
            SoundManager.instance.PlaySingle(SoundManager.buttonconfirm);

        // end player's turn and take them back to main page
        bool endSuccessful = player.GetComponent<Player>().onEndTurnClick();
        if (endSuccessful)
        {
            player.GetComponent<Player>().Spellcaster.hasAttacked = false;
            Scene m_Scene = SceneManager.GetActiveScene();
            if (m_Scene.name != "MainPlayerScene")
            {
                SceneManager.LoadScene("MainPlayerScene");
            }

        }
        PanelHolder.panelQueue.Dequeue();
        PanelHolder.instance.CheckPanelQueue();
    }
}
