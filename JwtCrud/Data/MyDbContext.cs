using Microsoft.EntityFrameworkCore;
using MySqlApi.Model;

namespace MySqlApi.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
        }

        public DbSet<Country> country { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Country>().HasKey(c => c.country_id);  // Country tablosuna primary key tanımlandı
        }

        // Tüm ülkeleri getir
        public List<Country> GetAllCountries()
        {
            return country.ToList();
        }

        // Yeni ülke ekle
        public void AddCountry(Country country)
        {
            this.country.Add(country);
            this.SaveChanges();
        }

        // Ülkeyi güncelle
        public bool UpdateCountry(int id, Country country)
        {
            var existingCountry = this.country.Find(id);
            if (existingCountry == null)
            {
                return false;
            }

            existingCountry.country = country.country;
            this.SaveChanges();
            return true;
        }

        public bool DeleteCountry(int id)
        {
            var existingCountry = this.country.Find(id);
            if (existingCountry == null)
            {
                return false;
            }

            this.country.Remove(existingCountry);
            this.SaveChanges();
            return true;
        }
    }
}