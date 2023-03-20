using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interface
{
    public interface IContryRepository
    {
         Task<ICollection<Country>> GetCountries();

         Task<Country> GetCountry(int id);  
         Task<Country> GetCountryByOwner(int ownerId);  
         Task<ICollection<Owner>> GetOwnersFromCountry(int countryId);
         Task<bool> CountryExists(int id); 
         Task<bool> UpdateCountry(Country country);
         Task<bool> CreateCountry(Country country);
         Task<bool> DeleteCountry(Country country);
         Task<bool> Save();
    }
}
