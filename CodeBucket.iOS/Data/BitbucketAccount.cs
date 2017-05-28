using SQLite;
using System.IO;
using System;
using System.Globalization;
using Splat;

namespace CodeBucket.Data
{
    public class BitbucketAccount : IDisposable
    {
        private SQLiteConnection _database;
        private AccountPinnedRepositories _pinnedRepositories;

        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string AvatarUrl { get; set; }

        public string DefaultStartupView { get; set; }

        public string Token { get; set; }

        public string RefreshToken { get; set; }

        public bool DontShowTeamEvents { get; set; }

        [Ignore]
        public SQLiteConnection Database 
        {
            get
            {
                if (_database == null)
                {
                    if (!Directory.Exists(AccountDirectory))
                        Directory.CreateDirectory(AccountDirectory);

                    var dbPath = Path.Combine(AccountDirectory, "settings.db");
                    _database = new SQLiteConnection(dbPath);
                    return _database;
                }

                return _database;
            }
        }

        [Ignore]
        public string AccountDirectory
        {
            get
            {
                var baseDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "..");
                var accountsDir = Path.Combine(baseDir, "Documents/accounts");
                return Path.Combine(accountsDir, Id.ToString(CultureInfo.InvariantCulture));
            }
        }

        [Ignore]
        public AccountPinnedRepositories PinnnedRepositories
        {
            get
            {
                return _pinnedRepositories ?? (_pinnedRepositories = new AccountPinnedRepositories(Database));
            }
        }

        private void CreateAccountDirectory()
        {
            if (!Directory.Exists(AccountDirectory))
                Directory.CreateDirectory(AccountDirectory);
        }

        /// <summary>
        /// This creates this account's directory
        /// </summary>
        public void Initialize()
        {
            CreateAccountDirectory();
        }

        /// <summary>
        /// This destorys this account's directory
        /// </summary>
        public void Destory()
        {
            if (!Directory.Exists(AccountDirectory))
                return;
            Directory.Delete(AccountDirectory, true);
        }


        /// <summary>
        /// Returns a <see cref="System.String"/> that represents the current <see cref="Account"/>.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents the current <see cref="Account"/>.</returns>
        public override string ToString()
        {
            return Username;
        }

        public void Dispose()
        {
            if (_database != null) _database.Dispose();
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to the current <see cref="Account"/>.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with the current <see cref="Account"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to the current
        /// <see cref="Account"/>; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            var act = obj as BitbucketAccount;
            return act != null && this.Id.Equals(act.Id);
        }

        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            return this.Id;
        }
            
		[Ignore]
		public bool ShowTeamEvents
		{
			get { return !DontShowTeamEvents; }
			set { DontShowTeamEvents = !value; }
		}

        /// <summary>
        /// Gets or sets whether teams & groups should be expanded in the menu controller to their actual contents
        /// </summary>
		public bool DontExpandTeamsAndGroups { get; set; }

		[Ignore]
		public bool ExpandTeamsAndGroups
		{
			get { return !DontExpandTeamsAndGroups; }
			set { DontExpandTeamsAndGroups = !value; }
		}

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="CodeBucket.Data.Account"/> hides the repository
        /// description in list.
        /// </summary>
        /// <value><c>true</c> if hide repository description in list; otherwise, <c>false</c>.</value>
		public bool HideRepositoryDescriptionInList { get; set; }

		[Ignore]
		public bool RepositoryDescriptionInList
		{
			get { return !HideRepositoryDescriptionInList; }
			set { HideRepositoryDescriptionInList = !value; }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Account"/> class.
		/// </summary>
		public BitbucketAccount()
		{
			//Set some default values
			ShowTeamEvents = true;
			ExpandTeamsAndGroups = true;
			RepositoryDescriptionInList = true;
		}

    }
}