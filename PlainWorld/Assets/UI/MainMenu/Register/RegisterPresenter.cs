using Assets.Service;
using Assets.UI.Enum;
using Assets.Utility;
using System;

namespace Assets.UI.MainMenu.Register
{
    public class RegisterPresenter : IDisposable
    {
        #region Attributes
        private readonly AuthService authService;
        private readonly UIService uiService;
        private readonly GameService gameService;

        private RegisterView registerView;
        private bool disposed;

        private string email;
        private string password;
        private string fullName;
        private string dob;
        private string gender;
        #endregion

        #region Properties
        #endregion

        public RegisterPresenter(
            AuthService authService, 
            UIService uiService,
            GameService gameService)
        {
            this.authService = authService;
            this.uiService = uiService;
            this.gameService = gameService;
        }

        #region Methods
        public void Bind(RegisterView registerView)
        {
            if (disposed)
                throw new ObjectDisposedException(nameof(RegisterPresenter));

            this.registerView = registerView;

            registerView.OnEmailChanged += OnEmailChanged;
            registerView.OnPasswordChanged += OnPasswordChanged;
            registerView.OnFullNameChanged += OnFullNameChanged;
            registerView.OnDobChanged += OnDobChanged;
            registerView.OnRegisterClicked += OnRegister;
            registerView.OnFemaleClicked += OnFemaleChanged;
            registerView.OnMaleClicked += OnMaleChanged;

            uiService.UIState.OnUIStateChanged += registerView.HandleUIState;
        }

        public void Dispose()
        {
            if (disposed) return;
            disposed = true;

            registerView.OnEmailChanged -= OnEmailChanged;
            registerView.OnPasswordChanged -= OnPasswordChanged;
            registerView.OnFullNameChanged -= OnFullNameChanged;
            registerView.OnDobChanged -= OnDobChanged;
            registerView.OnRegisterClicked -= OnRegister;
            registerView.OnFemaleClicked -= OnFemaleChanged;
            registerView.OnMaleClicked -= OnMaleChanged;

            uiService.UIState.OnUIStateChanged -= registerView.HandleUIState;
        }

        private void OnEmailChanged(string v)
        {
            email = v;
        }

        private void OnPasswordChanged(string v)
        {
            password = v;
        }

        private void OnFullNameChanged(string v)
        {
            fullName = v;
        }

        private void OnDobChanged(string v)
        {
            dob = v;
        }

        private void OnFemaleChanged()
        {
            gender = Gender.Female;
        }

        private void OnMaleChanged()
        {
            gender = Gender.Male;
        }

        private void OnRegister()
        {
            AsyncHelper.Run(() => authService.Register(
                email, 
                password,
                fullName,
                dob,
                gender));
        }
        #endregion
    }
}
