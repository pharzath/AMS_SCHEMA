using Microsoft.JSInterop;

namespace AMS_SCHEMA.Class
{
    public class BrowserService
    {
        readonly IJSRuntime _jsRuntime;

        public BrowserService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task GotoSection(string id)
        {
            await _jsRuntime.InvokeVoidAsync("GotoSection",id);
        }
    }
}
