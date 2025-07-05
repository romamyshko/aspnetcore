using ServiceContracts.DTO;
using System.Threading.Tasks;

namespace ServiceContracts
{
    public interface ICountriesAdderService
    {
        Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest);
    }
}
