using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Entities;
using AutoMapper;
using API.DTO;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public FlightsController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Flights
        [HttpGet]
        public IEnumerable<FlightDTO> GetFlights()
        {
            return _mapper.Map<List<FlightDTO>>(_context.Flights);
        }

        // GET: api/Flights/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFlight([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var flight = await _context.Flights.FindAsync(id);

            if (flight == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<FlightDetailedDTO>(flight));
        }

        [HttpGet("{fromLocation}/{toLocation}")]
        public IActionResult GetFlights([FromRoute] string fromLocation, [FromRoute] string toLocation)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var flights = _context.Flights.Where(flight => flight.FromLocation == fromLocation && flight.ToLocation == toLocation);

            if(flights.Count() == 0)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<List<FlightDTO>>(flights));
        }

        // PUT: api/Flights/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFlight([FromRoute] int id, [FromBody] FlightDetailedDTO flight)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != flight.FlightId)
            {
                return BadRequest();
            }

            _context.Entry(_mapper.Map<Flight>(flight)).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FlightExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Flights
        [HttpPost]
        public async Task<IActionResult> PostFlight([FromBody] FlightDetailedDTO flight)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var actualFlight = _mapper.Map<Flight>(flight);

            _context.Flights.Add(actualFlight);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFlight", new { id = actualFlight.FlightId }, flight);
        }

        private bool FlightExists(int id)
        {
            return _context.Flights.Any(e => e.FlightId == id);
        }
    }
}