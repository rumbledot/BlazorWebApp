using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using WebAppBlazor;
using WebAppBlazor.Shared;
using WebAppBlazor.Helpers;
using WebAppBlazor.Models;
using WebAppBlazor.Data;
using WebAppBlazor.Services;
using System.Web;
using System.Net.Http;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using WebAppBlazor.JSI;

namespace WebAppBlazor.Pages
{
    public partial class Pending_Invoices
    {
        [Parameter]
        public string user_id { get; set; }

        int uid = 0;
        List<Invoice> Invoices = new List<Invoice>();
        Invoice Invoice = new Invoice();
        List<Invoice_Line> Lines = new List<Invoice_Line>();
        List<Approval> History = new List<Approval>();
        List<Invoice> Approved = new List<Invoice>();
        bool display_warning = false, details_loaded = false;
        string error_message;
        byte[] pdf_binary;

        protected override void OnInitialized()
        {
            try
            {
                if (user_id != null && int.TryParse(user_id, out uid))
                {
                    GetPendingInvoices(uid);
                }
            }
            catch (Exception ex)
            {
                DisplayToast($"Failed to load page \n {ex.Message} \n {ex.StackTrace}");
            }
        }

        private async void GetPendingInvoices(int user_id)
        {
            try
            {
                var api_name = $"http://localhost:5555/kl/invoice/fetchwaitinginvoices/{user_id}";
                var httpResponse = await client.PostAsJsonAsync(api_name, user_id);
                if (httpResponse.IsSuccessStatusCode)
                {
                    Pending_Invoices_Response responseData = JsonConvert.DeserializeObject<Pending_Invoices_Response>(await httpResponse.Content.ReadAsStringAsync());
                    Invoices = responseData.invoices;
                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                DisplayToast($"Failed to get pending invoices \n {ex.Message} \n {ex.StackTrace}");
            }
        }

        private async void GetInvoiceDetails(int document_id)
        {
            try
            {
                pdf_binary = null;
                details_loaded = false;

                var api_name = $"http://localhost:5555/kl/invoice/fetchdetails/{document_id}";
                var httpResponse = await client.PostAsJsonAsync(api_name, document_id);
                if (httpResponse.IsSuccessStatusCode)
                {
                    Pending_Invoice_Details responseData = JsonConvert.DeserializeObject<Pending_Invoice_Details>(await httpResponse.Content.ReadAsStringAsync());
                    Invoice = responseData.invoice;
                    Lines = responseData.lines;
                    GetInvoiceHistory(document_id);

                    StateHasChanged();

                    if (user_id != null && int.TryParse(user_id, out uid))
                    {
                        User_Accno u_a = new User_Accno()
                        {
                            user_id = uid, 
                            accno = Invoice.supplier_accno
                        };

                        GetInvoiceApproved(u_a);
                    }
                }

                details_loaded = true;
            }
            catch (Exception ex)
            {
                DisplayToast($"Failed to get invoice details \n {ex.Message} \n {ex.StackTrace}");
            }
        }

        private async void GetInvoiceHistory(int document_id)
        {
            try
            {
                var api_name = $"http://localhost:5555/kl/invoice/FetchApprovalHistory/{document_id}";
                var httpResponse = await client.PostAsJsonAsync(api_name, document_id);
                if (httpResponse.IsSuccessStatusCode)
                {
                    Approval_History_Response responseData = JsonConvert.DeserializeObject<Approval_History_Response>(await httpResponse.Content.ReadAsStringAsync());
                    History = responseData.approvals;
                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                DisplayToast($"Failed to get invoice history \n {ex.Message} \n {ex.StackTrace}");
            }
        }

        private void DownloadFile(string filename, int document_id)
        {
            if (pdf_binary == null)
            {
                GetInvoicePDF(document_id);
            }
            else
            {
                var pdf = new PDFGenerator();
                pdf.DownloadPDF(js, $"{document_id}.pdf", pdf_binary);
            }
        }

        private void DisplayPDFDocument(string iFrame_id)
        {
            var pdf = new PDFGenerator();
            pdf.ViewPDF(js, iFrame_id, pdf_binary);
        }

        private void RemovePDFViewer(string iFrame_id) 
        {
            var ui = new UserInteraction();
            ui.ClearViewer(js, iFrame_id);
        }

        private void FocusToElemenetWithID(string id) 
        {
            var ui = new UserInteraction();
            ui.FocusToElementWithID(js, id);
        }

        private async void GetInvoicePDF(int document_id)
        {
            try
            {
                if (pdf_binary == null)
                {
                    var api_name = $"http://localhost:5555/kl/invoice/FetchInvoicePDF/{document_id}";
                    var httpResponse = await client.PostAsJsonAsync(api_name, document_id);
                    if (httpResponse.IsSuccessStatusCode)
                    {
                        pdf_binary = (await httpResponse.Content.ReadAsByteArrayAsync());

                        StateHasChanged();
                    }
                }
                //else 
                //{
                //    DisplayPDFDocument("PDF_iframe");
                //}
            }
            catch (Exception ex)
            {
                DisplayToast($"Failed to get Invoice PDF \n {ex.Message} \n {ex.StackTrace}");
            }
        }

        private async void GetInvoiceApproved(User_Accno user_accno)
        {
            try
            {
                var api_name = $"http://localhost:5555/kl/invoice/FetchApproved/";
                var httpResponse = await client.PostAsJsonAsync(api_name, user_accno);
                if (httpResponse.IsSuccessStatusCode)
                {
                    Pending_Invoices_Response responseData = JsonConvert.DeserializeObject<Pending_Invoices_Response>(await httpResponse.Content.ReadAsStringAsync());
                    Approved = responseData.invoices;

                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                DisplayToast($"Failed to get approved invoices \n {ex.Message} \n {ex.StackTrace}");
            }
        }

        private async Task DisplayToast(string message)
        {
            display_warning = true;
            error_message = message;
            await Task.Delay(1000);
            display_warning = false;
        }
    }
}