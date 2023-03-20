using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interface;
using PokemonReviewApp.Models;
using System.Linq.Expressions;

namespace PokemonReviewApp.Repository
{

    public class CountryReposiroty : IContryRepository
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public CountryReposiroty( DataContext dataContext, IMapper mapper ) {
            _dataContext = dataContext;
            _mapper = mapper;
        }  
        public async Task<bool> CountryExists(int id)
        {
            return await _dataContext.Countries.AnyAsync(c => c.Id == id); 
        }

        public async Task<bool> CreateCountry(Country country)
        {
           await _dataContext.AddAsync(country);
            return await Save();
        }

        public async Task<bool> DeleteCountry(Country country)
        {
            _dataContext?.Remove(country);
            return  await Save();  
        }

        public async Task<ICollection<Country>> GetCountries()
        {
            return await _dataContext.Countries.ToListAsync();
        }

        public async Task<Country> GetCountry(int id)
        {
            return await _dataContext.Countries.Where(e => e.Id == id).FirstOrDefaultAsync();  
        }

        public async Task<Country> GetCountryByOwner( int ownerId)
        {
            return await _dataContext.Owners.Where(o => o.Id == ownerId).Select(c => c.Country).FirstOrDefaultAsync();
        }

        public async Task<ICollection<Owner>> GetOwnersFromCountry(int countryId)
        {
            return await _dataContext.Owners.Where(c => c.Country.Id == countryId).ToListAsync(); 
        }

        public async Task<bool> Save()
        {
            var saved = await _dataContext.SaveChangesAsync(); 
            return saved > 0? true: false;  
        }

        public async Task<bool> UpdateCountry(Country country)
        {
           _dataContext.Update(country);    

            return await Save();  
        }
    }
}   
