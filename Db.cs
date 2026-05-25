using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;


public class DB
{
    DataSet ds;

    public DB()
    {
        ds = new DataSet();
    }
    public string QueryScalar(string Query)  
    {
        string str;
        string conString = "server=localhost;uid=Favorite;pwd=%%%%;database=users";
        MySqlConnection con = new MySqlConnection(conString);
        MySqlCommand cmd = new MySqlCommand(Query, con);
        con.Open();


        try
        {
            try
            {
                str = cmd.ExecuteScalar().ToString();
            }
            finally
            {
                cmd.Dispose();
                con.Close();
                con.Dispose();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return str;
    }
}

