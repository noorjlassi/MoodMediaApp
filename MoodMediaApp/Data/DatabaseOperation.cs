using Dapper;
using MoodMediaApp.Interfaces;
using System.Data.SqlClient;

namespace MoodMediaApp.Data;

public class DatabaseOperation : IDatabaseOperation
{
    private readonly string _connectionString;
    private SqlConnection _connection;
    private SqlTransaction _transaction;

    public DatabaseOperation(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<int> ExecuteScalarAsync(string query, object parameters)
    {
        return await _connection.ExecuteScalarAsync<int>(query, parameters, _transaction);
    }

    public async Task<int> ExecuteCommandAsync(string query, object parameters)
    {
        return await _connection.ExecuteAsync(query, parameters, _transaction);
    }

    public async Task BeginTransactionAsync()
    {
        _connection = new SqlConnection(_connectionString);
        try
        {
            await _connection.OpenAsync();
            _transaction = _connection.BeginTransaction();
        }
        catch
        {
            _connection.Dispose();
            throw;
        }
    }

    public async Task CommitTransactionAsync()
    {
        try
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
            }
        }
        finally
        {
            if (_connection != null)
            {
                await _connection.CloseAsync();
                await _connection.DisposeAsync();
            }
        }
    }

    public async Task RollbackTransactionAsync()
    {
        try
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
            }
        }
        finally
        {
            if (_connection != null)
            {
                await _connection.CloseAsync();
                await _connection.DisposeAsync();
            }
        }
    }
}
