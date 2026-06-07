using Microsoft.AspNetCore.Mvc;

namespace NZWalks.Core.Shared.Paging;

/// <summary>
/// ページネーションのためのクエリパラメータです
/// </summary>
public sealed class Paging
{
    [FromQuery(Name = "page")]
    public int PageNumber { get; set; } = 1;

    [FromQuery(Name = "page_size")]
    public int PageSize { get; set; } = 10;
}
