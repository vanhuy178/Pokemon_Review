using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interface
{
    public interface IReviweRepository
    {
         ICollection<Review> GetReviews();

         Review GetReview(int reviewId);

         ICollection<Review> GetReviewOfPokemon(int pokeId);

         bool ReviewExist(int reviewId);
    }
}
