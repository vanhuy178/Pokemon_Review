using AutoMapper;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interface;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public OwnerRepository(DataContext dataContext, IMapper mapper) {
            _dataContext = dataContext;
            _mapper = mapper;
        
        }    
        public Owner GetOwner(int ownerId) // get one may be using where, firstordefault
        {
            return _dataContext.Owners.Where(o => o.Id == ownerId).FirstOrDefault();
        }

        public ICollection<Owner> GetOwners()
        {
            return _dataContext.Owners.ToList();
        }

        public ICollection<Owner> GetOwnerOfPokemon(int ownerId)
        {
           return _dataContext.PokemonOwners.Where(o => o.Pokemon.Id == ownerId).Select(o => o.Owner).ToList();
        }

        public bool OwnerExist(int ownerId)
        {
            return _dataContext.Owners.Any(o => o.Id == ownerId);
        }

        public ICollection<Pokemon> GetPokemonByOwner(int ownerId)
        {
            return _dataContext.PokemonOwners.Where(p => p.Owner.Id == ownerId).Select(p => p.Pokemon).ToList();
        }

        public bool CreateOwner(Owner owner)
        {
            _dataContext.Add(owner);
            return Save();
        }

        public bool Save()
        {
            var saved = _dataContext.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
