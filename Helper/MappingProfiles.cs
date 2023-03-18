using AutoMapper;
using PokemonReviewApp.DTO;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Helper
{
    public class MappingProfiles : Profile
    {
       public MappingProfiles() {
            CreateMap<Pokemon, PokemonDTO>();
            CreateMap<Category, CategoryDTO>();
            CreateMap<CategoryDTO, Category>();
            CreateMap<CountryDTO, Country>();
            CreateMap<Country, CountryDTO>();
            CreateMap<Review, ReviewDTO>();
            CreateMap<Reviewer, ReviwersDTO>(); 
            CreateMap<Owner, OwnerDTO>();
            CreateMap<OwnerDTO, Owner>();
        } 
    }
}
