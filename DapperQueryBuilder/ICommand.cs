using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Libraries.DapperBuilder
{
    /// <summary>
    /// Any command (Contains Connection, SQL, and Parameters)
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// SQL of Command
        /// </summary>
        string Sql { get; }

        /// <summary>
        /// Parameters of Command
        /// </summary>
        DynamicParameters Parameters { get; }

        /// <summary>
        /// Underlying connection
        /// </summary>
        IDbConnection Connection { get; }
    }
}
