using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject menuPanel;
    public Button startButton;
    public Button continueButton;
    public Button loadButton;
    public Button galleryButton;
    public Button settingsButton;
    public Button quitButton;

    public AudioSource vocalAudio;

    private bool hasStarted = false;
    public static MenuManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        MenuButtonsAddListener();
    }
    void MenuButtonsAddListener()
    {
        //startButton.onClick.AddListener(StartGame);
        startButton.onClick.AddListener(ShowInputPanel);
        continueButton.onClick.AddListener(ContinueGame);
        loadButton.onClick.AddListener(LoadGame);
        galleryButton.onClick.AddListener(ShowGalleryPanel);
        settingsButton.onClick.AddListener(ShowSettingPanel);
        quitButton.onClick.AddListener(QuitGame);
    }
    void PlayVocalAudio(string audioFileName)
    {
        string audioPath = Constants.VOCAL_PATH + audioFileName;
        PlayAudio(audioPath, vocalAudio, false);
    }
    void PlayAudio(string audioPath, AudioSource audioSource, bool isLoop)
    {
        AudioClip audioClip = Resources.Load<AudioClip>(audioPath);
        if (audioClip != null)
        {
            audioSource.clip = audioClip;
            audioSource.gameObject.SetActive(true);
            audioSource.Play();
            audioSource.loop = isLoop;
        }
        else
        {
            Debug.LogError(Constants.AUDIO_LOAD_FAILED + audioPath);
        }
    }
    public void StartGame()
    {
        hasStarted = true;
        PlayVocalAudio(Constants.click);
        FVNManager.Instance.StartGame();
        ShowGamePanel();
    }
    private void ContinueGame()
    {
        PlayVocalAudio("click");
        if (hasStarted)
        {
            ShowGamePanel();
        }
    }
    private void LoadGame()
    {
        PlayVocalAudio("click");
        FVNManager.Instance.ShowLoadPanel(ShowGamePanel);
    }
    private void ShowInputPanel()
    {
        PlayVocalAudio("click");
        InputManager.Instance.ShowInputPanel();
    }
    private void ShowGamePanel()
    {
        menuPanel.SetActive(false);
        FVNManager.Instance.gamePanel.SetActive(true);
    }
    private void ShowGalleryPanel()
    {
        PlayVocalAudio("click");
        GalleryManager.Instance.ShowGalleryPanel();
    }
    private void ShowSettingPanel()
    {
        PlayVocalAudio("click");
        SettingManager.Instance.ShowSettingPanel();
    }
    private void QuitGame()
    {
        PlayVocalAudio("click");
        Application.Quit();
    }
}
