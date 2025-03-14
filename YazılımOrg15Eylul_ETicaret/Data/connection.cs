using Microsoft.Data.SqlClient;

namespace YazılımOrg15Eylul_ETicaret.Data;

public class connection
{
    public static SqlConnection baglanti
    {
        get
        {
            SqlConnection sqlcon = new SqlConnection("Server=KARTAL\\MSSQLSERVER01; Database=YazilimaOrg15EylulETicaret; Trusted_Connection=True;TrustServerCertificate=True");
            return sqlcon;
        }
    }



}
