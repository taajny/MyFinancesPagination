using MyFinances.Models.Domains;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyFinances.Models
{
    public class OperationRepository
    {
        private readonly SQLiteAsyncConnection _context;

        public OperationRepository(SQLiteAsyncConnection context)
        {
            _context = context;
        }
        public async Task<int> AddAsync(Operations operation)
        {
            return await _context.InsertAsync(operation);
        }

        public async Task DeleteAsync(Operations operation)
        {
            await _context.DeleteAsync(operation);
        }

        public async Task<Operations> GetAsync(int id)
        {
            return await _context.Table<Operations>().FirstAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Operations>> GetAsync()
        {
            return await _context.Table<Operations>().ToListAsync();
        }

        public async Task<IEnumerable<Operations>> GetAsync(int numberOfRecords, int numberOfPage)
        {
            return await _context.Table<Operations>()
                .Skip(numberOfRecords * (numberOfPage - 1))
                .Take(numberOfRecords)
                .ToListAsync();
        }

        public async Task UpdateAsync(Operations operation)
        {
            await _context.UpdateAsync(operation);
        }
    }
}
