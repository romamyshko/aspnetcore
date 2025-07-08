using ServiceContracts.DTO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ServiceContracts
{
    public interface ICountriesUploaderService
    {
        Task<int> UploadCountriesFromExcelFile(IFormFile formFile);
    }
}
