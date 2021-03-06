using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Libraries.DapperBuilder
{
    /// <summary>
    /// Any command (Contains Connection, SQL, and Parameters) which is complete for execution.
    /// </summary>
    public interface ICompleteCommand : ICommand
    {
    }
}
