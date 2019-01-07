using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Entities;
using System.Net.Http;
using Newtonsoft.Json;
using API.DTO;

namespace Web.Controllers
{
    public class FlightsController : Controller
    {
        // HTTP client used to get data from API
        private readonly HttpClient _httpClient;
        // The base URI for the web api
        private Uri BaseEndPoint { get; set; }

        public FlightsController()
        {
            // Set the port to whatever the API port is once you've started it once. Shouldn't change on restarts.
            BaseEndPoint = new Uri("https://localhost:44386/api/Flights");
            _httpClient = new HttpClient();
        }

        // GET: Flights
        public async Task<IActionResult> Index()
        {
            // use HTTP client to read data from API. Move on once the headers have been read. Errors are caught slightly quicker this way.
            var response = await _httpClient.GetAsync(BaseEndPoint, HttpCompletionOption.ResponseHeadersRead);
            // Make sure that we got a success status code in the headers. Returns an exception (and 500 status code) if not successful
            response.EnsureSuccessStatusCode();
            // Turn the response body into a string
            var data = await response.Content.ReadAsStringAsync();
            // Treat the response body string as JSON, and deserialize it into a list of gifts
            return View(JsonConvert.DeserializeObject<List<Flight>>(data));
        }

        // GET: Flights
        [HttpPost]
        public async Task<IActionResult> Index([FromForm] string fromLocation, [FromForm] string toLocation)
        {
            // use HTTP client to read data from API. Move on once the headers have been read. Errors are caught slightly quicker this way.
            var response = await _httpClient.GetAsync(BaseEndPoint + $"/{fromLocation}/{toLocation}", HttpCompletionOption.ResponseHeadersRead);
            // Make sure that we got a success status code in the headers. Returns an exception (and 500 status code) if not successful
            response.EnsureSuccessStatusCode();
            // Turn the response body into a string
            var data = await response.Content.ReadAsStringAsync();
            // Treat the response body string as JSON, and deserialize it into a list of gifts
            return View(JsonConvert.DeserializeObject<List<Flight>>(data));
        }

        // GET: Flights/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // use HTTP client to read data from API. Move on once the headers have been read. Errors are caught slightly quicker this way.
            var response = await _httpClient.GetAsync(BaseEndPoint + $"/{id}", HttpCompletionOption.ResponseHeadersRead);
            // Turn the response body into a string
            var data = await response.Content.ReadAsStringAsync();
            var flight = JsonConvert.DeserializeObject<Flight>(data);
            if (flight == null)
            {
                return NotFound();
            }
            return View(flight);
        }

        // POST: Flights/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FlightId,AircraftType,FromLocation,ToLocation,DepartureTime,ArrivalTime,Version")] Flight flight)
        {
            if (id != flight.FlightId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Post the created gift as JSON to API. HttpClient handles serialization for us
                    var response = await _httpClient.PutAsJsonAsync<Flight>(BaseEndPoint + $"/{id}", flight);
                    response.EnsureSuccessStatusCode();
                }
                catch (HttpRequestException)
                {
                    if (!await FlightExists(flight.FlightId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(flight);
        }

        private async Task<bool> FlightExists(int id)
        {
            var response = await _httpClient.GetAsync(BaseEndPoint, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();
            var context = JsonConvert.DeserializeObject<List<Flight>>(data);
            return context.Any(e => e.FlightId == id);
        }
    }
}
