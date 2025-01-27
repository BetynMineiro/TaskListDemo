using Task.CrossCutting.Enum.Common;

namespace Task.CrossCutting.RequestObjects;

public  class PagedRequest
{
    public Status? Status { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 15;
}