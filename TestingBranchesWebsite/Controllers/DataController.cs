using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Data.SQLite;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.IO;

public class DataController : Controller
{

  public JsonResult CheckLogin(string username, string password)
  {
    string connectionString = $"Data Source={Server.MapPath("~/App_Data/webdata.db")};Version=3;";
    string decryptionKey = "b14ca5898a4e4133bbce2ea2315a1916";

    using (SQLiteConnection db = new SQLiteConnection(connectionString))
    {
      db.Open();

      using (SQLiteCommand cmd = db.CreateCommand())
      {
        cmd.Parameters.Clear();
        cmd.CommandText = @"SELECT * FROM login WHERE username = @username";
        cmd.Parameters.AddWithValue("@username", username);

        using (SQLiteDataReader reader = cmd.ExecuteReader())
        {
          if (reader.Read()) //if it reads, username is found
          {
            string storedPassword = reader["password"].ToString();
            storedPassword = DecryptString(decryptionKey, storedPassword);

            if (storedPassword == password)
            {
              return Json(new { success = true });
            }
            else
            {
              return Json(new { success = false });
            }
          }
          else
          {
            return Json(new { success = false });
          }
        }
      }
    }
  }

  public static string DecryptString(string key, string cipherText)
  {
    byte[] iv = new byte[16];
    byte[] buffer = Convert.FromBase64String(cipherText);

    using (Aes aes = Aes.Create())
    {
      aes.Key = Encoding.UTF8.GetBytes(key);
      aes.IV = iv;
      ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

      using (MemoryStream memoryStream = new MemoryStream(buffer))
      {
        using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
        {
          using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
          {
            return streamReader.ReadToEnd();
          }
        }
      }
    }
  }

  /*public string EncryptString()
  {
    var key = "b14ca5898a4e4133bbce2ea2315a1916"; byte[] iv = new byte[16];
    var plainText = "DRAdminMalta";

    byte[] array;

    using (Aes aes = Aes.Create())
    {
      aes.Key = Encoding.UTF8.GetBytes(key);
      aes.IV = iv;

      ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

      using (MemoryStream memoryStream = new MemoryStream())
      {
        using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
        {
          using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
          {
            streamWriter.Write(plainText);
          }

          array = memoryStream.ToArray();
        }
      }
    }

    return Convert.ToBase64String(array);
  }*/

  public class Branch
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public int Status { get; set; }
  }

  [HttpGet]
  public JsonResult CheckBranchStatus()
  {
    string connectionString = $"Data Source={Server.MapPath("~/App_Data/webdata.db")};Version=3;";
    List<Branch> branchList = new List<Branch>();

    using (SQLiteConnection db = new SQLiteConnection(connectionString))
    {
      db.Open();

      using (SQLiteCommand cmd = db.CreateCommand())
      {
        cmd.Parameters.Clear();
        cmd.CommandText = @"SELECT * FROM status ORDER BY id ASC";

        using (SQLiteDataReader reader = cmd.ExecuteReader())
        {
          DataTable dataTable = new DataTable();
          dataTable.Load(reader);

          foreach (DataRow row in dataTable.Rows)
          {
            Branch branch = new Branch
            {
              Id = Convert.ToInt32(row["id"]),
              Name = Convert.ToString(row["name"]),
              Status = Convert.ToInt32(row["status"]),
            };

            branchList.Add(branch);
          }
        }
      }
    }

    return Json(branchList, JsonRequestBehavior.AllowGet);
  }

}
