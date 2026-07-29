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
        if (string.IsNullOrWhiteSpace(dto.CardId))
            throw new ArgumentException("La carte est obligatoire.");

        if (dto.CardId.Trim().Length > 100)
            throw new ArgumentException("L'identifiant de la carte est trop long.");

        if (dto.Price <= 0)
            throw new ArgumentException("Le prix doit être supérieur à zéro.");

        if (dto.Description?.Length > 500)
            throw new ArgumentException("La description ne peut pas dépasser 500 caractères.");

        dto.CardId = dto.CardId.Trim();

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

        if (dto.Price.HasValue)
        {
            if (dto.Price.Value <= 0)
                throw new ArgumentException("Le prix doit être supérieur à zéro.");

            existing.Price = dto.Price.Value;
        }

        if (dto.Description?.Length > 500)
            throw new ArgumentException("La description ne peut pas dépasser 500 caractères.");

        // Le formulaire envoie null pour effacer une description : il faut le conserver.
        existing.Description = dto.Description;

        if (dto.Status.HasValue)
        {
            if (!Enum.IsDefined(dto.Status.Value))
                throw new ArgumentException("Le statut de l'annonce est invalide.");

            if (dto.Status.Value == ListingStatus.Sold)
                throw new ArgumentException("Une annonce ne peut être vendue que via l'achat.");

            existing.Status = dto.Status.Value;
        }

        return await _repository.UpdateAsync(existing);
    }
}
