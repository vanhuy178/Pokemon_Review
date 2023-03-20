﻿using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interface;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DataContext _context;

        public CategoryRepository(DataContext context) {
            _context = context;
        }
        public async Task<bool> CategoryExist(int id)
        {
            return await _context.Categories.AnyAsync(item => item.Id == id);
        }

        public async Task<bool> CreateCategory(Category category)
        {
            await _context.AddAsync(category);
            return await Save();
        }

        public async Task<bool> DeleteCategory(Category category)
        {
            _context.Remove(category);  
            return await Save();  
        }

        public async Task<ICollection<Category>> GetCategories()
        {
            return await _context.Categories.OrderBy(c => c.Id).ToListAsync();    
        }

        public async Task<Category> GetCategory(int id)
        {
            return await _context.Categories.Where(e => e.Id == id).FirstOrDefaultAsync();
        }

        public async Task<ICollection<Pokemon>> GetPokemonByCategories(int categoryId)
        {
           return await _context.PokemonCategories.Where(e => e.CategoryId  == categoryId).Select(e =>e.Pokemon).ToListAsync();
        }

        public async Task<bool> Save()
        {
            var saved = await _context.SaveChangesAsync();

            return saved > 0 ? true : false;
           

        }

        public async Task<bool> UpdateCategory(Category category)
        {
            _context.Update(category);  
            return await Save();  
        }


    }
}
    