using Microsoft.AspNetCore.SignalR;

namespace YazılımOrg15Eylul_ETicaret.Hubs;

public class AdminHub : Hub
{
    //

    public async Task KullaniciHareketGonder(string kullanici, string aktivite)
    {
        await Clients.All.SendAsync("KullaniciHareketiniYakala", kullanici, aktivite);
    }








}
