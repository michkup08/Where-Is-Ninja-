using UnityEngine;
using UnityEngine.UI;

public class WalterBlackPanelUI : MonoBehaviour
{
    [Header("UI References")]
    public GameObject menuPanel;
    public CanvasGroup menuCanvasGroup;

    [Header("Minigry")]
    public MemoryManager memoryMinigame;
    public SpamManager spamMinigame;
    public LaneDodgeManager laneDodgeMinigame;   // <--- NOWE (3 pasy)

    private bool isOpen = false;
    private AttacksManager attacksManager;
    private bool attacksActive = true;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        attacksManager = FindObjectOfType<AttacksManager>();

        if (menuPanel != null)
        {
            menuPanel.SetActive(false);

            // Back
            Transform backBtnTr = menuPanel.transform.Find("Back_BTN");
            if (backBtnTr != null)
                backBtnTr.GetComponent<Button>().onClick.AddListener(ResumeGameButton);

            // Bron 1 -> Memory
            Transform bron1Tr = menuPanel.transform.Find("Bron1_BTN");
            if (bron1Tr != null)
            {
                Button btn = bron1Tr.GetComponent<Button>();
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(UruchomMemory);
            }

            // Bron 2 -> Spam
            Transform bron2Tr = menuPanel.transform.Find("Bron2_BTN");
            if (bron2Tr != null)
            {
                Button btn = bron2Tr.GetComponent<Button>();
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(UruchomSpam);
            }

            // Bron 3 -> Lane Dodge (3 pasy)
            Transform bron3Tr = menuPanel.transform.Find("Bron3_BTN");
            if (bron3Tr != null)
            {
                Button btn = bron3Tr.GetComponent<Button>();
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(UruchomLaneDodge);
            }
        }

        if (menuCanvasGroup != null)
        {
            menuCanvasGroup.alpha = 0f;
            menuCanvasGroup.interactable = false;
            menuCanvasGroup.blocksRaycasts = false;
        }
    }

    public void UruchomMemory()
    {
        HideWalterUI();
        if (memoryMinigame != null) memoryMinigame.OpenMemoryGame();
        else Debug.LogError("Brak przypisanego MemoryManager!");
    }

    public void UruchomSpam()
    {
        HideWalterUI();
        if (spamMinigame != null) spamMinigame.OpenSpamGame();
        else Debug.LogError("Brak przypisanego SpamManager!");
    }

    public void UruchomLaneDodge()
    {
        HideWalterUI();
        if (laneDodgeMinigame != null) laneDodgeMinigame.OpenLaneDodgeGame();
        else Debug.LogError("Brak przypisanego LaneDodgeManager!");
    }

    private void HideWalterUI()
    {
        if (menuCanvasGroup != null)
        {
            menuCanvasGroup.alpha = 0f;
            menuCanvasGroup.interactable = false;
            menuCanvasGroup.blocksRaycasts = false;
        }
        if (menuPanel != null) menuPanel.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isOpen) return;
        if (other.CompareTag("Player")) OpenMenu(true);
    }

    public void OpenMenu(bool pauseTime)
    {
        if (menuPanel == null) return;

        menuPanel.SetActive(true);
        if (pauseTime) Time.timeScale = 0f;
        isOpen = true;

        if (menuCanvasGroup != null)
        {
            menuCanvasGroup.alpha = 1f;
            menuCanvasGroup.interactable = true;
            menuCanvasGroup.blocksRaycasts = true;
        }

        if (attacksManager != null)
        {
            attacksManager.enabled = false;
            attacksActive = false;
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGameButton()
    {
        Time.timeScale = 1f;
        HideWalterUI();
        isOpen = false;

        if (attacksManager != null && !attacksActive)
        {
            attacksManager.enabled = true;
            attacksActive = true;
        }
    }
}
