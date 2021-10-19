﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Libraries.DapperBuilder
{
    /// <summary>
    /// Query Builder with one or more orderby clauses, which can still add more clauses to orderby
    /// </summary>
    public interface IOrderByBuilder : ICompleteCommand
    {
        /// <summary>
        /// Adds a new condition to orderby clauses.
        /// </summary>
        IOrderByBuilder OrderBy(FormattableString column);

        /// <summary>
        /// Adds offset and rowcount clauses
        /// </summary>
        ICompleteCommand Limit(int offset, int rowCount);
    }
}
