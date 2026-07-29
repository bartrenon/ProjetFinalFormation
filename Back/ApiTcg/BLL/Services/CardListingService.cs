using BLL.Dtos.CardListing;
using BLL.Interfaces;
using BLL.Mappers;
using DAL.Interfaces;
using Domain.Entities;
using Domain.Enum;

namespace BLL.Services;

public class CardListingService : ICardListingService
{
    private readonly ICardListingRepository _repository;

    public CardListingService(ICardListingRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> BuyAsync(int listingId, int buyerId)
    {
        CardListing? listing = await _repository.GetByIdAsync(listingId);

        if (listing is null || listing.Status != ListingStatus.Active)
            return false;

        if (listing.SellerId == buyerId)
            throw new InvalidOperationException("Un vendeur ne peut pas acheter sa propre carte.");

        return await _repository.MarkAsSoldAsync(listingId, buyerId);
    }

    public async Task<CardListingResponseDto> CreateAsync(CreateCardListingDto dto)
    {
        if (dto.Price < 0)
            throw new ArgumentException("Le prix ne peut pas être négatif.");

        CardListing listing = CardListingMapper.ToCardListing(dto) ;

        int newId = await _repository.CreateAsync(listing);
        CardListing? created = await _repository.GetByIdAsync(newId)
            ?? throw new InvalidOperationException("Erreur lors de la création de l'annonce.");

        return CardListingMapper.ToCardListingResponseDto(created);
    }

    public async Task<bool> DeleteAsync(int listingId)
    {
        return await _repository.DeleteAsync(listingId);
    }

    public async Task<IEnumerable<CardListingResponseDto>> GetActiveAsync()
    {
        IEnumerable<CardListing> listings = await _repository.GetActiveAsync();

        return listings.Select(CardListingMapper.ToCardListingResponseDto);
    }

    public async Task<CardListingResponseDto?> GetByIdAsync(int listingId)
    {
        CardListing? listing = await _repository.GetByIdAsync(listingId);

        return listing is null ? null : CardListingMapper.ToCardListingResponseDto(listing);
    }

    public async Task<IEnumerable<CardListingResponseDto>> GetBySellerAsync(int sellerId)
    {
        IEnumerable<CardListing> listings = await _repository.GetBySellerAsync(sellerId);

        return listings.Select(CardListingMapper.ToCardListingResponseDto);
    }

    public async Task<bool> UpdateAsync(int listingId, UpdateCardListingDto dto)
    {
        CardListing? existing = await _repository.GetByIdAsync(listingId);

        if (existing is null)
            return false;

        existing.Price = dto.Price ?? existing.Price;
        existing.Description = dto.Description ?? existing.Description;
        existing.Status = dto.Status ?? existing.Status;

        return await _repository.UpdateAsync(existing);
    }
}
