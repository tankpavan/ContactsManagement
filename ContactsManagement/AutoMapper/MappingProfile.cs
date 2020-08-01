using AutoMapper;
using ContactsManagement.DataAccess.Models;
using ContactsManagement.Domain.Models;

namespace ContactsManagement.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Contacts, ContactModel>();
        }
    }
}
