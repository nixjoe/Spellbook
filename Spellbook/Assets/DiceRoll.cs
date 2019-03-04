using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceRoll : MonoBehaviour
{
    public GameObject TextBox;
    public int RandomNumber;

    public void NumberGenerator() 
    {
        RandomNumber = Random.Range(1, 7);
        TextBox.GetComponent<Text>().text = "" + RandomNumber;
    }
}
