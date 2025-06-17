using System;
using System.Collections.Generic;
using Entities;
using ServiceContracts.DTO;
using ServiceContracts;
using Services;
using Xunit;

namespace CRUDTests
{
    public class CountriesServiceTest
    {
        private readonly ICountriesService _countriesService;

        public CountriesServiceTest()
        {
            _countriesService = new CountriesService(false);
        }

        #region AddCountry

        [Fact]
        public void AddCountry_NullCountry()
        {
            CountryAddRequest? request = null;

            Assert.Throws<ArgumentNullException>(() =>
            {
                _countriesService.AddCountry(request);
            });
        }

        [Fact]
        public void AddCountry_CountryNameIsNull()
        {
            CountryAddRequest? request = new CountryAddRequest() { CountryName = null };

            Assert.Throws<ArgumentException>(() =>
            {
                _countriesService.AddCountry(request);
            });
        }

        [Fact]
        public void AddCountry_DuplicateCountryName()
        {
            CountryAddRequest? request1 = new CountryAddRequest() { CountryName = "USA" };
            CountryAddRequest? request2 = new CountryAddRequest() { CountryName = "USA" };

            Assert.Throws<ArgumentException>(() =>
            {
                _countriesService.AddCountry(request1);
                _countriesService.AddCountry(request2);
            });
        }

        [Fact]
        public void AddCountry_ProperCountryDetails()
        {
            CountryAddRequest? request = new CountryAddRequest() { CountryName = "Japan" };

            CountryResponse response = _countriesService.AddCountry(request);
            List<CountryResponse> countries_from_GetAllCountries = _countriesService.GetAllCountries();

            Assert.True(response.CountryID != Guid.Empty);
            Assert.Contains(response, countries_from_GetAllCountries);
        }

        #endregion

        #region GetAllCountries

        [Fact]

        public void GetAllCountries_EmptyList()
        {
            List<CountryResponse> actual_country_response_list = _countriesService.GetAllCountries();

            Assert.Empty(actual_country_response_list);
        }

        [Fact]
        public void GetAllCountries_AddFewCountries()
        {
            List<CountryAddRequest> country_request_list = new List<CountryAddRequest>() {
        new CountryAddRequest() { CountryName = "USA" },
        new CountryAddRequest() { CountryName = "UK" }
      };

            List<CountryResponse> countries_list_from_add_country = new List<CountryResponse>();

            foreach (CountryAddRequest country_request in country_request_list)
            {
                countries_list_from_add_country.Add(_countriesService.AddCountry(country_request));
            }

            List<CountryResponse> actualCountryResponseList = _countriesService.GetAllCountries();

            foreach (CountryResponse expected_country in countries_list_from_add_country)
            {
                Assert.Contains(expected_country, actualCountryResponseList);
            }
        }
        #endregion

        #region GetCountryByCountryID

        [Fact]

        public void GetCountryByCountryID_NullCountryID()
        {
            Guid? countrID = null;

            CountryResponse? country_response_from_get_method = _countriesService.GetCountryByCountryID(countrID);

            Assert.Null(country_response_from_get_method);
        }

        [Fact]

        public void GetCountryByCountryID_ValidCountryID()
        {
            CountryAddRequest? country_add_request = new CountryAddRequest() { CountryName = "China" };
            CountryResponse country_response_from_add = _countriesService.AddCountry(country_add_request);

            CountryResponse? country_response_from_get = _countriesService.GetCountryByCountryID(country_response_from_add.CountryID);

            Assert.Equal(country_response_from_add, country_response_from_get);
        }
        #endregion
    }
}
