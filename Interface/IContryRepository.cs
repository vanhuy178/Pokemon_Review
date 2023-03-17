using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interface
{
    public interface IContryRepository
    {
        public ICollection<Country> GetCountries();

        public Country GetCountry(int id);  
        public Country GetCountryByOwner(int ownerId);  
        public ICollection<Owner> GetOwnersFromCountry(int countryId);
        public bool CountryExists(int id);  
    }
}
