using Flurl.Http;
using Flurl;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PRN231_FinalProject.DTO;

namespace WebApp.Pages.Label
{
    public class CreateModel : PageModel
    {
        private string link = "http://localhost:5126/api";

        [BindProperty]
        public LabelDTO label { set; get; } = new LabelDTO();
        public async Task<IActionResult> OnGet()
        {
            var userSession = HttpContext.Session.GetInt32("UserSession");
            if (userSession == null)
            {
                return RedirectToPage("/Login/Index", new { Message = "You must login first" });
            }

            return Page();
        }

        public async Task<IActionResult> OnPostEdit(int id)
        {
            var userSession = HttpContext.Session.GetInt32("UserSession");

            try
            {
                var serializer = new Flurl.Http.Newtonsoft.NewtonsoftJsonSerializer();
                label.UserId = userSession.Value;
                label = await link.AppendPathSegments("Label")
                    .WithOAuthBearerToken("token")
                    .WithSettings(s => s.JsonSerializer = serializer)
                    .PostJsonAsync(label).ReceiveJson<LabelDTO>();
                return RedirectToPage("/Label/index", new { id = label.LabelId });
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
