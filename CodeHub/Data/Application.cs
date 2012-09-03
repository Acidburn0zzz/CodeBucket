using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeHub
{
    /// <summary>
    /// Application.
    /// </summary>
    public static class Application
    {
        public static GithubSharp.Core.API.User Client { get; private set; }
        public static Account Account { get; private set; }
        public static Accounts Accounts { get; private set; }

        static Application()
        {
            Accounts = new Accounts();
        }


        public static void SetUser(Account account)
        {
            Account = account;
            Accounts.SetDefault(Account);

            //Client = new BitbucketSharp.Client(Account.Username, Account.Password) { Timeout = 1000 * 30 };
        }

        public static void LoadSettings()
        {
        }
    }
}

