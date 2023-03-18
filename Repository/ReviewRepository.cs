using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interface;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class ReviewRepository : IReviweRepository
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public ReviewRepository(DataContext dataContext, IMapper mapper) { 
            _dataContext = dataContext;
            _mapper = mapper;
        }   
        public Review GetReview(int reviewId)
        {
            return _dataContext.Reviews.Where(r => r.Id == reviewId).FirstOrDefault();
        }

        public ICollection<Review> GetReviewOfPokemon(int pokeId)
        {
             return _dataContext.Reviews.Where(r => r.Id == pokeId).ToList();       
        }

        public ICollection<Review> GetReviews()
        {
            return _dataContext.Reviews.ToList();
        }

        public bool ReviewExist(int reviewId)
        {
            return _dataContext.Reviews.Any(r => r.Id == reviewId); 
        }
    }
}
