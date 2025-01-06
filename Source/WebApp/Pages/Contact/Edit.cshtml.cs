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
    public class EditModel : PageModel
    {
        private PhoneNumberService phoneNumberService = new PhoneNumberService();
        private string link = "http://localhost:5126/api";
        private readonly string _uploadDirectory = "wwwroot/img";

        public EditModel()
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
                labels = await link.AppendPathSegment("Label/User/" + userSession)
                    .WithSettings(s => s.JsonSerializer = serializer)
                    .GetJsonAsync<List<LabelDTO>>();
                emails = contact.ContactEmails.ToList();
                phones = contact.ContactPhones.ToList();
                return Page();
            }
            catch (FlurlHttpException ex)
            {
                var err = await ex.GetResponseStringAsync(); // or GetResponseStringAsync(), etc.
                Console.WriteLine($"Error returned from {ex.Call.Request.Url}: {err}");
                return Page();

            }

        }

        public async Task<IActionResult> OnPostEdit(int id, string firstName, string lastName, string note, DateTime dob)
        {
            var userSession = HttpContext.Session.GetInt32("UserSession");
            var serializer = new Flurl.Http.Newtonsoft.NewtonsoftJsonSerializer();
            try
            {
                contact = await link.AppendPathSegment("Contact/" + id)
                    .WithSettings(s => s.JsonSerializer = serializer)
                    .GetJsonAsync<ContactDTO>();
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
                string newFileName = $"avartar{Path.GetExtension(uploadImage.FileName)}";
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
            }

            var ContactLabels = new List<ContactsLabel>();
            foreach (var checkedLabel in checkedLabels)
            {
                ContactLabels.Add(new ContactsLabel()
                {
                    ContactId = contact.ContactId,
                    LabelId = checkedLabel
                });
            }

            foreach (ContactPhone contactPhone in phones)
            {
                contactPhone.Phone = phoneNumberService.FormatPhoneNumberNational(contactPhone.Phone, contactPhone.Code.ToUpper());
            }
            //var contacts = await link.AppendPathSegment("Contact/ListContact/" + userSession)
            //   .WithSettings(s => s.JsonSerializer = serializer)
            //   .GetJsonAsync<List<ContactDTO>>();
            //var invalidPhones = new List<ContactPhone>();
            //var invalidEmails = new List<ContactEmail>();
            //phones = phones.GroupBy(e => e.Phone).Select(g => g.FirstOrDefault()).ToList();
            //emails = emails.GroupBy(e => e.Email).Select(g => g.FirstOrDefault()).ToList();
            //foreach (var contactDto in contacts)
            //{
            //    foreach (var phone in phones)
            //    {
            //        if (contactDto.ContactPhones.Any(e => e.Phone.Equals(phone.Phone)))
            //        {
            //            if (!invalidPhones.Any(e => e.Phone.Contains(phone.Phone)))
            //            {
            //                invalidPhones.Add(phone);
            //            }
            //        }
            //        if (contactDto.ContactPhones.Any(e => e.Phone.Equals(phone.Phone)))
            //        {
            //            if (!invalidPhones.Any(e => e.Phone.Contains(phone.Phone)))
            //            {
            //                invalidPhones.Add(phone);
            //            }
            //        }
            //    }

            //    phones = phones.Except(invalidPhones).ToList();
            //}
            //foreach (var contactDto in contacts)
            //{
            //    foreach (var email in emails)
            //    {
            //        if (contactDto.ContactEmails.Any(e => e.Email.Equals(email.Email)))
            //        {
            //            if (!invalidEmails.Contains(email))
            //            {
            //                invalidEmails.Add(email);
            //            }
            //        }
            //    }
            //    emails = emails.Except(invalidEmails).ToList();
            //}

            
            foreach (var phone in phones)
            {
                Console.WriteLine(phone.Phone);
            }
            foreach (var email in emails)
            {
                Console.WriteLine(email.Email);
            }
            contact.ContactsLabels = ContactLabels;
            contact.ContactPhones = phones;
            contact.ContactEmails = emails;
            contact.Firstname = firstName;
            contact.Lastname = lastName;
            contact.DateOfBirth = dob;
            contact.Notes = note;
            try
            {
                contact = await link.AppendPathSegments("Contact")
                    .WithOAuthBearerToken("token")
                    .WithSettings(s => s.JsonSerializer = serializer)
                    .PutJsonAsync(contact).ReceiveJson<ContactDTO>();
                return RedirectToPage("/Contact/index", new { id = contact.ContactId });
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
