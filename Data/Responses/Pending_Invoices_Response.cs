using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppBlazor.Data
{
    public class User_Accno
    {
        public int user_id { get; set; } = -1;
        public int accno { get; set; } = -1;
    }

    public class Pending_Invoices_Response
    {
        public Response_Code status_code { get; set; } = Response_Code.ok;
        public string status_message { get; set; } = "";
        public List<Invoice> invoices { get; set; }
    }

    public class Pending_Invoice_Details
    {
        public Response_Code status_code { get; set; } = Response_Code.ok;
        public string status_message { get; set; } = "";
        public Invoice invoice { get; set; }
        public List<Invoice_Line> lines { get; set; }
    }

    public class Invoice
    {
        public int document_id { get; set; } = -1;
        public string supplier_name { get; set; } = "";
        public string invoice_number { get; set; } = "";
        public string invoice_date { get; set; } = "";
        public string invoice_due { get; set; } = "";

        //details
        public int supplier_accno { get; set; } = -1;
        public string supplier_gst { get; set; } = "xxx-xxx-xxx";
        public string currency { get; set; } = "NZD";
        public string invoice_period { get; set; } = "";
        public string order_ref { get; set; } = "";
        public string branch { get; set; } = "";

        public decimal invoice_sub_total { get; set; } = 0.0M;
        public decimal invoice_tax { get; set; } = 0.0M;
        public decimal invoice_total { get; set; } = 0.0M;
    }

    public class Invoice_Line
    {
        public int line_id { get; set; } = -1;
        public string branch { get; set; } = "";
        public string line_code { get; set; } = "";
        public string description { get; set; } = "";
        public int quantity { get; set; } = 0;
        public string tax_rate { get; set; } = "";
        public decimal line_unit_price { get; set; } = 0.0M;
        public decimal line_sub_total { get; set; } = 0.0M;
        public decimal line_tax { get; set; } = 0.0M;
        public decimal line_total { get; set; } = 0.0M;
    }

}
