namespace NZWalks.Core.Shared.Paging;

public static class PagingExtension
{
    public static int Limit(this Paging paging) => paging.PageSize;

    public static int Offset(this Paging paging) => (paging.PageNumber - 1) * paging.PageSize;
}
