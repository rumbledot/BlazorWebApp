using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppBlazor.Data
{
    public class Users_Response
    {
        public Response_Code status_code { get; set; } = Response_Code.ok;
        public string status_message { get; set; } = "";
        public List<KL_User> Users = new List<KL_User>();
    }

    public class KL_User
    {
        public int id { get; set; } = -1;
        public string username { get; set; } = "";
        public string password { get; set; } = "";
        public string email { get; set; } = "";
        public string role { get; set; } = "";
        public bool is_manager { get; set; } = false;
        public bool is_ceo { get; set; } = false;
        public string exo_name { get; set; } = "";
        public string exo_title { get; set; } = "";
        public string token { get; set; } = "";
    }

    public class Login
    {
        [Required]
        public string username { get; set; }

        [Required]
        public string password { get; set; }
    }
}
