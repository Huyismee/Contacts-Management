using Flurl;
using Flurl.Http;
using Flurl.Http.Newtonsoft;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using PRN231_FinalProject.DTO;

namespace WebApp.Pages.Contact
{
    public class IndexModel : PageModel
    {
        private string link = "http://localhost:5126/api";
        [BindProperty(SupportsGet = true)]
        public ContactDTO contact { set; get; } = new ContactDTO();
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
                contact = await link.AppendPathSegment("Contact/" + id)
                    .WithSettings(s => s.JsonSerializer = serializer)
                    .GetJsonAsync<ContactDTO>();
                if (contact.UserId != userSession)
                {
                    return RedirectToPage("/index");
                }
                return Page();

            }
            catch (FlurlHttpException ex)
            {
                var err = await ex.GetResponseStringAsync(); // or GetResponseStringAsync(), etc.
                Console.WriteLine($"Error returned from {ex.Call.Request.Url}: {err}");
                return Page();

            }
        }

        public async Task<IActionResult> OnPostDelete(int id)
        {
            try
            {
                var serializer = new Flurl.Http.Newtonsoft.NewtonsoftJsonSerializer();
                var contactDto = await link.AppendPathSegment("Contact/" + id)
                    .WithSettings(s => s.JsonSerializer = serializer)
                    .GetJsonAsync<ContactDTOWithoutLabel>();
                contactDto.DeleteDate = DateTime.Now.AddDays(30);
                var jsonstring = serializer.Serialize(contactDto);
                Console.WriteLine(jsonstring);
                contact = await link.AppendPathSegments("Contact")
                   .WithOAuthBearerToken("token")
                   .WithSettings(s => s.JsonSerializer = serializer)
                   .PutJsonAsync(contactDto).ReceiveJson<ContactDTO>();
                return RedirectToPage("/index");
            }
            catch (FlurlHttpException ex)
            {
                var err = await ex.GetResponseStringAsync(); // or GetResponseStringAsync(), etc.
                Console.WriteLine($"Error returned from {ex.Call.Request.Url}: {err}");
                return Page();

            }
        }
        public async Task<IActionResult> OnPostEdit(int id)
        {
            return RedirectToPage("/Contact/Edit", new { id = id });
        }
        public async Task<IActionResult> OnPostFavorite(int id)
        {
            try
            {
                var serializer = new Flurl.Http.Newtonsoft.NewtonsoftJsonSerializer();
                var contactDto = await link.AppendPathSegment("Contact/" + id)
                    .WithSettings(s => s.JsonSerializer = serializer)
                    .GetJsonAsync<ContactDTOWithoutLabel>();
                contactDto.Favorite = !contactDto.Favorite;
                var jsonstring = serializer.Serialize(contactDto);
                Console.WriteLine(jsonstring);
                var response = await link.AppendPathSegments("Contact")
                    .WithOAuthBearerToken("token")
                    .WithSettings(s => s.JsonSerializer = serializer)
                    .PutJsonAsync(contactDto).ReceiveJson<ContactDTO>();
                contact = await link.AppendPathSegment("Contact/" + id)
                    .WithSettings(s => s.JsonSerializer = serializer)
                    .GetJsonAsync<ContactDTO>();
                return RedirectToPage("/Contact/index", new { id = id });
            }
            catch (FlurlHttpException ex)
            {
                var err = await ex.GetResponseStringAsync(); // or GetResponseStringAsync(), etc.
                Console.WriteLine($"Error returned from {ex.Call.Request.Url}: {err}");
                return Page();

            }

            return RedirectToPage("Contact/" + id);
        }
        public async Task<IActionResult> OnPostDeleteTotal(int id)
        {
            try
            {
                var serializer = new Flurl.Http.Newtonsoft.NewtonsoftJsonSerializer();

                var contact = await link.AppendPathSegment("Contact/" + id)
                    .WithSettings(s => s.JsonSerializer = serializer)
                    .DeleteAsync();
                Console.Write(JsonConvert.SerializeObject(contact, Formatting.Indented));

                return RedirectToPage("/Trash/index");
            }
            catch (FlurlHttpException ex)
            {
                var err = await ex.GetResponseStringAsync(); // or GetResponseStringAsync(), etc.
                Console.WriteLine($"Error returned from {ex.Call.Request.Url}: {err}");
                return Page();

            }


        }

        public async Task<IActionResult> OnPostRestore(int id)
        {
            try
            {
                var serializer = new Flurl.Http.Newtonsoft.NewtonsoftJsonSerializer();
                var contactDto = await link.AppendPathSegment("Contact/" + id)
                    .WithSettings(s => s.JsonSerializer = serializer)
                    .GetJsonAsync<ContactDTOWithoutLabel>();
                contactDto.DeleteDate = null;
                var jsonstring = serializer.Serialize(contactDto);
                Console.WriteLine(jsonstring);
                contact = await link.AppendPathSegments("Contact")
                    .WithOAuthBearerToken("token")
                    .WithSettings(s => s.JsonSerializer = serializer)
                    .PutJsonAsync(contactDto).ReceiveJson<ContactDTO>();
                return RedirectToPage("/Trash/index");
            }
            catch (FlurlHttpException ex)
            {
                var err = await ex.GetResponseJsonAsync<Exception>(); // or GetResponseStringAsync(), etc.
                Console.WriteLine($"Error returned from {ex.Call.Request.Url}: {err.Message}");
                return Page();
            }
        }
    }
}
