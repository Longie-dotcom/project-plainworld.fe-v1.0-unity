using UnityEngine;

namespace Assets.Core
{
    public class CoroutineRunner : MonoBehaviour
    {
        #region Attributes
        private static CoroutineRunner instance;
        #endregion

        #region Properties
        #endregion

        public static CoroutineRunner Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameObject("[CoroutineRunner]")
                        .AddComponent<CoroutineRunner>();

                    DontDestroyOnLoad(instance.gameObject);

                    // Initialize Global Error Handling
                    GlobalExceptionHandler.Initialize();
                }
                return instance;
            }
        }

        #region Methods
        #endregion
    }
}
