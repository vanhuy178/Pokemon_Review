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
        public bool CountryExists(int id)
        {
            return _dataContext.Countries.Any(c => c.Id == id); 
        }

        public bool CreateCountry(Country country)
        {
           _dataContext.Add(country);
            return Save();
        }

        public ICollection<Country> GetCountries()
        {
            return _dataContext.Countries.ToList();
        }

        public Country GetCountry(int id)
        {
            return _dataContext.Countries.Where(e => e.Id == id).FirstOrDefault();  
        }

        public Country GetCountryByOwner( int ownerId)
        {
            return _dataContext.Owners.Where(o => o.Id == ownerId).Select(c => c.Country).FirstOrDefault();
        }

        public ICollection<Owner> GetOwnersFromCountry(int countryId)
        {
            return _dataContext.Owners.Where(c => c.Country.Id == countryId).ToList(); 
        }

        public bool Save()
        {
            var saved = _dataContext.SaveChanges(); 
            return saved > 0? true: false;  
        }
    }
}   
