using PokemonReviewApp.Models;
using System.Threading.Tasks;

namespace PokemonReviewApp.Interface
{
    public interface IReviewerRepository
    {
         Task<ICollection<Reviewer>> GetReviewers();    
         Task<Reviewer> GetReviewer(int id);
        Task< ICollection<Review>> GetReviewsByReviewer(int reviewerId);
        Task<bool> ReviewerExists(int id);
        Task<bool> UpdateReviewer(Reviewer reviewer);
        Task<bool> CreateReviewer(Reviewer reviewer);
        Task<bool> DeleteReviewer(Reviewer reviewer);
        Task<bool> Save();
    }
}
    