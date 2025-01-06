using Flurl;
using Flurl.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using PRN231_FinalProject.DTO;
using PRN231_FinalProject.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using WebApp.Service;

namespace WebApp.Pages.Contact
{
    public class CreateModel : PageModel
    {
        private PhoneNumberService phoneNumberService = new PhoneNumberService();
        private string link = "http://localhost:5126/api";
        private readonly string _uploadDirectory = "wwwroot/img";

        public CreateModel()
        {
            if (!Directory.Exists(_uploadDirectory))
            {
                Directory.CreateDirectory(_uploadDirectory);
            }
        }

        [BindProperty(SupportsGet = true)]
        public List<int> checkedLabels { get; set; }
        [BindProperty(SupportsGet = true)]
        public ContactDTO contact { set; get; } = new ContactDTO();
        [BindProperty]
        public List<ContactEmail> emails { set; get; } = new List<ContactEmail>();
        [BindProperty]
        public List<ContactPhone> phones { set; get; } = new List<ContactPhone>();
        [BindProperty]
        public List<LabelDTO> labels { set; get; } = new List<LabelDTO>();

        [BindProperty]
        public IFormFile uploadImage { get; set; }

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

        public async Task<IActionResult> OnPostCreate(string firstName, string lastName, string note, DateTime dob)
        {
            var userSession = HttpContext.Session.GetInt32("UserSession");
            var serializer = new Flurl.Http.Newtonsoft.NewtonsoftJsonSerializer();
            var ContactLabels = new List<ContactsLabel>();
            foreach (var checkedLabel in checkedLabels)
            {
                ContactLabels.Add(new ContactsLabel()
                {
                    LabelId = checkedLabel
                });
            }
            foreach (ContactPhone contactPhone in phones)
            {
                if(phoneNumberService.ValidatePhoneNumber(contactPhone.Phone, contactPhone.Code.ToUpper()))
                {
                    contactPhone.Phone = phoneNumberService.FormatPhoneNumberNational(contactPhone.Phone, contactPhone.Code.ToUpper());
                };
            }
            contact.ContactsLabels = ContactLabels;
            contact.ContactPhones = phones;
            contact.ContactEmails = emails;
            contact.Firstname = firstName;
            contact.Lastname = lastName;
            contact.DateOfBirth = dob;
            contact.Notes = note;
            contact.UserId = userSession.Value;
            contact.Favorite = false;
            try
            {
                contact = await link.AppendPathSegments("Contact")
                    .WithOAuthBearerToken("token")
                    .WithSettings(s => s.JsonSerializer = serializer)
                    .PostJsonAsync(contact).ReceiveJson<ContactDTO>();
            }
            catch (FlurlHttpException ex)
            {
                var err = await ex.GetResponseStringAsync(); // or GetResponseStringAsync(), etc.
                Console.WriteLine($"Error returned from {ex.Call.Request.Url}: {err}");
                return Page();

            }
            var rentalDirectory = Path.Combine(_uploadDirectory, contact.ContactId.ToString());
            var imageDirectory = Path.Combine(rentalDirectory, "img");

            if (!Directory.Exists(imageDirectory))
            {
                Directory.CreateDirectory(imageDirectory);
            }
            if (uploadImage != null && uploadImage.ContentType.StartsWith("image/"))
            {
                string newFileName = $"{Path.GetExtension(uploadImage.FileName)}";
                string filePath = Path.Combine(imageDirectory, newFileName);
                var imagePath = Path.Combine("img", contact.ContactId.ToString(), "img", newFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    using (var image = Image.Load(uploadImage.OpenReadStream()))
                    {
                        var encoder = new JpegEncoder { Quality = 75 }; // Set chất lượng ảnh, giá trị từ 1-100
                        image.Save(stream, encoder);
                    }
                }

                contact.ProfileImage = imagePath;
                try
                {
                    contact = await link.AppendPathSegments("Contact")
                        .WithOAuthBearerToken("token")
                        .WithSettings(s => s.JsonSerializer = serializer)
                        .PutJsonAsync(contact).ReceiveJson<ContactDTO>();
                }
                catch (FlurlHttpException ex)
                {
                    var err = await ex.GetResponseStringAsync(); // or GetResponseStringAsync(), etc.
                    Console.WriteLine($"Error returned from {ex.Call.Request.Url}: {err}");
                    return Page();

                }
            }


           
            
            string test = JsonConvert.SerializeObject(contact);
            Console.WriteLine(test);
            return RedirectToPage("/index");

        }
    }
}
