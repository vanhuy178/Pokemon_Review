using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interface;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class ReviewerRepository : IReviewerRepository
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public ReviewerRepository(DataContext dataContext, IMapper mapper) {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public Task<bool> CreateReviewer(Reviewer reviewer)
        {
            _dataContext.Add(reviewer);
            return Save();
        }

        public async Task<Reviewer> GetReviewer(int reviewerId)
        {
            return await _dataContext.Reviewers.Where(r => r.Id == reviewerId).FirstOrDefaultAsync();
        }

        public async Task<ICollection<Reviewer>> GetReviewers()
        {
            return await _dataContext.Reviewers.ToListAsync();
        } 

        public async Task<ICollection<Review>> GetReviewsByReviewer(int reviewerId)
        {
            return _dataContext.Reviews.Where(re => re.Reviewer.Id == reviewerId).ToList();    
        }

        public async Task<bool> ReviewerExists(int id)
        {
            return await _dataContext.Reviewers.AnyAsync(r => r.Id == id); 
        }

        public async Task<bool> UpdateReviewer(Reviewer reviewer)
        {
            _dataContext.Update(reviewer);
            return await Save();
        }

        public async Task<bool> DeleteReviewer(Reviewer reviewer) {
            _dataContext.Remove(reviewer);
            return await Save();  
        }
        public async Task<bool> Save()
        {
            var saved = await _dataContext.SaveChangesAsync();
            return saved > 0 ? true : false;
        }
    }
}
