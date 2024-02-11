using RocketseatAuction.API.Entities;

namespace RocketseatAuction.API.Contracts;

public interface IOfferRepository
{
    Offer Add(Offer offer);
}