using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Linq;
using System.Reflection;
using PRN231_FinalProject.DTO;

namespace WebApp.Service;

public class ExcelExportService
{
    public void ExportFlatExcel(List<ContactDTO> contacts)
    {
        ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
        using (var package = new ExcelPackage())
        {
            if (!contacts.Any())
            {
                return;
            }
            var workSheet = package.Workbook.Worksheets.Add("Sheet1");

            // setting the properties 
            // of the work sheet  
            workSheet.TabColor = System.Drawing.Color.Black;
            workSheet.DefaultRowHeight = 12;

            // Setting the properties 
            // of the first row 
            workSheet.Row(1).Height = 20;
            workSheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Row(1).Style.Font.Bold = true;

            var mostEmailContact = contacts.MaxBy(e => e.ContactEmails.Count);
            var MostPhoneContact = contacts.MaxBy(e => e.ContactPhones.Count);
            // Header of the Excel sheet 
            workSheet.Cells[1, 1].Value = "Firstname";
            workSheet.Cells[1, 2].Value = "Lastname";
            workSheet.Cells[1, 3].Value = "DoB";
            workSheet.Cells[1, 4].Value = "Notes";
            int count = 5;
            int i = 1;
            if (mostEmailContact != null)
            {
                foreach (var email in mostEmailContact.ContactEmails)
                {
                    workSheet.Cells[1, count].Value = "EmailAddress" + i;
                    workSheet.Cells[1, count + 1].Value = "EmnailLabel" + i;
                    i++;
                    count += 2;
                }
            }
            

            i = 1;
            if (MostPhoneContact != null)
            {
                foreach (var phone in MostPhoneContact.ContactPhones)
                {
                    workSheet.Cells[1, count].Value = "PhoneNumber" + i;
                    workSheet.Cells[1, count + 1].Value = "PhoneCode" + i;
                    workSheet.Cells[1, count + 2].Value = "PhoneLabel" + i;
                    i++;
                    count += 3;
                }
            }
            
            // Inserting the article data into excel 
            // sheet by using the for each loop 
            // As we have values to the first row  
            // we will start with second row 
            int recordIndex = 2;

            foreach (var contact in contacts)
            {
                workSheet.Cells[recordIndex, 1].Value = contact.Firstname;
                workSheet.Cells[recordIndex, 2].Value = contact.Lastname;
                workSheet.Cells[recordIndex, 3].Value = contact.DateOfBirth.Value.ToString("MM/dd/yyyy");
                workSheet.Cells[recordIndex, 4].Value = contact.Notes;
                int k = 5;
                if (mostEmailContact != null)
                {
                    foreach (var email in contact.ContactEmails)
                    {
                        for (int j = k; j < count; j++)
                        {
                            if (workSheet.Cells[1, j].Text.Contains("EmailAddress"))
                            {
                                workSheet.Cells[recordIndex, j].Value = email.Email;
                                workSheet.Cells[recordIndex, j + 1].Value = email.Label;
                                j += 1;
                                k += 2;
                                break;
                            }

                        }
                    }
                }

                if (mostEmailContact != null)
                {
                    foreach (var phone in contact.ContactPhones)
                    {
                        for (int j = k; j < count; j++)
                        {
                            if (workSheet.Cells[1, j].Text.Contains("PhoneNumber"))
                            {
                                workSheet.Cells[recordIndex, j].Value = phone.Phone;
                                workSheet.Cells[recordIndex, j + 1].Value = phone.Code;
                                workSheet.Cells[recordIndex, j + 2].Value = phone.Label;
                                j += 2;
                                k += 3;
                                break;
                            }
                        }
                    }
                }

                

                recordIndex++;
            }

            // By default, the column width is not  
            // set to auto fit for the content 
            // of the range, so we are using 
            // AutoFit() method here.  
            workSheet.Columns.AutoFit();


            // file name with .xlsx extension  
            string p_strPath = "wwwroot/contacts.xlsx";

            if (File.Exists(p_strPath))
                File.Delete(p_strPath);

            // Create excel file on physical disk  
            FileStream objFileStrm = File.Create(p_strPath);
            objFileStrm.Close();

            // Write content to excel file  
            File.WriteAllBytes(p_strPath, package.GetAsByteArray());
            //Close Excel package 
            package.Dispose();
        }
    }
}