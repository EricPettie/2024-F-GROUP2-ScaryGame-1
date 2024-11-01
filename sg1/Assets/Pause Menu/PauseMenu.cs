using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject optionsCanvas; // Reference to the existing OptionsCanvas in the hierarchy
    public GameObject overlayCanvas; // Reference to the Overlay canvas
    public GameObject player; // Reference to the player GameObject
    public Button resumeButton;
    public Button optionsButton;
    public Button backToTitleButton;
    public Button quitButton; // Reference to the Quit button
    private bool isPaused = false;
    private Vector3 savedPosition;
    private Quaternion savedRotation;

    private OptionsMenuTitle optionsMenuTitle; // Reference to the OptionsMenuTitle script

    void Start()
    {
        resumeButton.onClick.AddListener(Resume);
        optionsButton.onClick.AddListener(OpenOptions);
        backToTitleButton.onClick.AddListener(BackToTitle);
        quitButton.onClick.AddListener(QuitGame); // Add listener for Quit button

        pauseMenuUI.SetActive(false);

        // Find the OptionsMenuTitle script and assign it
        optionsMenuTitle = GetComponent<OptionsMenuTitle>();

        if (optionsMenuTitle != null)
        {
            // Set the references for the OptionsMenuTitle script
            optionsMenuTitle.optionsCanvas = optionsCanvas;
            optionsMenuTitle.mainPanel = optionsCanvas.transform.Find("MainPanel").gameObject;
            optionsMenuTitle.sensitivityPanel = optionsCanvas.transform.Find("SensitivityPanel").gameObject;
            optionsMenuTitle.sensitivityButton = optionsMenuTitle.mainPanel.transform.Find("SensitivityButton").GetComponent<Button>();
            optionsMenuTitle.backButtonMainPanel = optionsMenuTitle.mainPanel.transform.Find("BackButton").GetComponent<Button>();
            optionsMenuTitle.sensitivityXSlider = optionsMenuTitle.sensitivityPanel.transform.Find("Xslider").GetComponent<Slider>();
            optionsMenuTitle.sensitivityYSlider = optionsMenuTitle.sensitivityPanel.transform.Find("Yslider").GetComponent<Slider>();
            optionsMenuTitle.sensitivityXInputField = optionsMenuTitle.sensitivityPanel.transform.Find("XInputField (Legacy)").GetComponent<InputField>();
            optionsMenuTitle.sensitivityYInputField = optionsMenuTitle.sensitivityPanel.transform.Find("YInputField (Legacy)").GetComponent<InputField>();
            optionsMenuTitle.sensitivityApplyButton = optionsMenuTitle.sensitivityPanel.transform.Find("SensApplyButton").GetComponent<Button>();
            optionsMenuTitle.sensitivitySetToDefaultButton = optionsMenuTitle.sensitivityPanel.transform.Find("SensResetButton").GetComponent<Button>();
            optionsMenuTitle.backButtonSensitivityPanel = optionsMenuTitle.sensitivityPanel.transform.Find("SensBackButton").GetComponent<Button>();

            optionsMenuTitle.Initialize(); // Call Initialize instead of Start
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    void Resume()
    {
        pauseMenuUI.SetActive(false);
        overlayCanvas.SetActive(true); // Show Overlay canvas
        EnablePlayerControls(true); // Enable player controls
        Time.timeScale = 1f;
        isPaused = false;

        // Restore the player's position and rotation
        player.transform.position = savedPosition;
        player.transform.rotation = savedRotation;

        // Lock and hide the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Hide the options canvas
        optionsCanvas.SetActive(false);
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        overlayCanvas.SetActive(false); // Hide Overlay canvas
        EnablePlayerControls(false); // Disable player controls
        Time.timeScale = 0f;
        isPaused = true;

        // Save the player's position and rotation
        savedPosition = player.transform.position;
        savedRotation = player.transform.rotation;

        // Freeze the player's position and rotation
        player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;

        // Unlock and show the cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void OpenOptions()
    {
        // Toggle options canvas visibility
        bool isActive = !optionsCanvas.activeSelf;
        optionsCanvas.SetActive(isActive);

        if (isActive)
        {
            // Ensure the main panel is active
            optionsMenuTitle.mainPanel.SetActive(true);
            // Optionally, ensure other panels are set to their initial states
            optionsMenuTitle.sensitivityPanel.SetActive(false);
        }
    }

    void BackToTitle()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("TitleScreen");
    }

    void QuitGame()
    {
        Application.Quit();
    }

    void EnablePlayerControls(bool enable)
    {
        // The player has a script named "Movement" that controls the camera and movement
        player.GetComponent<Movement>().enabled = enable;

        if (enable)
        {
            player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        }
        else
        {
            player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
        }
    }
}
