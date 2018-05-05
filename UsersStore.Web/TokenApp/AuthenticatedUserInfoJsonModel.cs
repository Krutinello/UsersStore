using System;

namespace UsersStore.Web.TokenApp
{
    public class AuthenticatedUserInfoJsonModel
    {
        public string Token { get; internal set; }
        public string Login { get; internal set; }
    }
}
