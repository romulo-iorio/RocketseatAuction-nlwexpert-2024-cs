using Bogus;
using FluentAssertions;
using Moq;
using RocketseatAuction.API.Contracts;
using RocketseatAuction.API.Entities;
using RocketseatAuction.API.Enums;
using RocketseatAuction.API.UseCases.Auctions.GetCurrent;
using Xunit;

namespace UseCases.Test.Auctions.GetCurrent;

public class GetCurrentAuctionUseCaseTest
{
    public Auction GetTestAuction()
    {
        var auctionId = new Faker().Random.Number(1, 100);

        var testItem = new Faker<Item>()
            .RuleFor(item => item.Id, f => f.Random.Number(1, 100))
            .RuleFor(item => item.Name, f => f.Commerce.ProductName())
            .RuleFor(item => item.Brand, f => f.Commerce.Department())
            .RuleFor(item => item.BasePrice, f => f.Random.Decimal(50, 1000))
            .RuleFor(item => item.Condition, f => f.PickRandom<Condition>())
            .RuleFor(item => item.AuctionId, f => auctionId)
            .Generate();

        return new Faker<Auction>()
            .RuleFor(auction => auction.Id, f => auctionId)
            .RuleFor(auction => auction.Name, f => f.Lorem.Word())
            .RuleFor(auction => auction.Starts, f => f.Date.Past())
            .RuleFor(auction => auction.Ends, f => f.Date.Future())
            .RuleFor(auction => auction.Items, f => new List<Item> { testItem })
            .Generate();
    }

    [Fact]
    public void Success()
    {
        // Arrange
        var testAuction = GetTestAuction();

        var mock = new Mock<IAuctionRepository>();
        mock.Setup(i => i.GetCurrent()).Returns(testAuction);

        var useCase = new GetCurrentAuctionUseCase(mock.Object);

        // Act
        var auction = useCase.Execute();

        // Assert
        auction.Should().NotBeNull();
        auction.Id.Should().Be(testAuction.Id);
        auction.Name.Should().Be(testAuction.Name);
        auction.Starts.Should().Be(testAuction.Starts);
        auction.Ends.Should().Be(testAuction.Ends);
        auction.Items.Should().NotBeNullOrEmpty();
        auction.Items.First().Id.Should().Be(testAuction.Items.First().Id);
        auction.Items.First().Name.Should().Be(testAuction.Items.First().Name);
        auction.Items.First().Brand.Should().Be(testAuction.Items.First().Brand);
        auction.Items.First().BasePrice.Should().Be(testAuction.Items.First().BasePrice);
        auction.Items.First().Condition.Should().Be(testAuction.Items.First().Condition);
        auction.Items.First().AuctionId.Should().Be(testAuction.Id);
    }
}