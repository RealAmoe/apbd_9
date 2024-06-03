using WebApplication1.Models;
using WebApplication1.Models.Dto;

namespace WebApplication1.Repositories;

public interface ITripRepository
{
    public Task<GetTripDto> GetTripsAsync(int pageNum, int pageSize);
    public Task<bool> ClientExistAsync(int idClient);
    public Task<bool> ClientHasTripsAsync(int idClient);
    public Task<int?> DeleteClientAsync(int idClient);
    public Task<bool> TripExistAsync(int idTrip);
    public Task<bool> ClientPeselExistAsync(string clientPesel);
    public Task<bool> ClientPeselHasTripsAsync(int idTrip, string clientPesel);
    public Task<int> AddClientAsync(AssignTripDto assignTripDto);
    public Task<int?> AssignClientToTripAsync(AssignTripDto assignTripDto);
}