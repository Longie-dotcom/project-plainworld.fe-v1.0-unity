using Assets.Network.NetworkException;
using Assets.Service;
using Assets.State;
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
        private readonly RegisterView registerView;

        private bool disposed;
        private bool isEmailValid;
        private bool isPasswordValid;
        private bool isNameValid;
        private bool isGenderValid;
        private bool isDobValid;

        private string email;
        private string password;
        private string fullName;
        private string dob;
        private string day;
        private string month;
        private string year;
        private string gender;
        #endregion

        #region Properties
        #endregion

        public RegisterPresenter(
            AuthService authService, 
            UIService uiService,
            GameService gameService,
            RegisterView registerView)
        {
            this.authService = authService;
            this.uiService = uiService;
            this.gameService = gameService;
            this.registerView = registerView;

            Bind();
        }

        #region Methods
        public void Dispose()
        {
            if (disposed) return;
            disposed = true;

            registerView.OnEmailChanged -= OnEmailChanged;
            registerView.OnPasswordChanged -= OnPasswordChanged;
            registerView.OnFullNameChanged -= OnFullNameChanged;
            registerView.OnDayDobChanged -= OnDayDobChanged;
            registerView.OnMonthDobChanged -= OnMonthDobChanged;
            registerView.OnYearDobChanged -= OnYearDobChanged;
            registerView.OnBackClicked -= OnBack;
            registerView.OnRegisterClicked -= OnRegister;
            registerView.OnFemaleClicked -= OnFemaleChanged;
            registerView.OnMaleClicked -= OnMaleChanged;

            uiService.UIState.OnUIStateChanged -= registerView.HandleUIState;
        }

        private void Bind()
        {
            if (disposed)
                throw new ObjectDisposedException(nameof(RegisterPresenter));

            registerView.OnEmailChanged += OnEmailChanged;
            registerView.OnPasswordChanged += OnPasswordChanged;
            registerView.OnFullNameChanged += OnFullNameChanged;
            registerView.OnDayDobChanged += OnDayDobChanged;
            registerView.OnMonthDobChanged += OnMonthDobChanged;
            registerView.OnYearDobChanged += OnYearDobChanged;
            registerView.OnBackClicked += OnBack;
            registerView.OnRegisterClicked += OnRegister;
            registerView.OnFemaleClicked += OnFemaleChanged;
            registerView.OnMaleClicked += OnMaleChanged;

            uiService.UIState.OnUIStateChanged += registerView.HandleUIState;
        }

        private void OnEmailChanged(string v)
        {
            email = v;
            ValidateEmail();
            UpdateRegisterButton();
        }

        private void OnPasswordChanged(string v)
        {
            password = v;
            ValidatePassword();
            UpdateRegisterButton();
        }

        private void OnFullNameChanged(string v)
        {
            fullName = v;
            ValidateFullName();
            UpdateRegisterButton();
        }

        private void OnFemaleChanged()
        {
            gender = Gender.Female;
            ValidateGender();
            UpdateRegisterButton();
        }

        private void OnMaleChanged()
        {
            gender = Gender.Male;
            ValidateGender();
            UpdateRegisterButton();
        }

        private void OnDayDobChanged(string v)
        {
            day = v;
            ValidateDob();
            UpdateRegisterButton();
        }

        private void OnMonthDobChanged(string v)
        {
            month = v;
            ValidateDob();
            UpdateRegisterButton();
        }

        private void OnYearDobChanged(string v)
        {
            year = v;
            ValidateDob();
            UpdateRegisterButton();
        }

        private void OnBack()
        {
            gameService.GameState.SetPhase(GamePhase.Login);
        }

        private void OnRegister()
        {
            AsyncHelper.Run(async () =>
            {
                try
                {
                    // Request registration
                    await authService.Register(
                        email,
                        password,
                        fullName,
                        gender,
                        dob
                    );

                    // Show success and return to login view
                    uiService.UIState.ShowPopUp(
                        PopUpType.Information,
                        "Registration successful!"
                    );
                    gameService.GameState.SetPhase(GamePhase.Login);
                }
                catch (AuthException ex)
                {
                    uiService.UIState.ShowPopUp(
                        PopUpType.Error,
                        ex.Message
                    );
                }
                catch (Exception)
                {
                    uiService.UIState.ShowPopUp(
                        PopUpType.Error,
                        "Unexpected error. Please try again."
                    );
                }
            });
        }
        #endregion

        #region Private Helpers
        private void UpdateRegisterButton()
        {
            bool canRegister =
                isEmailValid &&
                isPasswordValid &&
                isNameValid &&
                isDobValid &&
                isGenderValid;

            registerView.SetRegisterInteractable(canRegister);
        }

        private void ValidateEmail()
        {
            isEmailValid = !string.IsNullOrWhiteSpace(email) && email.Contains("@");
            registerView.SetEmailValid(isEmailValid);
        }

        private void ValidatePassword()
        {
            isPasswordValid = !string.IsNullOrWhiteSpace(password) && password.Length >= 6;
            registerView.SetPasswordValid(isPasswordValid);
        }

        private void ValidateFullName()
        {
            isNameValid = !string.IsNullOrWhiteSpace(fullName);
            registerView.SetNameValid(isNameValid);
        }

        private void ValidateGender()
        {
            isGenderValid = true;
            registerView.SetGenderValid(true);
        }

        private void ValidateDob()
        {
            bool parsedDay = int.TryParse(day, out int d);
            bool parsedMonth = int.TryParse(month, out int m);
            bool parsedYear = int.TryParse(year, out int y);

            isDobValid =
                parsedDay &&
                parsedMonth &&
                parsedYear &&
                IsValidDate(d, m, y);

            if (isDobValid)
            {
                var date = new DateTime(y, m, d, 0, 0, 0, DateTimeKind.Utc);
                dob = date.ToString("o");
            }
            else
            {
                dob = null;
            }

            registerView.SetDobValid(isDobValid);
        }

        private bool IsValidDate(int day, int month, int year)
        {
            if (year < 1900 || year > DateTime.Now.Year)
                return false;

            try
            {
                _ = new DateTime(year, month, day);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion
    }
}
