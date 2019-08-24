using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Dagger.Data.Repositories
{
    public interface IRepository<TDTO> where TDTO:class
    {
        void SetTransaction(IDbTransaction transaction);
        Task<int> Create(TDTO obj);
        Task CreateAll(IEnumerable<TDTO> list);
        Task<TDTO> Read(int id);
        Task<IEnumerable<TDTO>> ReadAll();
        Task<IEnumerable<TDTO>> GetUsingSQL(string sql, object parameters = null);
        Task<bool> Update(TDTO obj);
        Task<bool> UpdateAll(IEnumerable<TDTO> list);
        Task<bool> Delete(TDTO obj);
        Task<int> ExecuteSQL(string sql, object parameters = null);    
        Task<int> ExecuteScalarQuery(string sql, object parameters = null);    
    }
}