using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GalleryManager : MonoBehaviour
{
    public GameObject galleryPanel;
    public TextMeshProUGUI panelTitle;
    public Button[] galleryButtons;
    public Button prevPageButton;
    public Button nextPageButton;
    public Button backButton;
    public GameObject bigImagePanel;
    public Image bigImage;

    public AudioSource vocalAudio;

    private int currentPage = Constants.DEFAULT_START_INDEX;
    private readonly int slotsPerPage = Constants.GALLERY_SLOTS_PER_PAGE;
    private readonly int totalSlots = Constants.ALL_BACKGROUNDS.Length;
    public static GalleryManager Instance { get; private set; }
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
    // Start is called before the fiest frame update
    private void Start()
    {
        prevPageButton.onClick.AddListener(PrevPage);
        nextPageButton.onClick.AddListener(NextPage);
        backButton.onClick.AddListener(GoBack);
        galleryPanel.SetActive(false);
        panelTitle.text = Constants.GALLERY;

        bigImagePanel.SetActive(false);
        Button bigImageButton = bigImagePanel.GetComponent<Button>();
        if (bigImageButton != null)
        {
            bigImageButton.onClick.AddListener(HideBigImage);
        }
        else
        {
            Debug.LogWarning("BigImagePanelÉÏµÄButtonÄØ?");
        }
    }
    public void ShowGalleryPanel()
    {
        UpdateUI();
        galleryPanel.SetActive(true);
    }
    private void UpdateUI()
    {
        for (int i = 0; i < slotsPerPage; ++i)
        {
            int slotIndex = currentPage * slotsPerPage + i;
            if (slotIndex < totalSlots)
            {
                UpdateGalleryButtons(galleryButtons[i], slotIndex);
            }
            else
            {
                galleryButtons[i].gameObject.SetActive(false);
            }
        }
    }
    private void UpdateGalleryButtons(Button button, int index)
    {
        button.gameObject.SetActive(true);
        button.interactable = true;
        string bgName = Constants.ALL_BACKGROUNDS[index];
        bool isUnlocked = FVNManager.Instance.unlockedBackgrounds.Contains(bgName);
        string imagePath = Constants.THUMBNAIL_PATH + (isUnlocked ? bgName : Constants.GALLERY_PLACEHOLDER);
        Sprite sprite = Resources.Load<Sprite>(imagePath);
        if (sprite != null)
        {
            button.GetComponentInChildren<Image>().sprite = sprite;
        }
        else
        {
            Debug.LogError(Constants.IMAGE_LOAD_FAILED + imagePath);
        }
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => OnButtonClick(button, index));
    }
    private void OnButtonClick(Button button, int index)
    {
        PlayVocalAudio(Constants.click);
        string bgName = Constants.ALL_BACKGROUNDS[index];
        bool isUnlocked = FVNManager.Instance.unlockedBackgrounds.Contains(bgName);

        if (!isUnlocked)
        {
            return;
        }

        string imagePath = Constants.BACKGROUND_PATH + bgName;
        Sprite sprite = Resources.Load<Sprite>(imagePath);
        if (sprite != null)
        {
            bigImage.sprite = sprite;
            bigImagePanel.SetActive(true);
        }
        else
        {
            Debug.LogError(Constants.BIG_IMAGE_LOAD_FAILED + imagePath);
        }
    }
    private void HideBigImage()
    {
        PlayVocalAudio(Constants.click);
        bigImagePanel.SetActive(false);
    }
    private void PrevPage()
    {
        PlayVocalAudio(Constants.click);
        if (currentPage > 0)
        {
            currentPage--;
            UpdateUI();
        }
    }

    private void NextPage()
    {
        PlayVocalAudio(Constants.click);
        if ((currentPage + 1) * slotsPerPage < totalSlots)
        {
            currentPage++;
            UpdateUI();
        }
    }
    private void GoBack()
    {
        PlayVocalAudio(Constants.click);
        galleryPanel.SetActive(false);
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
