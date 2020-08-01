using ContactsManagement.Domain.Interfaces;
using ContactsManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ContactsManagement.Tests.Services
{
    public class ContactsServiceFake : IContactsService
    {
        private List<ContactModel> _contactModels;

        public ContactsServiceFake()
        {
            _contactModels = new List<ContactModel>()
            {
                new ContactModel() { ContactId = 1, FirstName = "First One", LastName = "Last One", Email = "firstone.lastone@test.com", PhoneNumber = 9999988888, Status = "Active" },
                new ContactModel() { ContactId = 2, FirstName = "First Two", LastName = "Last Two", Email = "firsttwo.lasttwo@test.com", PhoneNumber = 8888877777, Status = "Active" },
                new ContactModel() { ContactId = 3, FirstName = "First Three", LastName = "Last Three", Email = "firstthree.lastthree@test.com", PhoneNumber = 7777766666, Status = "Active" }
            };
        }

        public void AddContact(ContactModel contactModel)
        {
            contactModel.ContactId = _contactModels.Count + 1;
            _contactModels.Add(contactModel);
        }

        public void DeleteContact(int contactId)
        {
            var contact = _contactModels.Where(c => c.ContactId == contactId).FirstOrDefault();
            contact.Status = "Inactive";
        }

        public List<ContactModel> GetAllContacts()
        {
            return _contactModels;
        }

        public ContactModel GetContactById(int contactId)
        {
            return _contactModels.Where(c => c.ContactId == contactId).FirstOrDefault();
        }

        public void UpdateContact(ContactModel contactModel)
        {
            var contact = _contactModels.Where(c => c.ContactId == contactModel.ContactId).FirstOrDefault();
            var contactIndex = _contactModels.FindIndex(c => c.ContactId == contactModel.ContactId);
            contact.FirstName = contactModel.FirstName;
            contact.LastName = contactModel.LastName;
            contact.Email = contactModel.Email;
            contact.PhoneNumber = contactModel.PhoneNumber;
            _contactModels[contactIndex] = contact; 
        }
    }
}
