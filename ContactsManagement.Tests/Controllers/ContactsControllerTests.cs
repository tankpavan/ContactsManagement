using ContactsManagement.Controllers;
using ContactsManagement.Domain.Interfaces;
using ContactsManagement.Domain.Models;
using ContactsManagement.Services;
using ContactsManagement.Tests.Repositories;
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
        IContactsRepository _contactsRepository;
        IContactsService _contactsService;
        ILogger<ContactsController> _contactsControllerLogger;
        ILogger<ContactsService> _contactsServiceLogger;

        public ContactsControllerTests()
        {
            var serviceProvider = new ServiceCollection().AddLogging().BuildServiceProvider();

            var factory = serviceProvider.GetService<ILoggerFactory>();

            _contactsControllerLogger = factory.CreateLogger<ContactsController>();
            _contactsServiceLogger = factory.CreateLogger<ContactsService>();
            _contactsRepository = new ContactsRepositoryFake();
            _contactsService = new ContactsService(_contactsServiceLogger, _contactsRepository);
            _contactsController = new ContactsController(_contactsControllerLogger, _contactsService);
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
        public void DeleteContact_ReturnsBadRequest()
        {
            var contact = _contactsController.DeleteContact(0) as BadRequestObjectResult;
            Assert.IsInstanceOf(typeof(BadRequestObjectResult), contact);
            Assert.AreEqual(400, contact.StatusCode);
            Assert.AreEqual("id should be greater than zero.", contact.Value.ToString());
        }

        [Test]
        public void DeleteContact_ReturnsOkObjectResult()
        {
            var contact = _contactsController.DeleteContact(2) as OkObjectResult;
            Assert.AreEqual(200, contact.StatusCode);
            Assert.AreEqual("Contact deleted successfully", contact.Value.GetType().GetProperty("Message").GetValue(contact.Value, null));
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
            var contact = _contactsController.UpdateContact(1, contactModel) as OkObjectResult;
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

            var contact = _contactsController.UpdateContact(1, contactModel) as BadRequestObjectResult;
            Assert.IsInstanceOf(typeof(BadRequestObjectResult), contact);
            Assert.AreEqual(400, contact.StatusCode);
        }
    }
}