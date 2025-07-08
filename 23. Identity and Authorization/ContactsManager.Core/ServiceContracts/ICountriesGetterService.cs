using ServiceContracts.DTO;
using System.Threading.Tasks;

namespace ServiceContracts
{
    public interface ICountriesGetterService
    {
        Task<List<CountryResponse>> GetAllCountries();

        Task<CountryResponse?> GetCountryByCountryID(Guid? countryID);
    }
}
