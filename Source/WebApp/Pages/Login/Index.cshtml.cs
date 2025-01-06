using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net;
using PRN231_Assignment2_eBookStoreAPI.DTO;
using Flurl.Http;
using Flurl;
using PRN231_FinalProject.DTO;
using WebApp.DTO;

namespace WebApp.Pages.Login
{
    public class IndexModel : PageModel
    {
        private string email;
        private string password;
        private string message;
        private string link = "http://localhost:5126/api";
        [BindProperty]
        public string Email { get { return email; } set { email = value; } }
        [BindProperty]
        public string Password { get { return password; } set { password = value; } }
        [BindProperty]
        public string Message { get { return message; } set { message = value; } }
        public void OnGet(string? Message)
        {
            this.Message = Message;
            var userSession = HttpContext.Session.GetString("UserSession");
            if (!string.IsNullOrEmpty(userSession))
            {
                HttpContext.Session.Remove("UserSession");
                HttpContext.Session.Remove("UserPfSession");
            }
        }

        public async Task<IActionResult> OnPost()
        {
            LoginDto loginDto = new LoginDto()
            {
                EmailAddress = Email,
                Password = Password,
            };
            try
            {
                var serializer = new Flurl.Http.Newtonsoft.NewtonsoftJsonSerializer();
                var user = await link.AppendPathSegment("User/Login")
                    .WithSettings(s => s.JsonSerializer = serializer)
                    .PostJsonAsync(loginDto).ReceiveJson<User>();
                HttpContext.Session.SetInt32("UserSession", user.UserId);
                return RedirectToPage("/index");
            }
            catch (FlurlHttpException ex)
            {
                var err = await ex.GetResponseStringAsync(); // or GetResponseStringAsync(), etc.
                message = $"Error {err}";
                return Page();

            }
        }
    }
}
