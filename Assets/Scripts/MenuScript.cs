using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class MenuScript : MonoBehaviour {

    public Canvas quitMenu;
    public Button startButton;
    public Button exitButton;
    public Toggle generateRandomSeed;
    public InputField userInputSeed;
    private string CharacterLimit = "32characterLimit32characterLimit";
    public string SeedFile;

    // Use this for initialization
    void Start () {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        quitMenu = quitMenu.GetComponent<Canvas>();
        startButton = startButton.GetComponent<Button>();
        exitButton = exitButton.GetComponent<Button>();
        generateRandomSeed = generateRandomSeed.GetComponent<Toggle>();
        userInputSeed.characterLimit = CharacterLimit.Length;
        quitMenu.enabled = false;
        File.Delete("seedfile.txt");
	
	}


    public void ExitPress()
    {
        quitMenu.enabled = true;
        startButton.enabled = false;
        exitButton.enabled = false;
    }

    public void NoPress()
    {
        quitMenu.enabled = false;
        startButton.enabled = true;
        exitButton.enabled = true;
    }

    /// <summary>
    /// When the level is started, this function takes the
    /// information from the input object and toggle object.
    /// This is then used to write a file, this process is done in
    /// the FileWorker Script
    /// </summary>
    public void UserSeedInput()
    {
        if (!generateRandomSeed.isOn)
            FileWorker.WriteSeed(userInputSeed.text);
        else
            FileWorker.RemoveSeed();
    }

    public void LevelStart()
    {
        SceneManager.LoadScene(1);
    }

 

    public void ExitGame()
    {
        Application.Quit();
        File.Delete("seedfile.txt");

    }

	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
            ExitPress();
    }
}
