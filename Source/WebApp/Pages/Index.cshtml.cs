using Flurl;
using Flurl.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using OfficeOpenXml;
using PRN231_FinalProject.DTO;
using WebApp.Service;

namespace WebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private string link = "http://localhost:5126/api";
        public string Message { get ;  set; } 

        public List<ContactDTO> contacts { get; set; } = new List<ContactDTO>();
        [BindProperty]
        public List<LabelDTO> labels { set; get; } = new List<LabelDTO>();
        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> OnGet()
        {
            this.Message = Message;
            var userSession = HttpContext.Session.GetInt32("UserSession");
            if (userSession == null)
            {
                return RedirectToPage("/Login/Index", new { Message = "You must login first" });
            }
            try
            {
                var serializer = new Flurl.Http.Newtonsoft.NewtonsoftJsonSerializer();
                contacts = await link.AppendPathSegment("Contact/ListContact/"+ userSession)
                    .WithSettings(s => s.JsonSerializer = serializer)
                    .GetJsonAsync<List<ContactDTO>>();
                contacts = contacts.Where(e => !e.DeleteDate.HasValue).ToList();
                labels = await link.AppendPathSegment("Label/User/" + userSession)
                    .WithSettings(s => s.JsonSerializer = serializer)
                    .GetJsonAsync<List<LabelDTO>>();
                return Page();
            }
            catch (FlurlHttpException ex)
            {
                var err = await ex.GetResponseStringAsync(); // or GetResponseStringAsync(), etc.
                Console.WriteLine($"Error returned from {ex.Call.Request.Url}: {err}");
                return Page();
            }
        }


        public async Task<IActionResult> OnPostExport(List<int> checkedContacts)
        {
            ExcelExportService service = new ExcelExportService();
            var userSession = HttpContext.Session.GetInt32("UserSession");
            try
            {
                var serializer = new Flurl.Http.Newtonsoft.NewtonsoftJsonSerializer();
                contacts = await link.AppendPathSegment("Contact/ListContact/" + userSession)
                    .WithSettings(s => s.JsonSerializer = serializer)
                    .GetJsonAsync<List<ContactDTO>>();
                var deleteContacts = contacts.Where(e => checkedContacts.Contains(e.ContactId)).ToList();
                service.ExportFlatExcel(deleteContacts);
                return RedirectToPage("/index");
            }
            catch (FlurlHttpException ex)
            {
                var err = await ex.GetResponseStringAsync(); // or GetResponseStringAsync(), etc.
                Console.WriteLine($"Error returned from {ex.Call.Request.Url}: {err}");
                return RedirectToPage("/index");

            }
        }

        public async Task<IActionResult> OnPostDelete(List<int> checkedContacts)
        {
            var userSession = HttpContext.Session.GetInt32("UserSession");
            try
            {
                var serializer = new Flurl.Http.Newtonsoft.NewtonsoftJsonSerializer();
                contacts = await link.AppendPathSegment("Contact/ListContact/" + userSession)
                    .WithSettings(s => s.JsonSerializer = serializer)
                    .GetJsonAsync<List<ContactDTO>>();
                var deleteContacts = contacts.Where(e => checkedContacts.Contains(e.ContactId));
                foreach (var contact in deleteContacts)
                {
                    contact.DeleteDate = DateTime.Now.AddDays(30);
                }
                contacts = await link.AppendPathSegment("Contact/UpdateRange")
                    .WithSettings(s => s.JsonSerializer = serializer)
                    .PutJsonAsync(deleteContacts).ReceiveJson<List<ContactDTO>>();
                Console.Write(JsonConvert.SerializeObject(deleteContacts, Formatting.Indented));
                return RedirectToPage("/index");
            }
            catch (FlurlHttpException ex)
            {
                var err = await ex.GetResponseStringAsync(); // or GetResponseStringAsync(), etc.
                Console.WriteLine($"Error returned from {ex.Call.Request.Url}: {err}");
                return RedirectToPage("/index");

            }

            
        }
        public async Task<IActionResult> OnPostAddLabel(List<int> checkedContacts, int labelId)
        {
            var userSession = HttpContext.Session.GetInt32("UserSession");
            try
            {
                var serializer = new Flurl.Http.Newtonsoft.NewtonsoftJsonSerializer();
                contacts = await link.AppendPathSegment("Contact/ListContact/" + userSession)
                    .WithSettings(s => s.JsonSerializer = serializer)
                    .GetJsonAsync<List<ContactDTO>>();
                var deleteContacts = contacts.Where(e => checkedContacts.Contains(e.ContactId) && !e.ContactsLabels.Any(e => e.LabelId == labelId)).ToList();
                foreach (var contact in deleteContacts)
                {
                    contact.ContactsLabels.Add(new ContactsLabel()
                    {
                        ContactId = contact.ContactId,
                        LabelId = labelId
                    });
                }
                contacts = await link.AppendPathSegment("Contact/UpdateRange")
                    .WithSettings(s => s.JsonSerializer = serializer)
                    .PutJsonAsync(deleteContacts).ReceiveJson<List<ContactDTO>>();
                Console.Write(JsonConvert.SerializeObject(deleteContacts, Formatting.Indented));
                return RedirectToPage("/index");
            }
            catch (FlurlHttpException ex)
            {
                var err = await ex.GetResponseStringAsync(); // or GetResponseStringAsync(), etc.
                Console.WriteLine($"Error returned from {ex.Call.Request.Url}: {err}");
                return RedirectToPage("/index");

            }
        }

    }
}
