using System;
using System.Collections.Generic;
using System.Text;

namespace Libraries.DapperBuilder
{
    /// <summary>
    /// Query Builder with one or more groupby clauses, which can still add more clauses to groupby
    /// </summary>
    public interface IGroupByBuilder : ICompleteCommand
    {
        IGroupByBuilder GroupBy(FormattableString groupBy);
        IGroupByHavingBuilder Having(FormattableString having);
        IOrderByBuilder OrderBy(FormattableString orderBy);
    }
}
