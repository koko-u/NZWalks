using System.Collections.Generic;
using NZWalks.Core.Data;

namespace NZWalks.Core.Responses;

/// <summary>
/// ページネーションを保持したレスポンスボディです
/// </summary>
/// <typeparam name="T"></typeparam>
public sealed class PagingItems<T>
{
    /// <summary>
    /// レスポンスとして返却するリストです
    /// </summary>
    public List<T> Items { get; init; } = [];

    /// <summary>
    /// 現在のページなどを保持します
    /// </summary>
    public required PageInfo Page { get; init; }
}
