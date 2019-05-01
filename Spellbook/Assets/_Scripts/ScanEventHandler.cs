﻿using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Vuforia;

/// <summary>
/// Grace Ko
/// This script handles Vuforia behavior
/// </summary>
public class ScanEventHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private GameObject instructionImage;
    [SerializeField] private Text moveInfo;
    [SerializeField] private UnityEngine.UI.Image backgroundImage;
    private void Start()
    {
        VuforiaBehaviour.Instance.enabled = false;
        moveInfo.text = "You move " + UICanvasHandler.instance.spacesMoved.ToString() + " spaces this turn.";
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        VuforiaBehaviour.Instance.enabled = true;
        instructionImage.SetActive(false);
        backgroundImage.enabled = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        VuforiaBehaviour.Instance.enabled = false;
        instructionImage.SetActive(true);
        backgroundImage.enabled = true;
    }
}
