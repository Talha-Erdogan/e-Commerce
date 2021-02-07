using e_Commerce.Web.Business.Models.Auth;
using e_Commerce.Web.Business.Models.Profile;
using System;
using System.Collections.Generic;
using System.Text;

namespace e_Commerce.Web.Business.Common
{
    // Asp.net Session StateServer kullanimi icin Serializable attribute eklenir
    [Serializable]
    public class SessionUser
    {
        public int Id { get; set; }
        public string TRNationalId { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public int SexId { get; set; }

        public string UserToken { get; set; }


        public List<Auth> AuthList { get; set; }
        public List<Profile> ProfileList { get; set; }
    }
}
