using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interface;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public OwnerRepository(DataContext dataContext ,IMapper mapper) {
            _dataContext = dataContext;
            _mapper = mapper;
        
        }    
        public async Task<Owner> GetOwner(int ownerId) // get one may be using where, firstordefault
        {
            return await _dataContext.Owners.Where(o => o.Id == ownerId).FirstOrDefaultAsync();
        }

        public async Task<ICollection<Owner>> GetOwners()
        {
            return await _dataContext.Owners.ToListAsync();
        }

        public async Task<ICollection<Owner>> GetOwnerOfPokemon(int ownerId)
        {
           return await _dataContext.PokemonOwners.Where(o => o.Pokemon.Id == ownerId).Select(o => o.Owner).ToListAsync();
        }

        public async Task<bool> OwnerExist(int ownerId)
        {
            return await _dataContext.Owners.AnyAsync(o => o.Id == ownerId);
        }

        public async Task<ICollection<Pokemon>> GetPokemonByOwner(int ownerId)
        {
            return await _dataContext.PokemonOwners.Where(p => p.Owner.Id == ownerId).Select(p => p.Pokemon).ToListAsync();
        }

        public async Task<bool> CreateOwner(Owner owner)
        {
            await _dataContext.AddAsync(owner);
            return await Save();
        }

        public async Task<bool> Save()
        {
            var saved = await _dataContext.SaveChangesAsync();
            return saved > 0 ? true : false;
        }

        public async Task<bool> UpdateOwner(Owner owner)
        {
            _dataContext.Update(owner);
            return await Save();
        }

        public async Task<bool> DeleteOwner(Owner owner) {
            _dataContext.Remove(owner); 
            return await Save();
         }
    }
}
