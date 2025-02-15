﻿using System.Data;
using Dapper;
using ThinkCoreBE.Domain.Entities;
using ThinkCoreBE.Domain.Interfaces;

namespace ThinkCoreBE.Infrastructure.Persistance.CustomQueries
{
    public sealed class CustomerContext : DapperEntity<Customer>, ICustomerContext
    {
        private readonly IDbConnection _connection;

        public CustomerContext(IDbConnection connection) : base(connection)
        {
            _connection = connection;
        }

        public async Task<bool> ShouldAddNewCustomer(string cnpj, CancellationToken cancellationToken = default)
        {
            var sql = $@"
            SELECT NOT EXISTS (
                SELECT 1 
                FROM {typeof(Customer).Name}s
                WHERE CNPJ = @Cnpj
            )";

            return await Task.Run(async () => await _connection.ExecuteScalarAsync<bool>(sql, new { Cnpj = cnpj }), cancellationToken);
        }
    }
}
