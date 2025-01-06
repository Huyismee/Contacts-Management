using PhoneNumbers;

namespace WebApp.Service;

public class PhoneNumberService
{
    private readonly PhoneNumberUtil phoneNumberUtil;

    public PhoneNumberService()
    {
        phoneNumberUtil = PhoneNumberUtil.GetInstance();
    }

    public bool ValidatePhoneNumber(string phoneNumber, string countryCode)
    {
        try
        {
            var checkPhoneNumber = phoneNumberUtil.Parse(phoneNumber, countryCode);
            var isValid = phoneNumberUtil.IsValidNumber(checkPhoneNumber);
            return isValid;
        }
        catch (NumberParseException e)
        {
            Console.WriteLine(e);
            return false;
        }
       
    }

    public string FormatPhoneNumberNational(string phoneNumberInput, string countryCode)
    {
        try
        {
            var phoneNumber = phoneNumberUtil.Parse(phoneNumberInput, countryCode);
            var formattedPhoneNumberNational = phoneNumberUtil.Format(phoneNumber, PhoneNumberFormat.NATIONAL);
            return formattedPhoneNumberNational;
        }
        catch (NumberParseException e)
        {
            Console.WriteLine(e);
            return phoneNumberInput;
        }
       
    }
    public string FormatPhoneNumberInternational(string phoneNumberInput, string countryCode)
    {
        try
        {
            var phoneNumber = phoneNumberUtil.Parse(phoneNumberInput, countryCode);
            var formattedPhoneNumberNational = phoneNumberUtil.Format(phoneNumber, PhoneNumberFormat.INTERNATIONAL);
            return formattedPhoneNumberNational;
        }
        catch (NumberParseException e)
        {
            Console.WriteLine(e);
            return phoneNumberInput;
        }
    }
}   