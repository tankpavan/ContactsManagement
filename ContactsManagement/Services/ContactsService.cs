using ContactsManagement.Domain.Interfaces;
using ContactsManagement.Domain.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactsManagement.Services
{
    public class ContactsService : IContactsService
    {
        private readonly ILogger<ContactsService> _logger;
        private readonly IContactsRepository _contactsRepository;

        public ContactsService(ILogger<ContactsService> logger, IContactsRepository contactsRepository)
        {
            _contactsRepository = contactsRepository;
            _logger = logger;
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
                _contactsRepository.AddContact(contactModel);
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
                _contactsRepository.UpdateContact(contactModel);
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
                _contactsRepository.DeleteContact(contactId);
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
        /// <returns></returns>
        public List<ContactModel> GetAllContacts()
        {
            _logger.LogInformation("ENTER: " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            try
            {
                var contacts = _contactsRepository.GetAllContacts();
                return contacts;
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
        /// <returns></returns>
        public ContactModel GetContactById(int contactId)
        {
            _logger.LogInformation("ENTER: " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            try
            {
                var contact = _contactsRepository.GetContactById(contactId);
                return contact;
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
