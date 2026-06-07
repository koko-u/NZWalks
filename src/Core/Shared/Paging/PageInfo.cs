namespace NZWalks.Core.Shared.Paging;

/// <summary>
/// ページネーションの情報を保持します
/// </summary>
/// <param name="CurrentPage">現在のページ番号です(1から)</param>
/// <param name="PageSize">ページサイズです</param>
/// <param name="TotalCount">取得したデータの総件数です</param>
public readonly record struct PageInfo(int CurrentPage, int PageSize, int TotalCount);
