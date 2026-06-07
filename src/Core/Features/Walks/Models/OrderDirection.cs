using Ardalis.SmartEnum;

namespace NZWalks.Core.Features.Walks.Models;

/// <summary>
/// 並び順の昇順・降順
/// </summary>
/// <param name="name"></param>
/// <param name="value"></param>
public sealed class OrderDirection(string name, string value)
    : SmartEnum<OrderDirection, string>(name, value)
{
    /// <summary>
    /// 昇順です
    /// </summary>
    public static readonly OrderDirection Ascending = new(nameof(Ascending), "ASC");

    /// <summary>
    /// 降順です
    /// </summary>
    public static readonly OrderDirection Descending = new(nameof(Descending), "DESC");
}
