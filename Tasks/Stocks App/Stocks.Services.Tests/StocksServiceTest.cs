using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using RepositoryContracts;
using ServiceContracts.DTO;
using ServiceContracts.StocksService;
using Services;
using Services.StocksService;
using System.Diagnostics;
using System.Xml.Linq;

namespace Tests.ServiceTests
{
    public class StocksServiceTest
    {
        private readonly IBuyOrdersService _stocksBuyOrdersService;
        private readonly ISellOrdersService _stocksSellOrdersService;

        private readonly Mock<IStocksRepository> _stocksRepositoryMock;
        private readonly IStocksRepository _stocksRepository;

        private readonly IFixture _fixture;

        public StocksServiceTest()
        {
            _fixture = new Fixture();

            _stocksRepositoryMock = new Mock<IStocksRepository>();
            _stocksRepository = _stocksRepositoryMock.Object;

            _stocksBuyOrdersService = new StocksBuyOrdersService(_stocksRepository);
            _stocksSellOrdersService = new StocksSellOrdersService(_stocksRepository);
        }

        #region CreateBuyOrder

        [Fact]
        public async Task CreateBuyOrder_NullBuyOrder_ToBeArgumentNullException()
        {
            BuyOrderRequest? buyOrderRequest = null;

            BuyOrder buyOrderFixture = _fixture.Build<BuyOrder>()
             .Create();
            _stocksRepositoryMock.Setup(temp => temp.CreateBuyOrder(It.IsAny<BuyOrder>())).ReturnsAsync(buyOrderFixture);

            Func<Task> action = async () =>
            {
                await _stocksBuyOrdersService.CreateBuyOrder(buyOrderRequest);
            };

            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Theory]
        [InlineData(0)]
        public async Task CreateBuyOrder_QuantityIsLessThanMinimum_ToBeArgumentException(uint buyOrderQuantity)
        {
            BuyOrderRequest? buyOrderRequest = _fixture.Build<BuyOrderRequest>()
             .With(temp => temp.Quantity, buyOrderQuantity)
             .Create();

            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();
            _stocksRepositoryMock.Setup(temp => temp.CreateBuyOrder(It.IsAny<BuyOrder>())).ReturnsAsync(buyOrder);

            Func<Task> action = async () =>
            {
                await _stocksBuyOrdersService.CreateBuyOrder(buyOrderRequest);
            };

            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Theory]
        [InlineData(100001)]
        public async Task CreateBuyOrder_QuantityIsGreaterThanMaximum_ToBeArgumentException(uint buyOrderQuantity)
        {
            BuyOrderRequest? buyOrderRequest = _fixture.Build<BuyOrderRequest>()
             .With(temp => temp.Quantity, buyOrderQuantity)
             .Create();

            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();
            _stocksRepositoryMock.Setup(temp => temp.CreateBuyOrder(It.IsAny<BuyOrder>())).ReturnsAsync(buyOrder);

            Func<Task> action = async () =>
            {
                await _stocksBuyOrdersService.CreateBuyOrder(buyOrderRequest);
            };

            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Theory]
        [InlineData(0)]
        public async Task CreateBuyOrder_PriceIsLessThanMinimum_ToBeArgumentException(uint buyOrderPrice)
        {
            BuyOrderRequest? buyOrderRequest = _fixture.Build<BuyOrderRequest>()
             .With(temp => temp.Price, buyOrderPrice)
             .Create();

            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();
            _stocksRepositoryMock.Setup(temp => temp.CreateBuyOrder(It.IsAny<BuyOrder>())).ReturnsAsync(buyOrder);

            Func<Task> action = async () =>
            {
                await _stocksBuyOrdersService.CreateBuyOrder(buyOrderRequest);
            };

            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Theory]
        [InlineData(10001)]
        public async Task CreateBuyOrder_PriceIsGreaterThanMaximum_ToBeArgumentException(uint buyOrderPrice)
        {
            BuyOrderRequest? buyOrderRequest = _fixture.Build<BuyOrderRequest>()
             .With(temp => temp.Price, buyOrderPrice)
             .Create();

            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();
            _stocksRepositoryMock.Setup(temp => temp.CreateBuyOrder(It.IsAny<BuyOrder>())).ReturnsAsync(buyOrder);

            Func<Task> action = async () =>
            {
                await _stocksBuyOrdersService.CreateBuyOrder(buyOrderRequest);
            };

            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task CreateBuyOrder_StockSymbolIsNull_ToBeArgumentException()
        {
            BuyOrderRequest? buyOrderRequest = _fixture.Build<BuyOrderRequest>()
             .With(temp => temp.StockSymbol, null as string)
             .Create();

            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();
            _stocksRepositoryMock.Setup(temp => temp.CreateBuyOrder(It.IsAny<BuyOrder>())).ReturnsAsync(buyOrder);

            Func<Task> action = async () =>
            {
                await _stocksBuyOrdersService.CreateBuyOrder(buyOrderRequest);
            };

            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task CreateBuyOrder_DateOfOrderIsLessThanYear2000_ToBeArgumentException()
        {
            BuyOrderRequest? buyOrderRequest = _fixture.Build<BuyOrderRequest>()
             .With(temp => temp.DateAndTimeOfOrder, Convert.ToDateTime("1999-12-31"))
             .Create();

            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();
            _stocksRepositoryMock.Setup(temp => temp.CreateBuyOrder(It.IsAny<BuyOrder>())).ReturnsAsync(buyOrder);

            Func<Task> action = async () =>
            {
                await _stocksBuyOrdersService.CreateBuyOrder(buyOrderRequest);
            };

            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task CreateBuyOrder_ValidData_ToBeSuccessful()
        {
            BuyOrderRequest? buyOrderRequest = _fixture.Build<BuyOrderRequest>()
             .Create();

            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();
            _stocksRepositoryMock.Setup(temp => temp.CreateBuyOrder(It.IsAny<BuyOrder>())).ReturnsAsync(buyOrder);

            BuyOrderResponse buyOrderResponseFromCreate = await _stocksBuyOrdersService.CreateBuyOrder(buyOrderRequest);

            buyOrder.BuyOrderID = buyOrderResponseFromCreate.BuyOrderID;
            BuyOrderResponse buyOrderResponse_expected = buyOrder.ToBuyOrderResponse();
            buyOrderResponseFromCreate.BuyOrderID.Should().NotBe(Guid.Empty);
            buyOrderResponseFromCreate.Should().Be(buyOrderResponse_expected);
        }

        #endregion

        #region CreateSellOrder

        [Fact]
        public async Task CreateSellOrder_NullSellOrder_ToBeArgumentNullException()
        {
            SellOrderRequest? sellOrderRequest = null;

            SellOrder sellOrderFixture = _fixture.Build<SellOrder>()
             .Create();
            _stocksRepositoryMock.Setup(temp => temp.CreateSellOrder(It.IsAny<SellOrder>())).ReturnsAsync(sellOrderFixture);

            Func<Task> action = async () =>
            {
                await _stocksSellOrdersService.CreateSellOrder(sellOrderRequest);
            };

            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Theory]
        [InlineData(0)]
        public async Task CreateSellOrder_QuantityIsLessThanMinimum_ToBeArgumentException(uint sellOrderQuantity)
        {
            SellOrderRequest? sellOrderRequest = _fixture.Build<SellOrderRequest>()
             .With(temp => temp.Quantity, sellOrderQuantity)
             .Create();

            SellOrder sellOrder = sellOrderRequest.ToSellOrder();
            _stocksRepositoryMock.Setup(temp => temp.CreateSellOrder(It.IsAny<SellOrder>())).ReturnsAsync(sellOrder);

            Func<Task> action = async () =>
            {
                await _stocksSellOrdersService.CreateSellOrder(sellOrderRequest);
            };

            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Theory]
        [InlineData(100001)]
        public async Task CreateSellOrder_QuantityIsGreaterThanMaximum_ToBeArgumentException(uint sellOrderQuantity)
        {
            SellOrderRequest? sellOrderRequest = _fixture.Build<SellOrderRequest>()
             .With(temp => temp.Quantity, sellOrderQuantity)
             .Create();

            SellOrder sellOrder = sellOrderRequest.ToSellOrder();
            _stocksRepositoryMock.Setup(temp => temp.CreateSellOrder(It.IsAny<SellOrder>())).ReturnsAsync(sellOrder);

            Func<Task> action = async () =>
            {
                await _stocksSellOrdersService.CreateSellOrder(sellOrderRequest);
            };

            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Theory]
        [InlineData(0)]
        public async Task CreateSellOrder_PriceIsLessThanMinimum_ToBeArgumentException(uint sellOrderPrice)
        {
            SellOrderRequest? sellOrderRequest = _fixture.Build<SellOrderRequest>()
             .With(temp => temp.Price, sellOrderPrice)
             .Create();

            SellOrder sellOrder = sellOrderRequest.ToSellOrder();
            _stocksRepositoryMock.Setup(temp => temp.CreateSellOrder(It.IsAny<SellOrder>())).ReturnsAsync(sellOrder);

            Func<Task> action = async () =>
            {
                await _stocksSellOrdersService.CreateSellOrder(sellOrderRequest);
            };

            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Theory]
        [InlineData(10001)]
        public async Task CreateSellOrder_PriceIsGreaterThanMaximum_ToBeArgumentException(double buyOrderPrice)
        {
            SellOrderRequest? sellOrderRequest = _fixture.Build<SellOrderRequest>()
             .With(temp => temp.Price, buyOrderPrice)
             .Create();

            SellOrder sellOrder = sellOrderRequest.ToSellOrder();
            _stocksRepositoryMock.Setup(temp => temp.CreateSellOrder(It.IsAny<SellOrder>())).ReturnsAsync(sellOrder);

            Func<Task> action = async () =>
            {
                await _stocksSellOrdersService.CreateSellOrder(sellOrderRequest);
            };

            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task CreateSellOrder_StockSymbolIsNull_ToBeArgumentException()
        {
            SellOrderRequest? sellOrderRequest = _fixture.Build<SellOrderRequest>()
             .With(temp => temp.StockSymbol, null as string)
             .Create();

            SellOrder sellOrder = sellOrderRequest.ToSellOrder();
            _stocksRepositoryMock.Setup(temp => temp.CreateSellOrder(It.IsAny<SellOrder>())).ReturnsAsync(sellOrder);

            Func<Task> action = async () =>
            {
                await _stocksSellOrdersService.CreateSellOrder(sellOrderRequest);
            };

            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task CreateSellOrder_DateOfOrderIsLessThanYear2000_ToBeArgumentException()
        {
            SellOrderRequest? sellOrderRequest = _fixture.Build<SellOrderRequest>()
             .With(temp => temp.DateAndTimeOfOrder, Convert.ToDateTime("1999-12-31"))
             .Create();

            SellOrder sellOrder = sellOrderRequest.ToSellOrder();
            _stocksRepositoryMock.Setup(temp => temp.CreateSellOrder(It.IsAny<SellOrder>())).ReturnsAsync(sellOrder);

            Func<Task> action = async () =>
            {
                await _stocksSellOrdersService.CreateSellOrder(sellOrderRequest);
            };

            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task CreateSellOrder_ValidData_ToBeSuccessful()
        {
            SellOrderRequest? sellOrderRequest = _fixture.Build<SellOrderRequest>()
             .Create();

            SellOrder sellOrder = sellOrderRequest.ToSellOrder();
            _stocksRepositoryMock.Setup(temp => temp.CreateSellOrder(It.IsAny<SellOrder>())).ReturnsAsync(sellOrder);

            SellOrderResponse sellOrderResponseFromCreate = await _stocksSellOrdersService.CreateSellOrder(sellOrderRequest);

            sellOrder.SellOrderID = sellOrderResponseFromCreate.SellOrderID;
            SellOrderResponse sellOrderResponse_expected = sellOrder.ToSellOrderResponse();
            sellOrderResponseFromCreate.SellOrderID.Should().NotBe(Guid.Empty);
            sellOrderResponseFromCreate.Should().Be(sellOrderResponse_expected);
        }

        #endregion

        #region GetBuyOrders

        [Fact]
        public async Task GetAllBuyOrders_DefaultList_ToBeEmpty()
        {
            List<BuyOrder> buyOrders = new List<BuyOrder>();

            _stocksRepositoryMock.Setup(temp => temp.GetBuyOrders()).ReturnsAsync(buyOrders);

            List<BuyOrderResponse> buyOrdersFromGet = await _stocksBuyOrdersService.GetBuyOrders();

            Assert.Empty(buyOrdersFromGet);
        }

        [Fact]
        public async Task GetAllBuyOrders_WithFewBuyOrders_ToBeSuccessful()
        {
            List<BuyOrder> buyOrder_requests = new List<BuyOrder>() {
    _fixture.Build<BuyOrder>().Create(),
    _fixture.Build<BuyOrder>().Create()
   };

            List<BuyOrderResponse> buyOrders_list_expected = buyOrder_requests.Select(temp => temp.ToBuyOrderResponse()).ToList();
            List<BuyOrderResponse> buyOrder_response_list_from_add = new List<BuyOrderResponse>();

            _stocksRepositoryMock.Setup(temp => temp.GetBuyOrders()).ReturnsAsync(buyOrder_requests);

            List<BuyOrderResponse> buyOrders_list_from_get = await _stocksBuyOrdersService.GetBuyOrders();

            buyOrders_list_from_get.Should().BeEquivalentTo(buyOrders_list_expected);
        }

        #endregion

        #region GetSellOrders

        [Fact]
        public async Task GetAllSellOrders_DefaultList_ToBeEmpty()
        {
            List<SellOrder> sellOrders = new List<SellOrder>();

            _stocksRepositoryMock.Setup(temp => temp.GetSellOrders()).ReturnsAsync(sellOrders);

            List<SellOrderResponse> sellOrdersFromGet = await _stocksSellOrdersService.GetSellOrders();

            Assert.Empty(sellOrdersFromGet);
        }

        [Fact]
        public async Task GetAllSellOrders_WithFewSellOrders_ToBeSuccessful()
        {
            List<SellOrder> sellOrder_requests = new List<SellOrder>() {
    _fixture.Build<SellOrder>().Create(),
    _fixture.Build<SellOrder>().Create()
   };

            List<SellOrderResponse> sellOrders_list_expected = sellOrder_requests.Select(temp => temp.ToSellOrderResponse()).ToList();
            List<SellOrderResponse> sellOrder_response_list_from_add = new List<SellOrderResponse>();

            _stocksRepositoryMock.Setup(temp => temp.GetSellOrders()).ReturnsAsync(sellOrder_requests);

            List<SellOrderResponse> sellOrders_list_from_get = await _stocksSellOrdersService.GetSellOrders();

            sellOrders_list_from_get.Should().BeEquivalentTo(sellOrders_list_expected);
        }

        #endregion
    }
}
