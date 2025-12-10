using Microsoft.AspNetCore.SignalR;

namespace LibraryApp.Hubs
{
    public class BookHub : Hub
    {
        // Clients will listen to this
        public static async Task BookAdded(IHubContext<BookHub> context)
        {
            await context.Clients.All.SendAsync("BookAdded");
        }
    }
}