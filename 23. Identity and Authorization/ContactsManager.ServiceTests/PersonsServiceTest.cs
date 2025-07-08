using System;
using System.Collections.Generic;
using Xunit;
using ServiceContracts;
using Entities;
using ServiceContracts.DTO;
using Services;
using ServiceContracts.Enums;
using Xunit.Abstractions;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using RepositoryContracts;
using Moq;
using System.Linq.Expressions;
using Serilog;
using Microsoft.Extensions.Logging;

namespace CRUDTests
{
    public class PersonsServiceTest
    {
        private readonly IPersonsGetterService _personsGetterService;
        private readonly IPersonsAdderService _personsAdderService;
        private readonly IPersonsUpdaterService _personsUpdaterService;
        private readonly IPersonsDeleterService _personsDeleterService;
        private readonly IPersonsSorterService _personsSorterService;

        private readonly Mock<IPersonsRepository> _personRepositoryMock;
        private readonly IPersonsRepository _personsRepository;

        private readonly ITestOutputHelper _testOutputHelper;
        private readonly IFixture _fixture;

        public PersonsServiceTest(ITestOutputHelper testOutputHelper)
        {
            _fixture = new Fixture();
            _personRepositoryMock = new Mock<IPersonsRepository>();
            _personsRepository = _personRepositoryMock.Object;

            var diagnosticContextMock = new Mock<IDiagnosticContext>();
            var loggerMock = new Mock<ILogger<PersonsGetterService>>();

            _personsGetterService = new PersonsGetterService(_personsRepository, loggerMock.Object, diagnosticContextMock.Object);

            _personsAdderService = new PersonsAdderService(_personsRepository, loggerMock.Object, diagnosticContextMock.Object);

            _personsDeleterService = new PersonsDeleterService(_personsRepository, loggerMock.Object, diagnosticContextMock.Object);

            _personsSorterService = new PersonsSorterService(_personsRepository, loggerMock.Object, diagnosticContextMock.Object);

            _personsUpdaterService = new PersonsUpdaterService(_personsRepository, loggerMock.Object, diagnosticContextMock.Object);

            _testOutputHelper = testOutputHelper;
        }

        #region AddPerson

        [Fact]
        public async Task AddPerson_NullPerson_ToBeArgumentNullException()
        {
            PersonAddRequest? personAddRequest = null;

            Func<Task> action = async () =>
            {
                await _personsAdderService.AddPerson(personAddRequest);
            };

            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task AddPerson_PersonNameIsNull_ToBeArgumentException()
        {
            PersonAddRequest? personAddRequest = _fixture.Build<PersonAddRequest>()
             .With(temp => temp.PersonName, null as string)
             .Create();

            Person person = personAddRequest.ToPerson();

            _personRepositoryMock
             .Setup(temp => temp.AddPerson(It.IsAny<Person>()))
             .ReturnsAsync(person);

            Func<Task> action = async () =>
            {
                await _personsAdderService.AddPerson(personAddRequest);
            };

            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task AddPerson_FullPersonDetails_ToBeSuccessful()
        {
            PersonAddRequest? personAddRequest = _fixture.Build<PersonAddRequest>()
             .With(temp => temp.Email, "someone@example.com")
             .Create();

            Person person = personAddRequest.ToPerson();
            PersonResponse person_response_expected = person.ToPersonResponse();

            _personRepositoryMock.Setup
             (temp => temp.AddPerson(It.IsAny<Person>()))
             .ReturnsAsync(person);

            PersonResponse person_response_from_add = await _personsAdderService.AddPerson(personAddRequest);

            person_response_expected.PersonID = person_response_from_add.PersonID;

            person_response_from_add.PersonID.Should().NotBe(Guid.Empty);
            person_response_from_add.Should().Be(person_response_expected);
        }

        #endregion

        #region GetPersonByPersonID

        [Fact]
        public async Task GetPersonByPersonID_NullPersonID_ToBeNull()
        {
            Guid? personID = null;

            PersonResponse? person_response_from_get = await _personsGetterService.GetPersonByPersonID(personID);

            person_response_from_get.Should().BeNull();
        }

        [Fact]
        public async Task GetPersonByPersonID_WithPersonID_ToBeSuccessful()
        {
            Person person = _fixture.Build<Person>()
             .With(temp => temp.Email, "email@sample.com")
             .With(temp => temp.Country, null as Country)
             .Create();
            PersonResponse person_response_expected = person.ToPersonResponse();

            _personRepositoryMock.Setup(temp => temp.GetPersonByPersonID(It.IsAny<Guid>()))
             .ReturnsAsync(person);

            PersonResponse? person_response_from_get = await _personsGetterService.GetPersonByPersonID(person.PersonID);

            person_response_from_get.Should().Be(person_response_expected);
        }

        #endregion

        #region GetAllPersons

        [Fact]
        public async Task GetAllPersons_ToBeEmptyList()
        {
            var persons = new List<Person>();
            _personRepositoryMock
             .Setup(temp => temp.GetAllPersons())
             .ReturnsAsync(persons);

            List<PersonResponse> persons_from_get = await _personsGetterService.GetAllPersons();

            persons_from_get.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllPersons_WithFewPersons_ToBeSuccessful()
        {
            List<Person> persons = new List<Person>() {
    _fixture.Build<Person>()
    .With(temp => temp.Email, "someone_1@example.com")
    .With(temp => temp.Country, null as Country)
    .Create(),

    _fixture.Build<Person>()
    .With(temp => temp.Email, "someone_2@example.com")
    .With(temp => temp.Country, null as Country)
    .Create(),

    _fixture.Build<Person>()
    .With(temp => temp.Email, "someone_3@example.com")
    .With(temp => temp.Country, null as Country)
    .Create()
   };

            List<PersonResponse> person_response_list_expected = persons.Select(temp => temp.ToPersonResponse()).ToList();

            _testOutputHelper.WriteLine("Expected:");
            foreach (PersonResponse person_response_from_add in person_response_list_expected)
            {
                _testOutputHelper.WriteLine(person_response_from_add.ToString());
            }

            _personRepositoryMock.Setup(temp => temp.GetAllPersons()).ReturnsAsync(persons);

            List<PersonResponse> persons_list_from_get = await _personsGetterService.GetAllPersons();

            _testOutputHelper.WriteLine("Actual:");
            foreach (PersonResponse person_response_from_get in persons_list_from_get)
            {
                _testOutputHelper.WriteLine(person_response_from_get.ToString());
            }

            persons_list_from_get.Should().BeEquivalentTo(person_response_list_expected);
        }
        #endregion

        #region GetFilteredPersons

        [Fact]
        public async Task GetFilteredPersons_EmptySearchText_ToBeSuccessful()
        {
            List<Person> persons = new List<Person>() {
    _fixture.Build<Person>()
    .With(temp => temp.Email, "someone_1@example.com")
    .With(temp => temp.Country, null as Country)
    .Create(),

    _fixture.Build<Person>()
    .With(temp => temp.Email, "someone_2@example.com")
    .With(temp => temp.Country, null as Country)
    .Create(),

    _fixture.Build<Person>()
    .With(temp => temp.Email, "someone_3@example.com")
    .With(temp => temp.Country, null as Country)
    .Create()
   };

            List<PersonResponse> person_response_list_expected = persons.Select(temp => temp.ToPersonResponse()).ToList();

            _testOutputHelper.WriteLine("Expected:");
            foreach (PersonResponse person_response_from_add in person_response_list_expected)
            {
                _testOutputHelper.WriteLine(person_response_from_add.ToString());
            }

            _personRepositoryMock.Setup(temp => temp
            .GetFilteredPersons(It.IsAny<Expression<Func<Person, bool>>>()))
             .ReturnsAsync(persons);

            List<PersonResponse> persons_list_from_search = await _personsGetterService.GetFilteredPersons(nameof(Person.PersonName), "");

            _testOutputHelper.WriteLine("Actual:");
            foreach (PersonResponse person_response_from_get in persons_list_from_search)
            {
                _testOutputHelper.WriteLine(person_response_from_get.ToString());
            }

            persons_list_from_search.Should().BeEquivalentTo(person_response_list_expected);
        }

        [Fact]
        public async Task GetFilteredPersons_SearchByPersonName_ToBeSuccessful()
        {
            List<Person> persons = new List<Person>() {
    _fixture.Build<Person>()
    .With(temp => temp.Email, "someone_1@example.com")
    .With(temp => temp.Country, null as Country)
    .Create(),

    _fixture.Build<Person>()
    .With(temp => temp.Email, "someone_2@example.com")
    .With(temp => temp.Country, null as Country)
    .Create(),

    _fixture.Build<Person>()
    .With(temp => temp.Email, "someone_3@example.com")
    .With(temp => temp.Country, null as Country)
    .Create()
   };

            List<PersonResponse> person_response_list_expected = persons.Select(temp => temp.ToPersonResponse()).ToList();

            _testOutputHelper.WriteLine("Expected:");
            foreach (PersonResponse person_response_from_add in person_response_list_expected)
            {
                _testOutputHelper.WriteLine(person_response_from_add.ToString());
            }

            _personRepositoryMock.Setup(temp => temp
            .GetFilteredPersons(It.IsAny<Expression<Func<Person, bool>>>()))
             .ReturnsAsync(persons);

            List<PersonResponse> persons_list_from_search = await _personsGetterService.GetFilteredPersons(nameof(Person.PersonName), "sa");

            _testOutputHelper.WriteLine("Actual:");
            foreach (PersonResponse person_response_from_get in persons_list_from_search)
            {
                _testOutputHelper.WriteLine(person_response_from_get.ToString());
            }

            persons_list_from_search.Should().BeEquivalentTo(person_response_list_expected);
        }

        #endregion

        #region GetSortedPersons

        [Fact]
        public async Task GetSortedPersons_ToBeSuccessful()
        {
            List<Person> persons = new List<Person>() {
    _fixture.Build<Person>()
    .With(temp => temp.Email, "someone_1@example.com")
    .With(temp => temp.Country, null as Country)
    .Create(),

    _fixture.Build<Person>()
    .With(temp => temp.Email, "someone_2@example.com")
    .With(temp => temp.Country, null as Country)
    .Create(),

    _fixture.Build<Person>()
    .With(temp => temp.Email, "someone_3@example.com")
    .With(temp => temp.Country, null as Country)
    .Create()
   };

            List<PersonResponse> person_response_list_expected = persons.Select(temp => temp.ToPersonResponse()).ToList();

            _personRepositoryMock
             .Setup(temp => temp.GetAllPersons())
             .ReturnsAsync(persons);

            _testOutputHelper.WriteLine("Expected:");
            foreach (PersonResponse person_response_from_add in person_response_list_expected)
            {
                _testOutputHelper.WriteLine(person_response_from_add.ToString());
            }
            List<PersonResponse> allPersons = await _personsGetterService.GetAllPersons();

            List<PersonResponse> persons_list_from_sort = await _personsSorterService.GetSortedPersons(allPersons, nameof(Person.PersonName), SortOrderOptions.DESC);

            _testOutputHelper.WriteLine("Actual:");
            foreach (PersonResponse person_response_from_get in persons_list_from_sort)
            {
                _testOutputHelper.WriteLine(person_response_from_get.ToString());
            }

            persons_list_from_sort.Should().BeInDescendingOrder(temp => temp.PersonName);
        }
        #endregion

        #region UpdatePerson

        [Fact]
        public async Task UpdatePerson_NullPerson_ToBeArgumentNullException()
        {
            PersonUpdateRequest? person_update_request = null;

            Func<Task> action = async () =>
            {
                await _personsUpdaterService.UpdatePerson(person_update_request);
            };

            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task UpdatePerson_InvalidPersonID_ToBeArgumentException()
        {
            PersonUpdateRequest? person_update_request = _fixture.Build<PersonUpdateRequest>()
             .Create();

            Func<Task> action = async () =>
            {
                await _personsUpdaterService.UpdatePerson(person_update_request);
            };

            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task UpdatePerson_PersonNameIsNull_ToBeArgumentException()
        {
            Person person = _fixture.Build<Person>()
             .With(temp => temp.PersonName, null as string)
             .With(temp => temp.Email, "someone@example.com")
             .With(temp => temp.Country, null as Country)
             .With(temp => temp.Gender, "Male")
             .Create();

            PersonResponse person_response_from_add = person.ToPersonResponse();

            PersonUpdateRequest person_update_request = person_response_from_add.ToPersonUpdateRequest();

            var action = async () =>
            {
                await _personsUpdaterService.UpdatePerson(person_update_request);
            };

            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task UpdatePerson_PersonFullDetails_ToBeSuccessful()
        {
            Person person = _fixture.Build<Person>()
             .With(temp => temp.Email, "someone@example.com")
             .With(temp => temp.Country, null as Country)
             .With(temp => temp.Gender, "Male")
             .Create();

            PersonResponse person_response_expected = person.ToPersonResponse();

            PersonUpdateRequest person_update_request = person_response_expected.ToPersonUpdateRequest();

            _personRepositoryMock
             .Setup(temp => temp.UpdatePerson(It.IsAny<Person>()))
             .ReturnsAsync(person);

            _personRepositoryMock
             .Setup(temp => temp.GetPersonByPersonID(It.IsAny<Guid>()))
             .ReturnsAsync(person);

            PersonResponse person_response_from_update = await _personsUpdaterService.UpdatePerson(person_update_request);

            person_response_from_update.Should().Be(person_response_expected);
        }

        #endregion

        #region DeletePerson

        [Fact]
        public async Task DeletePerson_ValidPersonID_ToBeSuccessful()
        {
            Person person = _fixture.Build<Person>()
             .With(temp => temp.Email, "someone@example.com")
             .With(temp => temp.Country, null as Country)
             .With(temp => temp.Gender, "Female")
             .Create();

            _personRepositoryMock
             .Setup(temp => temp.DeletePersonByPersonID(It.IsAny<Guid>()))
             .ReturnsAsync(true);

            _personRepositoryMock
             .Setup(temp => temp.GetPersonByPersonID(It.IsAny<Guid>()))
             .ReturnsAsync(person);

            bool isDeleted = await _personsDeleterService.DeletePerson(person.PersonID);

            isDeleted.Should().BeTrue();
        }

        [Fact]
        public async Task DeletePerson_InvalidPersonID()
        {
            bool isDeleted = await _personsDeleterService.DeletePerson(Guid.NewGuid());

            isDeleted.Should().BeFalse();
        }

        #endregion
    }
}
