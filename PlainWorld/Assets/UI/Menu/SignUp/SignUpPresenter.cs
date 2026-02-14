using Assets.Network.NetworkException;
using Assets.Service;
using Assets.UI.Enum;
using Assets.Utility;
using System;

namespace Assets.UI.Menu.SignUp
{
    public class SignUpPresenter : IDisposable
    {
        #region Attributes
        private readonly UIService uiService;
        private readonly GameService gameService;
        private readonly AuthService authService;
        private readonly SignUpView signUpView;

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

        private bool disposed;
        #endregion

        #region Properties
        #endregion

        public SignUpPresenter(
            UIService uiService,
            GameService gameService,
            AuthService authService,
            SignUpView signUpView)
        {
            this.uiService = uiService;
            this.gameService = gameService;
            this.authService = authService;
            this.signUpView = signUpView;

            Bind();
        }

        #region Methods
        public void Dispose()
        {
            if (disposed) return;
            disposed = true;

            // Inbound
            signUpView.OnSignUpClicked -= OnSignUpClicked;
            signUpView.OnFemaleClicked -= OnFemaleClicked;
            signUpView.OnMaleClicked -= OnMaleClicked;

            signUpView.OnEmailChanged -= OnEmailChanged;
            signUpView.OnPasswordChanged -= OnPasswordChanged;
            signUpView.OnFullNameChanged -= OnFullNameChanged;
            signUpView.OnDayDobChanged -= OnDayDobChanged;
            signUpView.OnMonthDobChanged -= OnMonthDobChanged;
            signUpView.OnYearDobChanged -= OnYearDobChanged;

            // Outbound
        }

        private void Bind()
        {
            if (disposed)
                throw new ObjectDisposedException(nameof(SignUpPresenter));

            // Inbound
            signUpView.OnSignUpClicked += OnSignUpClicked;
            signUpView.OnFemaleClicked += OnFemaleClicked;
            signUpView.OnMaleClicked += OnMaleClicked;

            signUpView.OnEmailChanged += OnEmailChanged;
            signUpView.OnPasswordChanged += OnPasswordChanged;
            signUpView.OnFullNameChanged += OnFullNameChanged;
            signUpView.OnDayDobChanged += OnDayDobChanged;
            signUpView.OnMonthDobChanged += OnMonthDobChanged;
            signUpView.OnYearDobChanged += OnYearDobChanged;

            // Outbound
        }

        #region Buttons
        private void OnSignUpClicked()
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
                    uiService.ShowPopUp(
                        PopUpType.Information,
                        "Registration successful!"
                    );

                    gameService.PopPhase();
                }
                catch (AuthException ex)
                {
                    uiService.ShowPopUp(
                        PopUpType.Error,
                        ex.Message
                    );
                }
                catch (Exception)
                {
                    uiService.ShowPopUp(
                        PopUpType.Error,
                        "Unexpected error. Please try again."
                    );
                }
            });
        }

        private void OnFemaleClicked()
        {
            gender = Gender.Female;
            ValidateGender();
            UpdateRegisterButton();
        }

        private void OnMaleClicked()
        {
            gender = Gender.Male;
            ValidateGender();
            UpdateRegisterButton();
        }
        #endregion

        #region Inputs
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
        #endregion
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

            signUpView.SetRegisterInteractable(canRegister);
        }

        private void ValidateEmail()
        {
            isEmailValid = !string.IsNullOrWhiteSpace(email) && email.Contains("@");
            signUpView.SetEmailValid(isEmailValid);
        }

        private void ValidatePassword()
        {
            isPasswordValid = !string.IsNullOrWhiteSpace(password) && password.Length >= 6;
            signUpView.SetPasswordValid(isPasswordValid);
        }

        private void ValidateFullName()
        {
            isNameValid = !string.IsNullOrWhiteSpace(fullName);
            signUpView.SetNameValid(isNameValid);
        }

        private void ValidateGender()
        {
            isGenderValid = true;
            signUpView.SetGenderValid(true);
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

            signUpView.SetDobValid(isDobValid);
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
