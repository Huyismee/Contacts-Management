using Flurl;
using Flurl.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PRN231_Assignment2_eBookStoreAPI.DTO;
using WebApp.DTO;

namespace WebApp.Pages.Register
{
    public class IndexModel : PageModel
    {
        private string email;
        private string password;
        private string confirmPassword;
        private string message;
        private string link = "http://localhost:5126/api";
        [BindProperty]
        public string Email { get { return email; } set { email = value; } }
        [BindProperty]
        public string Password { get { return password; } set { password = value; } }
        [BindProperty]
        public string Message { get { return message; } set { message = value; } }
        [BindProperty]
        public string ConfirmPassword { get { return confirmPassword; } set { confirmPassword = value; } }

        [BindProperty] public string Fullname { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            RegisterDto registerDto = new RegisterDto()
            {
                EmailAddress = Email,
                Password = Password,
                ReEnterPassword = ConfirmPassword,
                FullName = Fullname
            };
            try
            {
                var serializer = new Flurl.Http.Newtonsoft.NewtonsoftJsonSerializer();
                var user = await link.AppendPathSegment("User/Register")
                    .WithSettings(s => s.JsonSerializer = serializer)
                    .PostJsonAsync(registerDto).ReceiveJson<User>();
                HttpContext.Session.SetInt32("UserSession", user.UserId);
                return RedirectToPage("/login/index", new {message = "Register successful! Please Login"});
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
