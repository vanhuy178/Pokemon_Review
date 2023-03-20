using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interface
{
    public interface IReviweRepository
    {
         Task<ICollection<Review>> GetReviews();

         Task<Review> GetReview(int reviewId);

         Task<ICollection<Review>> GetReviewOfPokemon(int pokeId);

         Task<bool> ReviewExist(int reviewId);
         Task<bool> UpdateReview(Review review);
        Task<bool> CreateReview(Review review);
        Task<bool> DeleteReview(Review review);
        Task<bool> Save();
    }
}
