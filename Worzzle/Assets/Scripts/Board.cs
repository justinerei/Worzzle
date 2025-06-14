using System.Linq;
using TMPro;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    private static readonly KeyCode[] SUPPORTED_KEYS = new KeyCode[] {
        KeyCode.A, KeyCode.B, KeyCode.C, KeyCode.D, KeyCode.E, KeyCode.F, KeyCode.G,
        KeyCode.H, KeyCode.I, KeyCode.J, KeyCode.K, KeyCode.L, KeyCode.M, KeyCode.N,
        KeyCode.O, KeyCode.P, KeyCode.Q, KeyCode.R, KeyCode.S, KeyCode.T, KeyCode.U,
        KeyCode.V, KeyCode.W, KeyCode.X, KeyCode.Y, KeyCode.Z,
    };

    private Row[] rows;


    [SerializeField] private TextAsset englishSolutionsFile;
    [SerializeField] private TextAsset englishValidFile;
    [SerializeField] private TextAsset filipinoSolutionsFile;
    [SerializeField] private TextAsset filipinoValidFile;
    private string[] englishSolutions;
    private string[] englishValid;
    private string[] filipinoSolutions;
    private string[] filipinoValid;
    private string[] solutions;
    private string[] validWords;
    private string word;

    public Button languageToggleButton;
    public Sprite filipinoSprite;
    public Sprite englishSprite;
    private bool isFilipino = false;


    private int rowIndex;
    private int columnIndex;

    public ParticleSystem confetti;
    public ParticleSystem confettiBurst;


    [Header("States")]
    public Tile.State emptyState;
    public Tile.State occupiedState;
    public Tile.State correctState;
    public Tile.State wrongSpotState;
    public Tile.State incorrectState;

    [Header("UI")]
    public TextMeshProUGUI invalidWordText;
    public Button newWordButton;
    public Button tryAgainButton;
    public Button helpButton;
    public Button musicButton;
    public Button filButton;

    AudioManager audioManager;


    private void Awake()
    {
        rows = GetComponentsInChildren<Row>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void Start()
    {
        LoadData();
        NewGame();
    }

    public void NewGame()
    {
        ClearBoard();
        SetRandomWord();

        enabled = true;
    }

    public void TryAgain()
    {
        ClearBoard();

        enabled = true;
    }

    private void LoadData()
    {
        englishValid = englishValidFile.text
            .Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries)
            .Select(w => w.Trim().ToLower())
            .ToArray();

        englishSolutions = englishSolutionsFile.text
            .Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries)
            .Select(w => w.Trim().ToLower())
            .ToArray();

        filipinoValid = filipinoValidFile.text
            .Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries)
            .Select(w => w.Trim().ToLower())
            .ToArray();

        filipinoSolutions = filipinoSolutionsFile.text
            .Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries)
            .Select(w => w.Trim().ToLower())
            .ToArray();

        validWords = englishValid;
        solutions = englishSolutions;
    }

    private void SetRandomWord()
    {
        word = solutions[Random.Range(0, solutions.Length)];
        word = word.ToLower().Trim();
        Debug.Log("Correct Word: " + word);
    }

    private void Update()
    {
        Row currentRow = rows[rowIndex];

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            audioManager.PlaySFX(audioManager.backspace);
            columnIndex = Mathf.Max(columnIndex - 1, 0);
            currentRow.tiles[columnIndex].SetLetter('\0');
            currentRow.tiles[columnIndex].SetState(emptyState);
            invalidWordText.gameObject.SetActive(false);
        }
        else if (columnIndex >= currentRow.tiles.Length)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                audioManager.PlaySFX(audioManager.enter);
                SubmitRow(currentRow);
            }
        }
        else
        {
            for (int i = 0; i < SUPPORTED_KEYS.Length; i++)
            {
                if (Input.GetKeyDown(SUPPORTED_KEYS[i]))
                {
                    audioManager.PlaySFX(audioManager.softType);
                    currentRow.tiles[columnIndex].SetLetter((char)SUPPORTED_KEYS[i]);
                    currentRow.tiles[columnIndex].SetState(occupiedState);
                    columnIndex++;
                    break;
                }
            }
        }
    }

    private void SubmitRow(Row row)
    {
        if (!IsValidWord(row.word))
        {
            invalidWordText.gameObject.SetActive(true);
            audioManager.PlaySFX(audioManager.invalidWord);
            GetComponent<UIShaker>()?.Shake();
            return;
        }

        string remaining = word;

        // Check correct/incorrect letters first
        for (int i = 0; i < row.tiles.Length; i++)
        {
            Tile tile = row.tiles[i];

            if (tile.letter == word[i])
            {
                tile.SetState(correctState);

                remaining = remaining.Remove(i, 1);
                remaining = remaining.Insert(i, " ");
            }
            else if (!word.Contains(tile.letter))
            {
                tile.SetState(incorrectState);
            }
        }

        // Check wrong spots after
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

        if (HasWon(row))
        {
            if (confetti != null)
            {
                confetti.transform.position = new Vector3(0, 3, 6); // Adjust as needed
                confettiBurst.Play();
            }

            audioManager.PlaySFX(audioManager.winState);
            enabled = false;
            return;
        }

        rowIndex++;
        columnIndex = 0;

        if (rowIndex >= rows.Length)
        {
            audioManager.PlaySFX(audioManager.loseState);
            FindObjectOfType<ScreenFader>().FadeToRed();
            enabled = false;
        }    
    }

    private void ClearBoard()
    {
        for (int row = 0; row < rows.Length; row++)
        {
            for (int col = 0; col < rows[row].tiles.Length; col++)
            {
                rows[row].tiles[col].SetLetter('\0');
                rows[row].tiles[col].SetState(emptyState);
            }
        }

        rowIndex = 0;
        columnIndex = 0;
    }

    private bool IsValidWord(string word)
    {
        for (int i = 0; i < validWords.Length; i++)
        {
            if (string.Equals(word, validWords[i], System.StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }

    private bool HasWon (Row row)
    {
        for (int i = 0; i < row.tiles.Length; i++)
        {
            if (row.tiles[i].state != correctState)
            {
                return false;
            }
        }

        return true;
    }

    private void OnEnable()
    {
        tryAgainButton.gameObject.SetActive(false);
        newWordButton.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        tryAgainButton.gameObject.SetActive(true);
        newWordButton.gameObject.SetActive(true);
    }

    public void ToggleLanguage()
    {
        isFilipino = !isFilipino;

        if (isFilipino)
        {
            validWords = filipinoValid;
            solutions = filipinoSolutions;
            languageToggleButton.image.sprite = englishSprite; // shows ENG
        }
        else
        {
            validWords = englishValid;
            solutions = englishSolutions;
            languageToggleButton.image.sprite = filipinoSprite; // shows FIL
        }

        NewGame(); // restart game with new language
    }

}
