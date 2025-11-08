using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public GameObject inputPanel;
    public TextMeshProUGUI promptText;
    public TMP_InputField nameInputField;
    public Button confirmButton;
    public Button cancelButton;
    public AudioSource vocalAudio;
    public static InputManager Instance { get; private set; }
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
    void Start()
    {
        confirmButton.GetComponentInChildren<TextMeshProUGUI>().text = Constants.CONFIRM;
        confirmButton.onClick.AddListener(OnConfirm);
        cancelButton.GetComponentInChildren<TextMeshProUGUI>().text = Constants.CANCEL;
        cancelButton.onClick.AddListener(OnCancel);
        inputPanel.SetActive(false);
    }
    void OnConfirm()
    {
        PlayVocalAudio(Constants.click);
        string playerName = nameInputField.text.Trim();
        if (IsInvalidName(playerName))
        {
            //error
            return;
        }
        PlayerData.Instance.playerName = playerName;
        inputPanel.SetActive(false);
        MenuManager.Instance.StartGame();
    }
    void OnCancel()
    {
        PlayVocalAudio(Constants.click);
        inputPanel.SetActive(false);
    }
    bool IsInvalidName(string name)
    {
        return string.IsNullOrEmpty(name);
    }
    public void ShowInputPanel()
    {
        promptText.text = Constants.PROMPT_TEXT;
        nameInputField.text = "";
        inputPanel.SetActive(true);
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
}
