using System;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace ServiceContracts
{
    public interface IPersonsAdderService
    {
        Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest);
    }
}
