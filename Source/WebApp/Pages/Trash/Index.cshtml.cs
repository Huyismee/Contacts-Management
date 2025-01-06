using Flurl.Http;
using Flurl;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using PRN231_FinalProject.DTO;
using WebApp.Service;

namespace WebApp.Pages.Trash
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private string link = "http://localhost:5126/api";
        public List<ContactDTO> contacts { get; set; } = new List<ContactDTO>();
        [BindProperty] public List<LabelDTO> labels { set; get; } = new List<LabelDTO>();

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> OnGet()
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
                contacts = contacts.Where(e => e.DeleteDate.HasValue).ToList();
                labels = await link.AppendPathSegment("Label/User/" + userSession)
                    .WithSettings(s => s.JsonSerializer = serializer)
                    .GetJsonAsync<List<LabelDTO>>();
                return Page();
            }
            catch (FlurlHttpException ex)
            {
                var err = await ex.GetResponseJsonAsync<Exception>(); // or GetResponseStringAsync(), etc.
                Console.WriteLine($"Error returned from {ex.Call.Request.Url}: {err.Message}");
                return Page();
            }
        }



        public async Task<IActionResult> OnPostDelete(List<int> checkedContacts)
        {
            try
            {
                var serializer = new Flurl.Http.Newtonsoft.NewtonsoftJsonSerializer();
                foreach (var i in checkedContacts)
                {
                    var contact = await link.AppendPathSegment("Contact/" + i)
                        .WithSettings(s => s.JsonSerializer = serializer)
                        .DeleteAsync();
                    Console.Write(JsonConvert.SerializeObject(contact, Formatting.Indented));
                }
                return RedirectToPage("/Trash/index");
            }
            catch (FlurlHttpException ex)
            {
                var err = await ex.GetResponseJsonAsync<Exception>(); // or GetResponseStringAsync(), etc.
                Console.WriteLine($"Error returned from {ex.Call.Request.Url}: {err.Message}");
                return RedirectToPage("/Trash/index");

            }


        }

        public async Task<IActionResult> OnPostRestore(List<int> checkedContacts)
        {
            var userSession = HttpContext.Session.GetInt32("UserSession");

            try
            {
                var serializer = new Flurl.Http.Newtonsoft.NewtonsoftJsonSerializer();
                contacts = await link.AppendPathSegment("Contact/ListContact/" + userSession)
                    .WithSettings(s => s.JsonSerializer = serializer)
                    .GetJsonAsync<List<ContactDTO>>();
                var deleteContacts = contacts.Where(e =>
                    checkedContacts.Contains(e.ContactId)).ToList();
                foreach (var contact in deleteContacts)
                {
                    contact.DeleteDate = null;

                }

                contacts = await link.AppendPathSegment("Contact/UpdateRange")
                    .WithSettings(s => s.JsonSerializer = serializer)
                    .PutJsonAsync(deleteContacts).ReceiveJson<List<ContactDTO>>();
                Console.Write(JsonConvert.SerializeObject(deleteContacts, Formatting.Indented));
                return RedirectToPage("/Trash/index");
            }
            catch (FlurlHttpException ex)
            {
                var err = await ex.GetResponseJsonAsync<Exception>(); // or GetResponseStringAsync(), etc.
                Console.WriteLine($"Error returned from {ex.Call.Request.Url}: {err.Message}");
                return RedirectToPage("/Trash/index");

            }
        }
    }
}
