using System.Globalization;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Context;
using WebApplication1.Models;
using WebApplication1.Models.Dto;

namespace WebApplication1.Repositories;

public class TripRepository : ITripRepository
{
    private S27508Context _dbContext;

    public TripRepository(S27508Context dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<GetTripDto> GetTripsAsync(int pageNum, int pageSize)
    {
        var trips = await _dbContext.Trips.OrderByDescending(t => t.DateFrom)
            .Select(t => new TripDto()
            {
                Name = t.Name,
                Description = t.Description,
                DateFrom = t.DateFrom,
                DateTo = t.DateTo,
                MaxPeople = t.MaxPeople,
                Countries = t.IdCountries.Select(c => new CountryDto()
                {
                    Name = c.Name
                }).ToList(),
                Clients = t.ClientTrips.Where(ct => ct.IdTrip == t.IdTrip).Select(ct => new ClientDto()
                {
                    FirstName = ct.IdClientNavigation.FirstName,
                    LastName = ct.IdClientNavigation.LastName
                }).ToList()
            }).ToListAsync();

        return new GetTripDto()
        {
            PageNum = pageNum,
            PageSize = pageSize,
            AllPages = (int)Math.Ceiling((double)trips.Count / pageSize),
            Trips = trips
        };
    }

    public async Task<bool> ClientExistAsync(int idClient)
    {
        var client = await _dbContext.Clients.FindAsync(idClient);
        if (client == null)
        {
            return false;
        }
        return true;
    }

    public async Task<bool> ClientHasTripsAsync(int idClient)
    {
        var clientHasTrips = await _dbContext.ClientTrips.Where(c => c.IdClient == idClient).ToListAsync();

        if (clientHasTrips.Count > 0)
            return true;
        return false;
    }
    public async Task<int?> DeleteClientAsync(int idClient)
    {
        var clientToDelete = await _dbContext.Clients.FindAsync(idClient);
        _dbContext.Clients.Remove(clientToDelete);
        var check = await _dbContext.SaveChangesAsync();
        
        return check;
    }

    public async Task<bool> TripExistAsync(int idTrip)
    {
        var tripExist = await _dbContext.Trips.FindAsync(idTrip);

        if (tripExist == null || tripExist.DateFrom < DateTime.Now)
            return false;
        return true;
    }

    public async Task<bool> ClientPeselExistAsync(string clientPesel)
    {
        var clientExist = await _dbContext.Clients
            .FirstOrDefaultAsync(c => c.Pesel == clientPesel);
        if (clientExist == null)
            return false;
        return true;
    }

    public async Task<bool> ClientPeselHasTripsAsync(int idTrip, string clientPesel)
    {
        var clientHasTrip = await _dbContext.ClientTrips
                    .FirstOrDefaultAsync(ct => ct.IdClientNavigation.Pesel == clientPesel && ct.IdTrip == idTrip);
    
        if (clientHasTrip != null)
            return false;
        return true;
    }

    public async Task<int> AddClientAsync(AssignTripDto assignTripDto)
    {
        var client = new Client()
        {
            FirstName = assignTripDto.FirstName,
            LastName = assignTripDto.LastName,
            Email = assignTripDto.Email,
            Telephone = assignTripDto.Telephone,
            Pesel = assignTripDto.Pesel
        };
        _dbContext.Clients.Add(client);
        var check = await _dbContext.SaveChangesAsync();
        return check;
    }

    public async Task<int?> AssignClientToTripAsync(AssignTripDto assignTripDto)
    {
        var client = await _dbContext.Clients
            .FirstOrDefaultAsync(c => c.Pesel == assignTripDto.Pesel);
        var clientTrip = new ClientTrip()
        {
            IdClient = client.IdClient,
            IdTrip = assignTripDto.IdTrip,
            RegisteredAt = DateTime.Now,
            PaymentDate = assignTripDto.PaymentDate
        };
        _dbContext.ClientTrips.Add(clientTrip);
        await _dbContext.SaveChangesAsync();
        return 0;
    }
}