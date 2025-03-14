using Microsoft.EntityFrameworkCore;
using YazılımOrg15Eylul_ETicaret.Data;
using YazılımOrg15Eylul_ETicaret.Models;

namespace YazılımOrg15Eylul_ETicaret.Services;

public class SupplierService
{
    Yazilima15EylulETicaretContext context = new();

    public async Task<List<Supplier>> GetSuppliersAsync()
    {
        List<Supplier> suppliers = await context.Suppliers.ToListAsync();
        return suppliers;
    }



    public static bool SupplierInsert(Supplier supplier)
    {
        //metod static olduğu için context direk gelmez
        using (Yazilima15EylulETicaretContext context = new())
        {
            try
            {
                context.Add(supplier);
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

    }

    public async Task<Supplier?> GetSupplierDetailsAsync(int? id)
    {
        Supplier? supplier = await
             context
            .Suppliers
            .FirstOrDefaultAsync(s => s.SupplierID == id);
        return supplier;

    }


    public static bool SupplierUpdate(Supplier supplier)
    {
        //metod static olduğu için context direk gelmez
        using (Yazilima15EylulETicaretContext context = new())
        {
            try
            {
                context.Update(supplier);
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

    }


    public static bool SupplierDelete(int id)
    {
        //metod static olduğu için context direk gelmez
        using (Yazilima15EylulETicaretContext context = new())
        {
            try
            {
                Supplier? supplier = context
                    .Suppliers
                    .FirstOrDefault(s => s.SupplierID == id);


                supplier!.Active = false;
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
