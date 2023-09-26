using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    [System.Serializable]
    public class State
    {
        public Color fillColor;         //Only for controlling Colors and reaction
        public Color outlineColor;
    }

    public State state { get; private set; }

    public char letter { get; private set; }        //Property which we will use to get Publically, and set word privately ie inside code 


    private Image fill;
    private Outline outline;
    private TextMeshProUGUI text;       

    private void Awake()
    {
        fill = GetComponent<Image>();
        outline = GetComponent<Outline>();

        text = GetComponentInChildren<TextMeshProUGUI>();   //This will Help Us To Take Text inside Tile. ie Get Component In children
    }

    public void SetLetter(char letter)      //Method Setletter in parameter char, this will set Letter(Property:16)
    {
        this.letter = letter;

        text.text = letter.ToString();      //we have created a var text(:20) and .text, and in line 27 it will getComponent and then we assigned letter which we get from user input
    }

    public void SetState(State state)       //Public Method in which state is parameter
    {
        this.state = state;                 

        fill.color = state.fillColor;       

        outline.effectColor = state.outlineColor;
    }

}
