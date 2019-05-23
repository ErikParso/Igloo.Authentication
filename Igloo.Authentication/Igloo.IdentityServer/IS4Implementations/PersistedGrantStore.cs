using IdentityServer4.Models;
using IdentityServer4.Stores;
using Igloo.IdentityServer.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Igloo.IdentityServer.IS4Implementations
{
    public class PersistedGrantStore : IPersistedGrantStore
    {
        private readonly ApplicationDbContext _dbContext;

        public PersistedGrantStore(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<PersistedGrant>> GetAllAsync(string subjectId)
            => await _dbContext.PersistedGrants.Where(grant => grant.SubjectId == subjectId)
                .Select(grant => (PersistedGrant)grant)
                .ToListAsync();

        public async Task<PersistedGrant> GetAsync(string key)
            => await _dbContext.PersistedGrants.SingleOrDefaultAsync(grant => grant.Key == key);

        public async Task RemoveAllAsync(string subjectId, string clientId)
        {
            var grantsToRemove = _dbContext.PersistedGrants
                .Where(grant => grant.SubjectId == subjectId && grant.ClientId == clientId);
            _dbContext.RemoveRange(grantsToRemove);
            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoveAllAsync(string subjectId, string clientId, string type)
        {
            var grantsToRemove = _dbContext.PersistedGrants
                .Where(grant => grant.SubjectId == subjectId && grant.ClientId == clientId && grant.Type == type);
            _dbContext.RemoveRange(grantsToRemove);
            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoveAsync(string key)
        {
            var grantToRemove = _dbContext.PersistedGrants.Where(grant => grant.Key == key);
            _dbContext.RemoveRange(grantToRemove);
            await _dbContext.SaveChangesAsync();
        }

        public async Task StoreAsync(PersistedGrant grant)
        {
            var grantModel = new PersistedGrantModel()
            {
                ClientId = grant.ClientId,
                CreationTime = grant.CreationTime,
                Key = grant.Key,
                Data = grant.Data,
                Expiration = grant.Expiration,
                SubjectId = grant.SubjectId,
                Type = grant.Type
            };
            _dbContext.PersistedGrants.Add(grantModel);
            await _dbContext.SaveChangesAsync();
        }
    }
}
