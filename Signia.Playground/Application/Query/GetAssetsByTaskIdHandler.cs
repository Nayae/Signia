using Signia.Core.CQRS.Query;
using Signia.Playground.Domain.Query;

namespace Signia.Playground.Application.Query;

public class GetAssetsByTaskIdHandler : QueryHandler<GetAssetsByTaskIdQuery, string[]>
{
    protected override Task<string[]> Handle(GetAssetsByTaskIdQuery query)
    {
        return Task.FromResult(
            new[] { "Hello from GetAssetsByTaskIdHandler!" }
        );
    }
}