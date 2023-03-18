using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interface
{
    public interface IContryRepository
    {
         ICollection<Country> GetCountries();

         Country GetCountry(int id);  
         Country GetCountryByOwner(int ownerId);  
         ICollection<Owner> GetOwnersFromCountry(int countryId);
         bool CountryExists(int id); 
         bool CreateCountry(Country country);
         bool Save();
    }
}
