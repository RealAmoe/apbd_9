using WebApplication1.Models;
using WebApplication1.Models.Dto;

namespace WebApplication1.Repositories;

public interface ITripService
{
    public Task<GetTripDto> GetTripsAsync(int pageNum, int pageSize);
    public Task<int?> DeleteClientAsync(int idClient);
    public Task<int?> AssignClientToTripAsync(int idTrip, AssignTripDto assignTripDto);
}