using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using ContactsManagement.DataAccess.Models;
using ContactsManagement.Domain.Interfaces;
using ContactsManagement.Domain.Models;
using Microsoft.Extensions.Logging;

namespace ContactsManagement.DataAccess.Repositories
{
    public class ContactsRepository : IContactsRepository
    {
        private readonly ILogger<ContactsRepository> _logger;
        private ContactsManagementContext _context;
        private readonly IMapper _mapper; 

        public ContactsRepository(ILogger<ContactsRepository> logger, ContactsManagementContext context, IMapper mapper)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Add new contact
        /// </summary>
        /// <param name="contactModel">contact model</param>
        public void AddContact(ContactModel contactModel)
        {
            _logger.LogInformation("ENTER: " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            try
            {
                var contact = new Contacts();
                contact.FirstName = contactModel.FirstName;
                contact.LastName = contactModel.LastName;
                contact.Email = contactModel.Email;
                contact.PhoneNumber = contactModel.PhoneNumber;
                contact.Status = contactModel.Status;

                _context.Contacts.Add(contact);
                _context.SaveChanges();
                contactModel.ContactId = contact.ContactId;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw ex;
            }
            finally
            {
                _logger.LogInformation("EXIT: " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        /// <summary>
        /// Update existing contact details
        /// </summary>
        /// <param name="contactModel">contact model</param>
        public void UpdateContact(ContactModel contactModel)
        {
            _logger.LogInformation("ENTER: " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            try
            {
                var existingContact = _context.Contacts.Find(contactModel.ContactId);
                if (existingContact != null)
                {
                    existingContact.FirstName = contactModel.FirstName;
                    existingContact.LastName = contactModel.LastName;
                    existingContact.Email = contactModel.Email;
                    existingContact.PhoneNumber = contactModel.PhoneNumber;
                    existingContact.Status = contactModel.Status;

                    _context.SaveChanges();
                }
                else
                {
                    _logger.LogError("Contact not exist");
                    throw new Exception("Contact not exist");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw ex;
            }
            finally
            {
                _logger.LogInformation("EXIT: " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        /// <summary>
        /// Delete/inactive contact
        /// </summary>
        /// <param name="contactModel">contact model</param>
        public void DeleteContact(int contactId)
        {
            _logger.LogInformation("ENTER: " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            try
            {
                var existingContact = _context.Contacts.Find(contactId);
                if (existingContact != null)
                {
                    existingContact.Status = "InActive";

                    _context.SaveChanges();
                }
                else
                {
                    _logger.LogError("Contact not exist with given contact Id");
                    throw new Exception("Contact not exist with given contact Id");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw ex;
            }
            finally
            {
                _logger.LogInformation("EXIT: " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        /// <summary>
        /// Get all contacts
        /// </summary>
        /// <returns>List of ContactModel</returns>
        public List<ContactModel> GetAllContacts()
        {
            _logger.LogInformation("ENTER: " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            try
            {
                var contacts = _context.Contacts.Where(c => c.Status == "Active").ToList();

                var contactModels = _mapper.Map<List<ContactModel>>(contacts);

                return contactModels;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw ex;
            }
            finally
            {
                _logger.LogInformation("EXIT: " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        /// <summary>
        /// Get contact details by contact Id
        /// </summary>
        /// <param name="contactId">contactId of contact</param>
        /// <returns>contact model</returns>
        public ContactModel GetContactById(int contactId)
        {
            _logger.LogInformation("ENTER: " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            try
            {
                var contact = _context.Contacts.Find(contactId);

                var contactModel = _mapper.Map<ContactModel>(contact);

                return contactModel;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw ex;
            }
            finally
            {
                _logger.LogInformation("EXIT: " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        
    }
}
