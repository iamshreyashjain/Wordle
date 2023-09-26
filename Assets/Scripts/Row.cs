using UnityEngine;

public class Row : MonoBehaviour
{
    public Tile[] tiles { get; private set; }       //Property 

    public string word              //this property help us to form a word to check weather it is a valid word or not
    {
        get
        {
            string word = "";

            for (int i = 0; i < tiles.Length; i++) {
                word += tiles[i].letter;
            }

            return word;
        }
    }

    private void Awake()
    {
        tiles = GetComponentsInChildren<Tile>();        //this will help to take components inside TILES children
    }

}
