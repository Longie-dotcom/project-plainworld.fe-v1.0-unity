using Assets.Core;
using Assets.Service;
using Assets.Service.Enum;
using Assets.State.Interface.IReadOnlyState;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneOrchestrator : MonoBehaviour
{
    #region Attributes
    private GameService gameService;
    #endregion

    #region Properties
    #endregion

    #region Methods
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private IEnumerator Start()
    {
        // Wait for SceneService
        yield return new WaitUntil(() =>
            ServiceLocator.IsRegistered<GameService>() &&
            ServiceLocator.Get<GameService>().IsInitialized);

        gameService = ServiceLocator.Get<GameService>();
        gameService.GameState.OnRequestedNewScene += OnRequestedNewScene;
    }

    private void OnRequestedNewScene(IReadOnlyGameState state)
    {
        StopAllCoroutines();

        switch (state.PendingPhase)
        {
            case GamePhase.Login:
                StartCoroutine(LoadLobby());
                break;

            case GamePhase.InGame:
                StartCoroutine(LoadGame());
                break;
        }
    }

    private IEnumerator LoadLobby()
    {
        yield return SceneManager.LoadSceneAsync("LobbyScene", LoadSceneMode.Single);

        // Tell the game the world is ready
        var gameService = ServiceLocator.Get<GameService>();
    }

    private IEnumerator LoadGame()
    {
        yield return SceneManager.LoadSceneAsync("GameScene", LoadSceneMode.Single);

        // Tell the game the world is ready
        var gameService = ServiceLocator.Get<GameService>();
    }

    private void OnDestroy()
    {
        if (gameService != null)
            gameService.GameState.OnRequestedNewScene -= OnRequestedNewScene;
    }
    #endregion
}
