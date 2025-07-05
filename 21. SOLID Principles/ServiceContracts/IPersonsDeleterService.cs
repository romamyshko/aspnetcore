using System;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace ServiceContracts
{
    public interface IPersonsDeleterService
    {
        Task<bool> DeletePerson(Guid? personID);
    }
}
