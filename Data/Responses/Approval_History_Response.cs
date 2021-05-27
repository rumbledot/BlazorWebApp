using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppBlazor.Data
{
    public class Approval_History_Response
    {
        public Response_Code status_code { get; set; } = Response_Code.ok;
        public string status_message { get; set; } = "";
        public List<Approval> approvals { get; set; }
    }

    public class Approval
    {
        public bool is_approved { get; set; } = false;
        public string approval_type { get; set; } = "";
        public string approval_target { get; set; } = "";
        public string user_name { get; set; } = "";
        public string user_role { get; set; } = "";
        public string reason { get; set; } = "";
        public string approval_date { get; set; } = "";
    }
}
