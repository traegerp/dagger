using System;

namespace Dagger.Data.Connections
{
    public class Connection:IConnection
    {
        private string _connectionString;

        public Connection(string connectionString){
            _connectionString = connectionString;
        }

        public string GetConnectionString(){
            if(!String.IsNullOrEmpty(_connectionString)){
                return _connectionString;
            }
            else{
                throw new Exception("No connection string");
            }
        }
    }
}