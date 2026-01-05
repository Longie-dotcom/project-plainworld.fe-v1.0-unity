using Assets.Core;
using Assets.Service;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BinderOrchestrator : MonoBehaviour
{
    #region Attributes    
    private GameService gameService;

    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TMP_Text stepText;
    [SerializeField] private Image progressBar;

    private ComponentBinder[] currentBinders;
    #endregion

    #region Properties
    public static BinderOrchestrator Instance;
    #endregion

    #region Methods
    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Hide();
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Listen to all scene loads
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private IEnumerator Start()
    {
        // Wait for SceneService
        yield return new WaitUntil(() =>
            ServiceLocator.IsRegistered<GameService>() &&
            ServiceLocator.Get<GameService>().IsInitialized);

        gameService = ServiceLocator.Get<GameService>();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /// <summary>
    /// Called whenever a new scene is fully loaded
    /// </summary>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartLoading();
    }

    /// <summary>
    /// Show loading screen and start binding all binders in the scene.
    /// </summary>
    public void StartLoading()
    {
        Show();
        StartCoroutine(BindAllBinders());
    }

    private IEnumerator BindAllBinders()
    {
        // Find all binders in the current scene
        currentBinders = FindObjectsByType<ComponentBinder>(FindObjectsSortMode.None);

        int total = currentBinders.Length;
        int completed = 0;

        foreach (var binder in currentBinders)
        {
            // Show the step name
            SetStep(binder.StepName);

            // Wait for the binder to finish binding its services
            yield return binder.BindAllServices();

            completed++;
            SetProgress((float)completed / total);
        }

        // Loading done
        Hide();

        // Notify
        gameService.NotifySceneReady();
    }
    #endregion

    #region UI
    public void Show()
    {
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
    }

    public void Hide()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
    }

    public void SetStep(string step)
    {
        stepText.text = step;
    }

    public void SetProgress(float progress)
    {
        progressBar.fillAmount = Mathf.Clamp01(progress);
    }
    #endregion
}

