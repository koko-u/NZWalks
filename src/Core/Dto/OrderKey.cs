using Ardalis.SmartEnum;

namespace NZWalks.Core.Dto;

public sealed class OrderKey(string name, string value) : SmartEnum<OrderKey, string>(name, value)
{
    public static readonly OrderKey Id = new(nameof(Id), "id");
    public static readonly OrderKey WalkName = new(nameof(WalkName), "name");
    public static readonly OrderKey Length = new(nameof(Length), "length");
    public static readonly OrderKey RegionCode = new(nameof(RegionCode), "region_code");
    public static readonly OrderKey Difficulty = new(nameof(Difficulty), "difficulty");
}
