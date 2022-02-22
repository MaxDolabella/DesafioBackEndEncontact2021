using TesteBackendEnContact.Controllers.Models;

namespace TesteBackendEnContact.Core.Utils
{
    public class ContactParser
    {
        private const char SEPARATOR = ';';

        public bool TryParse(string line, out ParsedContact contact)
        {
            contact = default;
            var result = false;

            try
            {
                var data = line.Split(SEPARATOR, System.StringSplitOptions.TrimEntries);
                if (data.Length == 6
                    && !string.IsNullOrWhiteSpace(data[0])  // name
                    && !string.IsNullOrWhiteSpace(data[5])) // contactBookName
                {
                    //name;phone;email;address;company-name;contact-book
                    var name = data[0];
                    var phone = data[1];
                    var email = data[2];
                    var address = data[3];
                    var companyName = data[4];
                    var contactBookName = data[5];

                    contact = new ParsedContact(name, phone, email, address, companyName, contactBookName);
                    result = true;
                }
            }
            catch { }

            return result;
        }
    }
}