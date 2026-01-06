using UnityEngine;
using UnityEngine.UI;

public class LaneDodgeManager : MonoBehaviour
{
    [Header("UI Panel")]
    public GameObject panel;
    public CanvasGroup canvasGroup;

    [Header("Refs")]
    public LaneDodgeSpawnerUI spawner;
    public LaneDodgePlayerUI player;

    [Header("UI In-Game")]
    public Text scoreText;

    [Header("UI Game Over")]
    public GameObject gameOverPanel;
    public Text finalScoreText;
    public Button restartButton;
    public Button exitButton;  // <-- to jest "Wyjœcie" na ekranie wyniku (i mo¿e te¿ dzia³aæ w trakcie)

    [Header("Gameplay")]
    public bool pauseWorldTime = true;
    public float startDelay = 0.5f;

    private bool isRunning = false;
    private bool isGameOver = false;
    private int  score = 0;
    private float startTimer = 0f;

    // ? Automatycznie znalezione skrypty Waltera (bez podpinania w Inspectorze)
    private WalterBlackPanel walterPanel;
    private WalterBlackPanelUI walterPanelUI;

    private void Awake()
    {
        // true => znajdzie te¿ obiekty nieaktywne
        walterPanel = FindObjectOfType<WalterBlackPanel>(true);
        walterPanelUI = FindObjectOfType<WalterBlackPanelUI>(true);
    }

    private void Start()
    {
        HideAll();

        if (restartButton != null)
        {
            restartButton.onClick.RemoveAllListeners();
            restartButton.onClick.AddListener(RestartGame);
        }

        if (exitButton != null)
        {
            exitButton.onClick.RemoveAllListeners();
            exitButton.onClick.AddListener(ExitToWalterMenu); // ? zamiast CloseLaneDodgeGame()
        }

        if (spawner != null) spawner.manager = this;
        if (player != null) player.manager = this;
    }

    public void OpenLaneDodgeGame()
    {
        Show();

        if (pauseWorldTime) Time.timeScale = 0f;

        score = 0;
        isGameOver = false;
        isRunning = true;

        startTimer = startDelay;

        UpdateScore();
        ShowGameOver(false);

        if (spawner != null) spawner.ResetSpawner();
        if (player != null) player.ResetPlayer();
    }

    public void RestartGame()
    {
        score = 0;
        isGameOver = false;
        isRunning = true;

        startTimer = startDelay;

        UpdateScore();
        ShowGameOver(false);

        if (spawner != null) spawner.ResetSpawner();
        if (player != null) player.ResetPlayer();

        if (pauseWorldTime) Time.timeScale = 0f;
    }

    public void ExitToWalterMenu()
    {
        // 1) zamykamy minigrê (UI)
        isRunning = false;
        isGameOver = false;
        HideAll();

        // 2) zostajemy w "pauzie", bo wracamy do menu Waltera
        Time.timeScale = 0f;

        // 3) poka¿ menu Waltera (bez ¿adnego podpinania w inspectorze)
        if (walterPanel != null)
        {
            walterPanel.OpenMenu(false); // menu ON, timeScale nie ruszamy (ju¿ 0)
            return;
        }

        if (walterPanelUI != null)
        {
            walterPanelUI.OpenMenu(false);
            return;
        }

        // fallback: jak nie znaleziono Waltera, wróæ do gry (¿ebyœ nie utkn¹³)
        Debug.LogWarning("Nie znaleziono WalterBlackPanel/WalterBlackPanelUI w scenie. Wracam do gry.");
        Time.timeScale = 1f;
    }

    private void Update()
    {
        if (!isRunning) return;
        if (isGameOver) return;

        if (startTimer > 0f)
        {
            startTimer -= Time.unscaledDeltaTime;
            return;
        }

        if (spawner != null) spawner.Tick(Time.unscaledDeltaTime);
    }

    public void AddScore(int value)
    {
        if (!isRunning || isGameOver) return;

        score += value;
        UpdateScore();

        // przyspieszenie za punkty
        if (spawner != null) spawner.OnScoreGained(value);
    }

    public void GameOver()
    {
        if (isGameOver) return;

        MetaProgress.TrySetLaneBest(score);
        isGameOver = true;
        isRunning = false;

        if (pauseWorldTime) Time.timeScale = 0f;

        if (finalScoreText != null) finalScoreText.text = $"Wynik: {score}";
        ShowGameOver(true);
    }

    private void UpdateScore()
    {
        if (scoreText != null) scoreText.text = $"Score: {score}";
    }

    private void Show()
    {
        if (panel != null) panel.SetActive(true);
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void HideAll()
    {
        ShowGameOver(false);

        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }

        if (panel != null) panel.SetActive(false);
    }

    private void ShowGameOver(bool show)
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(show);
    }
}
