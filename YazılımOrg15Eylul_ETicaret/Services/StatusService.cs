using Microsoft.EntityFrameworkCore;
using YazılımOrg15Eylul_ETicaret.Data;
using YazılımOrg15Eylul_ETicaret.Models;

namespace YazılımOrg15Eylul_ETicaret.Services;

public class StatusService
{

    Yazilima15EylulETicaretContext context = new();

    public async Task<List<Status>> GetStatusesAsync()
    {
        List<Status> statuses = await context.Statuses.ToListAsync();
        return statuses;
    }



    public static bool StatusInsert(Status status)
    {
        //metod static olduğu için context direk gelmez
        using (Yazilima15EylulETicaretContext context = new())
        {
            try
            {
                context.Add(status);
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

    }


    public async Task<Status?> GetStatusDetailsAsync(int? id)
    {
        Status? status = await
             context
            .Statuses
            .FirstOrDefaultAsync(s => s.StatusID == id);
        return status;

    }


    public static bool StatusUpdate(Status status)
    {
        //metod static olduğu için context direk gelmez
        using (Yazilima15EylulETicaretContext context = new())
        {
            try
            {
                context.Update(status);
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

    }



    public static bool StatusDelete(int id)
    {
        //metod static olduğu için context direk gelmez
        using (Yazilima15EylulETicaretContext context = new())
        {
            try
            {
                Status? status = context
                    .Statuses
                    .FirstOrDefault(s => s.StatusID == id);


                status!.Active = false;
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

    }




}
