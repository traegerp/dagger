using System;
using System.Linq;
using System.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;
using Npgsql;
using Dagger.Data.Connections;

namespace Dagger.Data.Repositories
{

    /// <summary>
    /// Generic Postgresql Repository (stand alone and works with unit of work pattern)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Repository<TDto> : IRepository<TDto> where TDto : class
    {

        private readonly string _connectionString;
        private IDbTransaction _transaction;
        private int _commandTimeout = 60;

        internal IDbConnection Connection
        {
            get
            {
                return new NpgsqlConnection(this._connectionString);
            }
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="connectionString"></param>
        public Repository(IConnection connection)
        {
            _connectionString = connection.GetConnectionString();
        }

        /// <summary>
        /// UOW Ctor
        /// </summary>
        public Repository(IDbTransaction transaction)
        {
            _transaction = transaction;
        }

        /// <summary>
        /// Set Transaction to Repository
        /// </summary>
        /// <param name="transaction"></param>
        public void SetTransaction(IDbTransaction transaction)
        {
            if(transaction != null) throw new ArgumentNullException(nameof(transaction));

            _transaction = transaction;

        }

        /// <summary>
        /// Insert one
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<int> Create(TDto dto)
        {

            if (_transaction != null)
            {
                return await _transaction.Connection.InsertAsync(dto);
            }
            else
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    return await dbConnection.InsertAsync(dto);
                }
            }
        }

        /// <summary>
        /// Insert Many
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task CreateAll(IEnumerable<TDto> list)
        {

            if (_transaction != null)
            {
                await _transaction.Connection.InsertAsync(list);
            }
            else
            {

                using (IDbConnection dbConnection = Connection)
                {

                    dbConnection.Open();

                    using (IDbTransaction transaction = dbConnection.BeginTransaction())
                    {
                        try
                        {
                            await dbConnection.InsertAsync(list);
                            transaction.Commit();
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Get One
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TDto> Read(int id)
        {            
            if(_transaction != null)
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    var dto = await dbConnection.GetAsync<TDto>(id);                

                    return dto;
                }
            }
            else
            {
                var dto = await _transaction.Connection.GetAsync<TDto>(id);
                return dto;
            }
        }

        /// <summary>
        /// Get All
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<TDto>> ReadAll()
        {
            if(_transaction != null)
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    var dtos = await dbConnection.GetAllAsync<TDto>();
                    return dtos;
                }
            }
            else
            {
                var dtos = await _transaction.Connection.GetAllAsync<TDto>(); 
                return dtos;
            }
        }

        /// <summary>
        /// Queries with SQL
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<IEnumerable<TDto>> GetUsingSQL(string sql, object parameters = null)
        {

            if(parameters == null)
            {
                parameters = new object();
            }

            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return await dbConnection.QueryAsync<TDto>(sql, parameters);
            }
        }

        /// <summary>
        /// Queries with SQL with Generic Return Typing Get All
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<IEnumerable<T1>> GetAllUsingSQLGeneric<T1>(string sql, object parameters)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return await dbConnection.QueryAsync<T1>(sql, parameters);
            }
        }

        /// <summary>
        /// Queries with SQL with Generic Return Typing Get One
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<T1> GetOneUsingSQLGeneric<T1>(string sql, object parameters)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return (await dbConnection.QueryAsync<T1>(sql, parameters)).FirstOrDefault();
            }
        }

        /// <summary>
        /// Updates single object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<bool> Update(TDto dto)
        {

            if (_transaction != null)
            {
                return await _transaction.Connection.UpdateAsync(dto);
            }
            else
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    return await dbConnection.UpdateAsync(dto);
                }
            }
        }

        /// <summary>
        /// Updates list of objects
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAll(IEnumerable<TDto> list)
        {

            if (_transaction != null)
            {
                return await _transaction.Connection.UpdateAsync(list);
            }
            else
            {
                using (IDbConnection dbConnection = Connection)
                {

                    dbConnection.Open();

                    using (IDbTransaction transaction = dbConnection.BeginTransaction())
                    {
                        try
                        {
                            bool updated = await dbConnection.UpdateAsync(list);

                            transaction.Commit();

                            return updated;
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Executes Arbitrary ANSI SQL, use for Update, Insert
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<int> ExecuteSQL(string sql, object parameters = null)
        {

            if(parameters == null)
            {
                parameters = new object();
            }

            if (_transaction != null)
            {
                return await _transaction.Connection.ExecuteAsync(sql, parameters);
            }
            else
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    return await dbConnection.ExecuteAsync(sql, parameters, commandType: CommandType.Text, commandTimeout: _commandTimeout);
                }
            }
        }

        /// <summary>
        /// Deletes One
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<bool> Delete(TDto dto)
        {

            if (_transaction != null)
            {
                return await _transaction.Connection.DeleteAsync(dto);
            }
            else
            {
                using (IDbConnection dbConnection = Connection)
                {

                    dbConnection.Open();
                    return await dbConnection.DeleteAsync(dto);
                }
            }
        }

        /// <summary>
        /// Returns Scalar Value (single integer)
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<int> ExecuteScalarQuery(string sql, object parameters = null)
        {

            if(parameters == null)
            {
                parameters = new object();
            }

            if (_transaction != null)
            {
                return await _transaction.Connection.ExecuteScalarAsync<int>(sql, parameters);
            }
            else
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    return await dbConnection.ExecuteScalarAsync<int>(sql, parameters);
                }
            }
        }

        /// <summary>
        /// Generic Return Type
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <typeparam name="T1"></typeparam>
        /// <returns></returns>    
        public async Task<IEnumerable<T1>> GetAsGeneric<T1>(string sql, object parameters)
        {

            if(_transaction == null)
            {
                return await _transaction.Connection.QueryAsync<T1>(sql, parameters);
            }
            else
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    return await dbConnection.QueryAsync<T1>(sql, parameters);
                }
            }
        }
    }
}