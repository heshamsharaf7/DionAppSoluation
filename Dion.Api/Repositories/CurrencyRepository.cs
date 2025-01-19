using Microsoft.EntityFrameworkCore;
using Dion.Api.Data;
using Dion.Api.Entities;
using Dion.Api.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dion.Api.Repositories
{
    public class CurrencyRepository : ICurrencyRepository
    {
        private readonly DionDbContext _dionDbContext;

        public CurrencyRepository(DionDbContext dionDbContext)
        {
            _dionDbContext = dionDbContext;
        }

        public async Task<IEnumerable<Currency>> GetItems()
        {
            return await _dionDbContext.Currency.ToListAsync();
        }

        public async Task<Currency> AddItem(Currency currency)
        {
            _dionDbContext.Currency.Add(currency);
            await _dionDbContext.SaveChangesAsync();
            return currency;
        }

        public async Task<bool> DeleteItem(int id)
        {
            var currency = await _dionDbContext.Currency.FindAsync(id);

            if (currency != null)
            {
                _dionDbContext.Currency.Remove(currency);
                await _dionDbContext.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<Currency> UpdateItem(int id, Currency currency)
        {
            var existingCurrency = await _dionDbContext.Currency.FindAsync(id);

            if (existingCurrency != null)
            {
                existingCurrency.Name = currency.Name;
                existingCurrency.EnteredDate = currency.EnteredDate;
                await _dionDbContext.SaveChangesAsync();
                return existingCurrency;
            }

            return null;
        }

        public async Task<Currency> GetItem(int id)
        {
            return await _dionDbContext.Currency.FindAsync(id);
        }
    }
}