using System;
using System.Collections.Generic;
using NZWalks.Core.Data;
using NZWalks.Core.Dto;
using NZWalks.Core.QueryParameters;

namespace NZWalks.Core.Extensions;

public static class WalkOrderExtension
{
    public static Dictionary<OrderKey, OrderDirection> OrderByFields(this WalkOrder walkOrder)
    {
        if (string.IsNullOrWhiteSpace(walkOrder.OrderBy))
        {
            return [];
        }

        var fields = new Dictionary<OrderKey, OrderDirection>();
        foreach (
            var entry in walkOrder.OrderBy.Split(
                ",",
                StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries
            )
        )
        {
            if (entry.StartsWith("-"))
            {
                fields[OrderKey.FromValue(entry[1..])] = OrderDirection.Descending;
            }
            else
            {
                fields[OrderKey.FromValue(entry)] = OrderDirection.Ascending;
            }
        }

        return fields;
    }
}
