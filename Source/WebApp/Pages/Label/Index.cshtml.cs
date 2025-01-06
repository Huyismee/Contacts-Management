using Flurl.Http;
using Flurl;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using PRN231_FinalProject.DTO;
using WebApp.Service;

namespace WebApp.Pages.Label
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private string link = "http://localhost:5126/api";
        public List<ContactDTO> contacts { get; set; } = new List<ContactDTO>();
        [BindProperty]
        public List<LabelDTO> labels { set; get; } = new List<LabelDTO>();

        [BindProperty] public LabelDTO label { set; get; } = new LabelDTO();
        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> OnGet(int id)
        {
            var userSession = HttpContext.Session.GetInt32("UserSession");
            if (userSession == null)
            {
                return RedirectToPage("/Login/Index", new { Message = "You must login first" });
            }
            try
            {
                var serializer = new Flurl.Http.Newtonsoft.NewtonsoftJsonSerializer();
                contacts = await link.AppendPathSegment("Contact/ListContact/" + userSession)
                    .WithSettings(s => s.JsonSerializer = serializer)
                    .GetJsonAsync<List<ContactDTO>>();
                contacts = contacts.Where(e => !e.DeleteDate.HasValue && e.ContactsLabels.Any(e => e.LabelId == id)).ToList();
                labels = await link.AppendPathSegment("Label/User/" + userSession)
                    .WithSettings(s => s.JsonSerializer = serializer)
                    .GetJsonAsync<List<LabelDTO>>();
                if (!labels.Any(e => e.LabelId == id))
                {
                    return RedirectToPage("/Index");
                }
                label = labels.FirstOrDefault(e => e.LabelId == id);
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
            var userSession = HttpContext.Session.GetInt32("UserSession");

            ExcelExportService service = new ExcelExportService();
            try
            {
                var serializer = new Flurl.Http.Newtonsoft.NewtonsoftJsonSerializer();
                contacts = await link.AppendPathSegment("Contact/ListContact/" + userSession)
                    .WithSettings(s => s.JsonSerializer = serializer)
                    .GetJsonAsync<List<ContactDTO>>();
                var deleteContacts = contacts.Where(e => checkedContacts.Contains(e.ContactId)).ToList();
                service.ExportFlatExcel(deleteContacts);
                return RedirectToPage("/label/index");
            }
            catch (FlurlHttpException ex)
            {
                var err = await ex.GetResponseStringAsync(); // or GetResponseStringAsync(), etc.
                Console.WriteLine($"Error returned from {ex.Call.Request.Url}: {err}");
                return Page();

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
                return RedirectToPage("/label/index");
            }
            catch (FlurlHttpException ex)
            {
                var err = await ex.GetResponseStringAsync(); // or GetResponseStringAsync(), etc.
                Console.WriteLine($"Error returned from {ex.Call.Request.Url}: {err}");
                return Page();

            }
        }

        public async Task<IActionResult> OnPostDeleteLabel(int id)
        {
            try
            {
                var serializer = new Flurl.Http.Newtonsoft.NewtonsoftJsonSerializer();
                var res = await link.AppendPathSegment("Label/"+id)
                    .WithSettings(s => s.JsonSerializer = serializer)
                    .DeleteAsync();
                return RedirectToPage("/index");
            }
            catch (FlurlHttpException ex)
            {
                var err = await ex.GetResponseStringAsync(); // or GetResponseStringAsync(), etc.
                Console.WriteLine($"Error returned from {ex.Call.Request.Url}: {err}");
                return Page();

            }
        }

        public async Task<IActionResult> OnPostDeleteAll(int id)
        {
            var userSession = HttpContext.Session.GetInt32("UserSession");

            try
            {
                var serializer = new Flurl.Http.Newtonsoft.NewtonsoftJsonSerializer();
                var contacts = await link.AppendPathSegment("Contact/ListContact/" + userSession)
                    .WithSettings(s => s.JsonSerializer = serializer)
                    .GetJsonAsync<List<ContactDTO>>();
                contacts = contacts.Where(e => e.ContactsLabels.Any(e => e.LabelId == id)).ToList();
                foreach (var contact in contacts)
                {
                    contact.DeleteDate = DateTime.Now.AddDays(30);
                }
                contacts = await link.AppendPathSegment("Contact/UpdateRange")
                    .WithSettings(s => s.JsonSerializer = serializer)
                    .PutJsonAsync(contacts).ReceiveJson<List<ContactDTO>>();
                var res = await link.AppendPathSegment("Label/" + id)
                    .WithSettings(s => s.JsonSerializer = serializer)
                    .DeleteAsync();
                return RedirectToPage("/index");
            }
            catch (FlurlHttpException ex)
            {
                var err = await ex.GetResponseStringAsync(); // or GetResponseStringAsync(), etc.
                Console.WriteLine($"Error returned from {ex.Call.Request.Url}: {err}");
                return Page();

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
                return RedirectToPage("/label/index");
            }
            catch (FlurlHttpException ex)
            {
                var err = await ex.GetResponseStringAsync(); // or GetResponseStringAsync(), etc.
                Console.WriteLine($"Error returned from {ex.Call.Request.Url}: {err}");
                return Page();

            }
        }
    }
}
