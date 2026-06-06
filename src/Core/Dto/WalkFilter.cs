using Microsoft.AspNetCore.Mvc;

namespace NZWalks.Core.Dto;

/// <summary>
/// walks を取得する時の条件です
/// </summary>
public sealed class WalkFilter
{
    /// <summary>
    /// 名前の部分一致
    /// </summary>
    [FromQuery(Name = "name_like")]
    public string? NameLike { get; set; }

    /// <summary>
    /// 最低距離(Km)
    /// </summary>
    [FromQuery(Name = "min_length")]
    public double? MinLength { get; set; }

    /// <summary>
    /// 最長距離(Km)
    /// </summary>
    [FromQuery(Name = "max_length")]
    public double? MaxLength { get; set; }

    /// <summary>
    /// 地域(code)
    /// </summary>
    [FromQuery(Name = "region_code")]
    public string? RegionCode { get; set; }

    /// <summary>
    /// 難易度
    /// </summary>
    [FromQuery(Name = "difficulty")]
    public string? Difficulty { get; set; }
}
