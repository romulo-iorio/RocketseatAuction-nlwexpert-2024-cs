using Bogus;
using FluentAssertions;
using Moq;
using RocketseatAuction.API.Communication.Requests;
using RocketseatAuction.API.Contracts;
using RocketseatAuction.API.Entities;
using RocketseatAuction.API.Services;
using RocketseatAuction.API.UseCases.Auctions.GetCurrent;
using Xunit;

namespace UseCases.Test.Auctions.GetCurrent;

public class CreateOfferUseCaseTest
{
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void Success(int itemId)
    {
        // Arrange
        var request = new Faker<RequestCreateOfferJson>()
            .RuleFor(request => request.Price, f => f.Random.Decimal(50, 1000))
            .Generate();

        var offerRepositoryMock = new Mock<IOfferRepository>();
        offerRepositoryMock.Setup(i => i.Add(It.IsAny<Offer>())).Returns(new Offer());
        var loggedUserMock = new Mock<ILoggedUser>();
        loggedUserMock.Setup(i => i.User()).Returns(new User());

        var useCase = new CreateOfferUseCase(loggedUserMock.Object, offerRepositoryMock.Object);

        // Act
        var act = () => useCase.Execute(itemId, request);

        // Assert
        act.Should().NotThrow();
    }
}