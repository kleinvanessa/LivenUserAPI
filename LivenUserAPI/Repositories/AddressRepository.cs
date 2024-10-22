﻿using LivenUserAPI.Domain.Entities;
using LivenUserAPI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LivenUserAPI.Repositories
{
    public class AddressRepository : IAddressRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public AddressRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Address> GetAddressById(int id)
        {
            return await _dbContext.Addresses.FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Address>> GetAllAddressesByUserId(int userId)
        {
            return await _dbContext.Addresses.Where(a => a.UserId == userId).ToListAsync();
        }

        public async Task AddNewAddress(Address address)
        {
            await _dbContext.Addresses.AddAsync(address);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAddress(Address address)
        {
            _dbContext.Addresses.Update(address);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAddress(Address address)
        {
            _dbContext.Addresses.Remove(address);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> VerifyAddressByUserId(int addressId, int userId)
        {
            return await _dbContext.Addresses.AnyAsync(a => a.Id == addressId && a.UserId == userId);            
        }


    }
}
