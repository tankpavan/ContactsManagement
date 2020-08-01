using ContactsManagement.Controllers;
using ContactsManagement.Domain.Interfaces;
using ContactsManagement.Domain.Models;
using ContactsManagement.Tests.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using System.Collections.Generic;

namespace ContactsManagement.Tests.Controllers
{
    public class ContactsControllerTests
    {
        ContactsController _contactsController;
        IContactsService _contactsService;
        ILogger<ContactsController> _logger;

        public ContactsControllerTests()
        {
            var serviceProvider = new ServiceCollection().AddLogging().BuildServiceProvider();

            var factory = serviceProvider.GetService<ILoggerFactory>();

            _logger = factory.CreateLogger<ContactsController>();
            _contactsService = new ContactsServiceFake();
            _contactsController = new ContactsController(_logger, _contactsService);
        }

        [SetUp]
        public void SetUp()
        {
            _contactsController.ModelState.Clear();
        }
        
        [Test]
        public void GetAllContacts_ReturnsAllContacts()
        {
            var contacts = _contactsController.GetAllContacts() as OkObjectResult;
            Assert.IsInstanceOf(typeof(List<ContactModel>), contacts.Value);
            Assert.AreEqual(200, contacts.StatusCode);
            Assert.AreEqual(4, (contacts.Value as List<ContactModel>).Count);
        }

        [Test]
        public void GetContactById_ReturnsExistingContact()
        {
            var contact = _contactsController.GetContactById(1) as OkObjectResult;
            Assert.IsInstanceOf(typeof(ContactModel), contact.Value);
            Assert.AreEqual(200, contact.StatusCode);
            Assert.AreEqual(1, (contact.Value as ContactModel).ContactId);
        }

        [Test]
        public void AddContact_InvalidObject_ReturnsBadRequest()
        {
            ContactModel contactModel = null;
            var contact = _contactsController.AddContact(contactModel) as BadRequestObjectResult;
            Assert.IsInstanceOf(typeof(BadRequestObjectResult), contact);
            Assert.AreEqual(400, contact.StatusCode);
        }

        [Test]
        public void AddContact_ValidObject_ReturnsCreatedContact()
        {
            ContactModel contactModel = new ContactModel()
            {
                FirstName = "First UnitTest",
                LastName = "Last UnitTest",
                Email = "firstunittest.lastunittest@test.com",
                PhoneNumber = 1111122222,
                Status = "Active"
            };
            var contact = _contactsController.AddContact(contactModel) as CreatedAtActionResult;
            Assert.IsInstanceOf(typeof(ContactModel), contact.Value);
            Assert.AreEqual(201, contact.StatusCode);
            Assert.Greater((contact.Value as ContactModel).ContactId, 0);
        }

        [Test]
        public void DeleteContact_ReturnsOkObjectResult()
        {
            var contact = _contactsController.DeleteContact(2) as OkObjectResult;
            Assert.IsInstanceOf(typeof(string), contact.Value);
            Assert.AreEqual(200, contact.StatusCode);
            Assert.AreEqual("Contact deleted successfully", contact.Value.ToString());
        }

        [Test]
        public void UpdateContact_ReturnsOkObjectResult()
        {
            ContactModel contactModel = new ContactModel()
            {
                ContactId = 1,
                FirstName = "First One Updated",
                LastName = "Last One Updated",
                Email = "firstone.lastone@test.com",
                PhoneNumber = 9999977777,
                Status = "Active"
            };            
            var contact = _contactsController.UpdateContact(contactModel) as OkObjectResult;
            Assert.IsInstanceOf(typeof(ContactModel), contact.Value);
            Assert.AreEqual(200, contact.StatusCode);
            Assert.AreEqual(1, (contact.Value as ContactModel).ContactId);
            Assert.AreEqual("First One Updated", (contact.Value as ContactModel).FirstName);
        }

        [Test]
        public void UpdateContact_InvalidObject_ReturnsBadRequest()
        {
            ContactModel contactModel = new ContactModel()
            {
                ContactId = 1,
                LastName = "Last One Updated",
                Email = "firstone.lastone@test.com",
                PhoneNumber = 9999977777,
                Status = "Active"
            };
            _contactsController.ModelState.AddModelError("FirstName", "Required");

            var contact = _contactsController.UpdateContact(contactModel) as BadRequestObjectResult;
            Assert.IsInstanceOf(typeof(BadRequestObjectResult), contact);
            Assert.AreEqual(400, contact.StatusCode);
        }
    }
}