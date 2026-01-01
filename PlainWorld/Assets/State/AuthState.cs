using Assets.State.Interface.IReadOnlyState;

namespace Assets.State
{
    public class AuthState : IReadOnlyAuthState
    {
        #region Attributes
        #endregion

        #region Properites
        public string Token { get; private set; }

        #endregion

        public AuthState()
        {
        }

        #region Methods
        public void Set(string token)
        {
            if (Token == token) return;

            Token = token;
        }
        #endregion
    }
}
