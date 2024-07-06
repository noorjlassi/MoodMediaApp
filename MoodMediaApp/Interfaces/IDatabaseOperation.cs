using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoodMediaApp.Interfaces;
public interface IDatabaseOperation
{
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
    Task<int> ExecuteScalarAsync(string query, object parameters);
    Task<int> ExecuteCommandAsync(string query, object parameters);
}