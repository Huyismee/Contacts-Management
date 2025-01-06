using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Reflection;
using Flurl;
using OfficeOpenXml;
using PRN231_FinalProject.DTO;
using PRN231_FinalProject.Models;
using WebApp.Service;
using LicenseContext = System.ComponentModel.LicenseContext;
using Flurl.Http;

namespace WebApp.Pages.Contact
{
    public class InsertModel : PageModel
    {
        public PhoneNumberService PhoneNumberService = new PhoneNumberService();

        [BindProperty] public IFormFile inputSessions { get; set; }
        private string link = "http://localhost:5126/api";

        public IActionResult OnGet()
        {
            var userSession = HttpContext.Session.GetInt32("UserSession");
            if (userSession == null)
            {
                return RedirectToPage("/Login/Index", new { Message = "You must login first" });
            }

            return Page();
        }
        public async Task<IActionResult> OnPostInsert()
        {
            var userSession = HttpContext.Session.GetInt32("UserSession");

            if (inputSessions != null && inputSessions.Length > 0)
            {
                using (var stream = new MemoryStream())
                {
                    inputSessions.CopyTo(stream);
                    ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                    using (var package = new ExcelPackage(stream))
                    {
                        var workbook = package.Workbook;
                        var worksheet = workbook.Worksheets[0]; // Assuming data is in the first sheet
                        var excelDataList = new List<ContactDTO>();
                        LabelDTO label = new LabelDTO()
                        {
                            UserId = userSession.Value,
                            LabelName = "Created at " + DateTime.Now.ToString("MM/dd/yyyy")
                        };
                        try
                        {
                            var serializer = new Flurl.Http.Newtonsoft.NewtonsoftJsonSerializer();
                            label = await link.AppendPathSegments("Label")
                                .WithOAuthBearerToken("token")
                                .WithSettings(s => s.JsonSerializer = serializer)
                                .PostJsonAsync(label).ReceiveJson<LabelDTO>();
                        }
                        catch (FlurlHttpException ex)
                        {
                            var err = await ex.GetResponseStringAsync(); // or GetResponseStringAsync(), etc.
                            Console.WriteLine($"Error returned from {ex.Call.Request.Url}: {err}");


                        }
                        for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
                        {
                            var excelData = new ContactDTO
                            {
                                Firstname = worksheet.Cells[row, 1].Text,
                                Lastname = worksheet.Cells[row, 2].Text,
                                DateOfBirth = DateTime.Parse(worksheet.Cells[row, 3].Text),
                                Notes = worksheet.Cells[row, 4].Text,
                                
                                Favorite = false,
                                UserId = userSession.Value,
                                ContactsLabels = new List<ContactsLabel>(),
                                ContactEmails = new List<ContactEmail>(),
                                ContactPhones = new List<ContactPhone>()

                                // Assign values to properties based on the column index
                            };
                            for (int i = 5; i <= worksheet.Dimension.End.Column; i++)
                            {
                                if (worksheet.Cells[1, i].Text.Contains("EmailAddress"))
                                {
                                    if (!string.IsNullOrEmpty(worksheet.Cells[row, i].Text))
                                    {
                                        excelData.ContactEmails.Add(new ContactEmail()
                                        {
                                            Email = worksheet.Cells[row, i].Text,
                                            Label = worksheet.Cells[row, i += 1].Text
                                        });
                                    }
                                }
                                else if (worksheet.Cells[1, i].Text.Contains("PhoneNumber"))
                                {
                                    if (!string.IsNullOrEmpty(worksheet.Cells[row, i].Text))
                                    {
                                        excelData.ContactPhones.Add(new ContactPhone()
                                        {
                                            Phone = PhoneNumberService.ValidatePhoneNumber(worksheet.Cells[row, i].Text, worksheet.Cells[row, i + 1].Text.ToUpper()) == true ? PhoneNumberService.FormatPhoneNumberNational(worksheet.Cells[row, i].Text, worksheet.Cells[row, i + 1].Text.ToUpper()) : (worksheet.Cells[row, i].Text),
                                            Code = worksheet.Cells[row, i += 1].Text,
                                            Label = worksheet.Cells[row, i += 1].Text
                                        });
                                    }

                                }
                            }
                            //List<ContactEmail> email = new List<ContactEmail>();
                            //List<ContactPhone> phone = new List<ContactPhone>();
                            //email = JsonConvert.DeserializeObject<List<ContactEmail>>(worksheet.Cells[row, 5].Text);
                            //phone = JsonConvert.DeserializeObject<List<ContactPhone>>(worksheet.Cells[row, 6].Text);
                            //excelData.ContactEmails = email;
                            //excelData.ContactPhones = phone;
                            excelData.ContactsLabels.Add(new ContactsLabel()
                            {
                                LabelId = label.LabelId
                            });

                            excelDataList.Add(excelData);
                        }

                        //Now you have a list of ExcelData objects that you can use
                        foreach (var contact in excelDataList)
                        {
                            string test = JsonConvert.SerializeObject(contact);
                            Console.WriteLine(test);
                        }
                        try
                        {
                            var serializer = new Flurl.Http.Newtonsoft.NewtonsoftJsonSerializer();
                            var contacts = await link.AppendPathSegments("Contact/AddRange")
                                .WithOAuthBearerToken("token")
                                .WithSettings(s => s.JsonSerializer = serializer)
                                .PostJsonAsync(excelDataList).ReceiveJson<List<ContactDTO>>();
                        }
                        catch (FlurlHttpException ex)
                        {
                            var err = await ex.GetResponseStringAsync(); // or GetResponseStringAsync(), etc.
                            Console.WriteLine($"Error returned from {ex.Call.Request.Url}: {err}");

                        }

                        //TempData["List"] = JsonConvert.SerializeObject(ValidSessions);
                    }
                }
            }

            return RedirectToPage("/index");
        }

        //public void OnPostApprove()
        //{
        //    List<Models.CourseSession> insertSessions = new List<Models.CourseSession>();
        //    if (TempData["List"] is string serializedList)
        //    {
        //        insertSessions = JsonConvert.DeserializeObject<List<Models.CourseSession>>(serializedList);
        //    }

        //    _context.CourseSessions.AddRange(insertSessions);
        //    _context.SaveChanges();
        //    vadidateSchedule.addSchedule();
        //    TempData.Clear();
        //}
    }
}
