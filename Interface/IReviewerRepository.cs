using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interface
{
    public interface IReviewerRepository
    {
         ICollection<Reviewer> GetReviewers();    
         Reviewer GetReviewer(int id);
         ICollection<Review> GetReviewsByReviewer(int reviewerId);
         bool ReviewerExists(int id); 
    }
}
