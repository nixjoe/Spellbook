using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomNumberGenerator : MonoBehaviour
{
    public GameObject TextBox;
    public int RandomNumber;

    // Start is called before the first frame update
    public void NumberGenerator()
    {
        RandomNumber = Random.Range(1, 6);
        TextBox.GetComponent<Text>().text = "" + RandomNumber;
    }

}
