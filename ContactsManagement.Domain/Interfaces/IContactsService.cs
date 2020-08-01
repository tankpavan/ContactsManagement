using ContactsManagement.Domain.Models;
using System.Collections.Generic;

namespace ContactsManagement.Domain.Interfaces
{
    public interface IContactsService
    {
        /// <summary>
        /// Add new contact
        /// </summary>
        /// <param name="contactModel">contact model</param>
        void AddContact(ContactModel contactModel);

        /// <summary>
        /// Update existing contact details
        /// </summary>
        /// <param name="contactModel">contact model</param>
        void UpdateContact(ContactModel contactModel);

        /// <summary>
        /// Delete/inactive contact
        /// </summary>
        /// <param name="contactModel">contact model</param>
        void DeleteContact(int contactId);

        /// <summary>
        /// Get all contacts
        /// </summary>
        /// <returns></returns>
        List<ContactModel> GetAllContacts();

        /// <summary>
        /// Get contact details by contact Id
        /// </summary>
        /// <param name="contactId">contactId of contact</param>
        /// <returns></returns>
        ContactModel GetContactById(int contactId);
    }
}
