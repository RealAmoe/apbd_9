using System.Transactions;
using WebApplication1.Models;
using WebApplication1.Models.Dto;

namespace WebApplication1.Repositories;

public class TripService : ITripService
{
    private ITripRepository _tripRepository;

    public TripService(ITripRepository tripRepository)
    {
        _tripRepository = tripRepository;
    }

    public async Task<GetTripDto> GetTripsAsync(int pageNum, int pageSize)
    {
        var data = await _tripRepository.GetTripsAsync(pageNum, pageSize);
        var trips = data.Trips;
        
        if (pageNum > 0 && pageSize > 0)
        {
            var rowSkipCount = (pageNum - 1) * pageSize;
            trips = trips.Skip(rowSkipCount).Take(pageSize).ToList();
        }

        data.Trips = trips;
        return data;
    }

    public async Task<int?> DeleteClientAsync(int idClient)
    {
        var client = await _tripRepository.ClientExistAsync(idClient);
        if (!client)
        {
            return -1;
        }

        var clientTrip = await _tripRepository.ClientHasTripsAsync(idClient);
        if (clientTrip)
        {
            return -2;
        }

        return await _tripRepository.DeleteClientAsync(idClient);
    }

    public async Task<int?> AssignClientToTripAsync(int idTrip, AssignTripDto assignTripDto)
    {
        var tripExist = await _tripRepository.TripExistAsync(assignTripDto.IdTrip);
        var peselExist = await _tripRepository.ClientPeselExistAsync(assignTripDto.Pesel);
        var clientPeselHasTrips = await _tripRepository.ClientPeselHasTripsAsync(assignTripDto.IdTrip, assignTripDto.Pesel);
        if (!tripExist)
        {
            return -2;
        }
        
        if (!peselExist)
        {
            await _tripRepository.AddClientAsync(assignTripDto);
            return await _tripRepository.AssignClientToTripAsync(assignTripDto);
        }
        
        if (clientPeselHasTrips)
        {
            return await _tripRepository.AssignClientToTripAsync(assignTripDto);
        }

        return -1;
    }
}