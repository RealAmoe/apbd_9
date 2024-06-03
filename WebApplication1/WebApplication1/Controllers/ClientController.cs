using Microsoft.AspNetCore.Mvc;
using WebApplication1.Repositories;

namespace WebApplication1.Controllers;
[ApiController]
[Route("api/clients")]
public class ClientController : ControllerBase
{
    private ITripService _tripService;

    public ClientController(ITripService tripService)
    {
        _tripService = tripService;
    }
    [HttpDelete("{idClient}")]
    public async Task<IActionResult> DeleteClient(int idClient)
    {
        var response = await _tripService.DeleteClientAsync(idClient);

        switch (response)
        {
            case -1: return NotFound("Client was not found");
            case -2 : return Conflict("Client has assigned trips");
        }
        return NoContent();
    }
}