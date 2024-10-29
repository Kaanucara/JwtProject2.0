using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using MySqlApi.Data;
using MySqlApi.Model; // Country modelini kullanmak için
using System.Collections.Generic;
using System.Diagnostics.Metrics;

namespace JwtCrud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CrudController : ControllerBase
    {
        private readonly MyDbContext _context;  // Veri tabanı bağlantısı

        public CrudController(MyDbContext context)
        {
            _context = context;
        }

        [HttpPost("create")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Create([FromBody] Country countryData)
        {
            countryData.last_update = DateTime.Now;   // Son güncelleme zamanını ayarla
            _context.country.Add(countryData);     // Yeni ülke verisini ekler
            await _context.SaveChangesAsync();       // Veritabanına kaydeder
            return Ok("Country created successfully!");
        }

        [HttpGet("read")]
        [Authorize()]
        public async Task<IActionResult> Read()
        {
            var countries = await _context.country.ToListAsync();  // Tablonun adını düzeltiyoruz
            return Ok(countries);
        }


        [HttpPut("update")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update( [FromBody] Country updatedCountry)
        {
            var existingCountry = await _context.country.FindAsync(updatedCountry.country_id);

            if (existingCountry == null)
            {
                return NotFound("Country not found.");
            }

            existingCountry.country = updatedCountry.country;  // Ülke adını güncelle
            existingCountry.last_update = DateTime.Now;        // Son güncelleme tarihini güncelle
            await _context.SaveChangesAsync();                 // Veritabanına uygular
            return Ok("Country updated successfully!");
        }

        [HttpDelete("delete")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int country_id)
        {
            var existingCountry = await _context.country.FindAsync(country_id);

            if (existingCountry == null)
            {
                return NotFound("Country not found.");
            }

            _context.country.Remove(existingCountry);  // Ülkeyi siler
            await _context.SaveChangesAsync();         // Veritabanına uygular
            return Ok("Country deleted successfully!");
        }

        [HttpGet("changes")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetChanges()
        {
            var countries = await _context.country.ToListAsync();  // Tüm ülke verilerini getirir
            return Ok(countries);
        }
    }
}