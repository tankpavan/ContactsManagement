using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ContactsManagement.Domain.Interfaces;
using ContactsManagement.Domain.Models;

namespace ContactsManagement.Controllers
{
    [Route("api")]
    public class ContactsController : Controller
    {
        private readonly ILogger<ContactsController> _logger;
        private readonly IContactsService _contactsService;

        public ContactsController(ILogger<ContactsController> logger, IContactsService contactsService)
        {
            _contactsService = contactsService;
            _logger = logger;
        }

        /// <summary>
        /// Get all contacts
        /// </summary>
        /// <returns></returns>
        [HttpGet("contacts")]
        public IActionResult GetAllContacts()
        {
            _logger.LogInformation("ENTER: " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            try
            {
                var contacts = _contactsService.GetAllContacts();

                return Ok(contacts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
            finally
            {
                _logger.LogInformation("EXIT: " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        /// <summary>
        /// Get contact by given contact id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("contacts/{id}")]
        public IActionResult GetContactById(int id)
        {
            _logger.LogInformation("ENTER: " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            try
            {
                if (id <= 0)
                    return BadRequest("id should be greater than zero.");

                var contact = _contactsService.GetContactById(id);
                if (contact != null)
                    return Ok(contact);
                else
                    return StatusCode((int)HttpStatusCode.NotFound, "Contact not found with given contact id");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
            finally
            {
                _logger.LogInformation("EXIT: " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        /// <summary>
        /// Add contact
        /// </summary>
        /// <param name="contactModel">contact model</param>
        /// <returns></returns>
        [HttpPost("contacts")]
        public IActionResult AddContact([FromBody] ContactModel contactModel)
        {
            _logger.LogInformation("ENTER: " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            try
            {
                if (contactModel == null)
                    return BadRequest("Request doesn't contain a valid contact object");

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                _contactsService.AddContact(contactModel);
                if (contactModel.ContactId > 0)
                    return CreatedAtAction("AddContact", contactModel);
                else
                    return StatusCode((int)HttpStatusCode.InternalServerError, "Unable to create contact");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
            finally
            {
                _logger.LogInformation("EXIT: " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        /// <summary>
        /// update contact details
        /// </summary>
        /// <param name="id">contact id</param>
        /// <param name="contactModel">contact model</param>
        /// <returns></returns>
        [HttpPut("contacts/{id}")]
        public IActionResult UpdateContact(int id, [FromBody] ContactModel contactModel)
        {
            _logger.LogInformation("ENTER: " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            try
            {
                if (id <= 0)
                    return BadRequest("id should be greater than zero.");
                if (contactModel == null)
                    return BadRequest("Request doesn't contain a valid contact object");

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var contact = _contactsService.GetContactById(id);
                if (contact != null)
                {
                    _contactsService.UpdateContact(contactModel);
                    return Ok(contactModel);
                }
                else
                {
                    return NotFound("Contact not found with given id");
                }                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
            finally
            {
                _logger.LogInformation("EXIT: " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        /// <summary>
        /// Delete (Inactivate) contact
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("contacts/{id}")]
        public IActionResult DeleteContact(int id)
        {
            _logger.LogInformation("ENTER: " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            try
            {
                if (id <= 0)
                    return BadRequest("id should be greater than zero.");

                _contactsService.DeleteContact(id);
                return Ok(new { Message = "Contact deleted successfully"});
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
            finally
            {
                _logger.LogInformation("EXIT: " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
    }
}
