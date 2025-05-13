using CloudNimble.BlazorEssentials.IndexedDb;
using Microsoft.JSInterop;

namespace VotingSystem.Blazor.WebAssembly.Infrastructure
{
    public class VotingSystemIndexDatabase(IJSRuntime jsRuntime) : IndexedDbDatabase(jsRuntime)
    {
        [ObjectStore(AutoIncrementKeys = true)]
        public IndexedDbObjectStore Votes { get; set; } = null!;
    }
}
