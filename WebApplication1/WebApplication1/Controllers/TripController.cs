using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models.Dto;
using WebApplication1.Repositories;

namespace WebApplication1.Controllers;
[ApiController]
[Route("api/trips")]
public class TripController : ControllerBase
{
    private ITripService _tripService;

    public TripController(ITripService tripService)
    {
        _tripService = tripService;
    }

    [HttpGet]
    public async Task<IActionResult> GetTrips(int pageNum, int pageSize = 10)
    {
        var trips = await _tripService.GetTripsAsync(pageNum, pageSize);
        return Ok(trips);
    }

    [HttpPost("{idTrip}/clients")]
    public async Task<IActionResult> AssignClientToTrip(int idTrip, AssignTripDto assignTripDto)
    {
        var response = await _tripService.AssignClientToTripAsync(idTrip, assignTripDto);
        switch (response)
        {
            case -1: return NotFound("Client already has the given trip assigned");
            case -2: return NotFound("Given trip not found");
        }
        return Created();
    }
}