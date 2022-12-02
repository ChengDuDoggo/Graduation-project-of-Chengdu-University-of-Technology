using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    public GameObject dialogueBox;
    public Text dialogueText;
    public Image faceRight, faceLeft;
    public Text nameRight, nameLeft;
    public GameObject continueBox;
    private void Awake()
    {
        continueBox.SetActive(false);
    }
}
