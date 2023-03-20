using AutoMapper;
using Microsoft.AspNetCore.Identity;
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

        public async Task<bool> CreateReview(Review review)
        {
            await _dataContext.AddAsync(review);
            return await Save();
        }

        public async Task<bool> DeleteReview(Review review)
        {
            _dataContext?.Remove(review);
            return await Save();
        }

        public async Task<Review> GetReview(int reviewId)
        {
            return await _dataContext.Reviews.Where(r => r.Id == reviewId).FirstOrDefaultAsync();
        }

        public async Task<ICollection<Review>> GetReviewOfPokemon(int pokeId)
        {
             return await _dataContext.Reviews.Where(r => r.Id == pokeId).ToListAsync();       
        }

        public async Task<ICollection<Review>> GetReviews()
        {
            return await _dataContext.Reviews.ToListAsync();
        }

        public async Task<bool> ReviewExist(int reviewId)
        {
            return await _dataContext.Reviews.AnyAsync(r => r.Id == reviewId); 
        }

        public async Task<bool> Save()
        {
            var saved = await _dataContext.SaveChangesAsync();
            return  saved > 0 ? true : false;
        }
        


        public async Task<bool> UpdateReview(Review review)
        {
            _dataContext.Update(review);
            return await Save();
        }
    }
}
