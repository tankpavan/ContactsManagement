using System;
using System.Collections.Generic;

namespace ContactsManagement.DataAccess.Models
{
    public partial class Contacts
    {
        public int ContactId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public decimal? PhoneNumber { get; set; }
        public string Status { get; set; }
    }
}
