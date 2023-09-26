using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class Board : MonoBehaviour
{
    [SerializeField]
    GameObject canvasOne, canvasTwo, canvasThree;

    private static readonly KeyCode[] SUPPORTED_KEYS = new KeyCode[] {
        KeyCode.A, KeyCode.B, KeyCode.C, KeyCode.D, KeyCode.E, KeyCode.F,
        KeyCode.G, KeyCode.H, KeyCode.I, KeyCode.J, KeyCode.K, KeyCode.L,
        KeyCode.M, KeyCode.N, KeyCode.O, KeyCode.P, KeyCode.Q, KeyCode.R,
        KeyCode.S, KeyCode.T, KeyCode.U, KeyCode.V, KeyCode.W, KeyCode.X,
        KeyCode.Y, KeyCode.Z,
    };

    [SerializeField]
    TMP_Text hint;

    private Row[] rows;

    private int rowIndex; 
    private int columnIndex;

    private string[] solutions;
    private string[] validWords;
    private string word;

    [Header("Tiles")]
    public Tile.State emptyState;
    public Tile.State occupiedState;
    public Tile.State correctState;
    public Tile.State wrongSpotState;
    public Tile.State incorrectState;
    string str = "";


    private void Awake()
    {
        rows = GetComponentsInChildren<Row>();
    }

    private void Start()
    {
        LoadData();
        SetRandomWord();
    }

    private void LoadData()
    {
        TextAsset textFile = Resources.Load("WordleDicitionary") as TextAsset;//Create a TextAsset Variable in which we will store our wordle Dictionary. By Deafult unity have preserved Folder name "Resources in which we stored our txt file.
        solutions = textFile.text.Split("\n");  //Split() is basically plays a major role beacuse this will help us to separte the word in array of strings. If we Dont use Spilt(), It will read all word in our txt file.
                                                //(\n) New Line, thisis basically a parameter which will deicde where to divde from.So it will allow to take New word
                                                //If(we used comma Example hello, chair, then => we will choose ',' as a paramter, but we have to use(\n).

        textFile = Resources.Load("WrodleAnswerSheet") as TextAsset;
        validWords = textFile.text.Split("\n");
    }



    //imp
    private void SetRandomWord()
    {
        word = solutions[Random.Range(0, solutions.Length)];//This will basically pick a Randow 5-char Word from Range Start 0 to the Number of Words we are defined in it
        word = word.ToLower().Trim(); //ToLower() => this will convert word into lower case if any is fullCaps, Trim() => this will eat the blank space, if there is any
    }

    private void Update()
    {
        Row currentRow = rows[rowIndex];
        
        if (Input.GetKeyDown(KeyCode.Backspace))            //this will help us to  Delete Word
        {
            columnIndex = Mathf.Max(columnIndex - 1, 0);        //Mathf.Max... This will Decide between a Bigger Value and Smaller Value and From its name its clear it will choose a Bigger one, So... it will not go in Negative Value
                                                                //Not Using columnIndex-- because it will a probelm too

            currentRow.tiles[columnIndex].SetLetter('\0');  //SetLetter to Null: \0, ie char to null   property(Declare in Script:Tile:30)
            currentRow.tiles[columnIndex].SetState(emptyState);
            str = str.Remove(str.Length-1);

        }
        else if (columnIndex >= currentRow.tiles.Length)
        {
            if (Input.GetKeyDown(KeyCode.Return)) {
                SubmitRow(currentRow);
            }
        }
        else
        {
            for (int i = 0; i < SUPPORTED_KEYS.Length; i++)
            {
                if (Input.GetKeyDown(SUPPORTED_KEYS[i]))
                {
                    currentRow.tiles[columnIndex].SetLetter((char)SUPPORTED_KEYS[i]);       //We called a property(Declare in Script:Tile:30) with parameter....Which is declared in Runtime
                    currentRow.tiles[columnIndex].SetState(occupiedState);
                    str += (char)SUPPORTED_KEYS[i];
                    Debug.Log(str);
                    columnIndex++;                                                          //this will help us to shift from one column to another 
                    break;
                }
            }
        }
    }

    public void Replay()
    {
        SceneManager.LoadSceneAsync(0);
    }
    public void Hint()
    {
        var indexholder = columnIndex;
        if(indexholder>4)
        {
                indexholder = 4;
        }
        Debug.Log(indexholder);
        hint.text = "Try: " + word[indexholder];

    }
    private void SubmitRow(Row row)
    {

        string remaining = word; //to Store word which we created randomly in String remaining

        if (word == str)
        {
            for (int i = 0; i < row.tiles.Length; i++)
            {
                Tile tile = row.tiles[i];
                tile.SetState(correctState);
            }
            canvasTwo.gameObject.SetActive(true);
            return;
        }
        
        // check correct/incorrect letters first
        for (int i = 0; i < row.tiles.Length; i++)
        {
            Tile tile = row.tiles[i];
            //All-Correct State

            if (tile.letter == word[i])     //word[i] is accessing our String...
            {
                tile.SetState(correctState);
                remaining = remaining.Remove(i, 1);     //This is because we are removing one character
                remaining = remaining.Insert(i, " ");
            }

            //InCorrect State
            else if (!word.Contains(tile.letter))
            {
                tile.SetState(incorrectState);
            }
            else
            {
                //Incorrect
            }
        }

        // check wrong spots after
        for (int i = 0; i < row.tiles.Length; i++)
        {
            Tile tile = row.tiles[i];

            if (tile.state != correctState && tile.state != incorrectState)
            {
                if (remaining.Contains(tile.letter))
                {
                    tile.SetState(wrongSpotState);

                    int index = remaining.IndexOf(tile.letter);
                    remaining = remaining.Remove(index, 1);
                    remaining = remaining.Insert(index, " ");
                }
                else
                {
                    tile.SetState(incorrectState);
                }
            }
        }
        
        if(rowIndex==3)
        {
            canvasThree.gameObject.SetActive(true);
            Debug.Log(" you lost");//canvas ACTIVE
            return;
        }
        rowIndex++;
        columnIndex = 0;

        if (rowIndex >= rows.Length)
        {
            enabled = false;
        }
        str = "";
    }
}
