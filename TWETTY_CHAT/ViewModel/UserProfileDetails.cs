namespace TWETTY_CHAT
{
    /// <summary>
    /// The result of a login request or get user profile details
    /// </summary>
    public class UserProfileDetails : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// The user email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The user first name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The user last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// The initials to show for the profile picture background
        /// </summary>
        public string Initials
        {
            get
            {
                return string.Format("{0}{1}", string.IsNullOrWhiteSpace(FirstName) ? ' ' : FirstName[0], string.IsNullOrWhiteSpace(LastName) ? ' ' : LastName[0]).ToUpper();
            }
        }

        /// <summary>
        /// The user profile picture
        /// </summary>
        public string ProfilePictureRGB
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(FirstName))
                    return GenerateRGBValues.Generate(FirstName[0]);
                return "";
            }
            set
            {
                ProfilePictureRGB = value;
            }
        }

        /// <summary>
        /// The user status
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// The authentication token used to stay authenticated through future requests
        /// </summary>
        /// <remarks>The Token is only provided when called from the connection methods</remarks>
        public string Token { get; set; }

        #endregion

        #region Singleton

        /// <summary>
        /// A single instance
        /// </summary>
        public static UserProfileDetails Instance { get; set; } = new UserProfileDetails();

        #endregion

        #region Constructor
        public UserProfileDetails()
        {
            Email = "admin@twetty.md";
            FirstName = "Admin";
            LastName = "Application";
            ProfilePictureRGB = GenerateRGBValues.Generate(FirstName[0]);
        }

        #endregion

        #region Public Helper Methods

        /// <summary>
        /// Creates a new <see cref="LoginCredentialsDataModel"/>
        /// from this model
        /// </summary>
        /// <returns>Login data</returns>
        public LoginCredentialsDataModel ToLoginCredentialsDataModel()
        {
            return new LoginCredentialsDataModel
            {
                Email = Email,
                FirstName = FirstName,
                LastName = LastName,
                Token = Token
            };
        }

        #endregion

    }
}
