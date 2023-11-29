using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Data.SQLite;
using System.Data;
using System.Web.Services;
using System.Security.Cryptography;
using System.Text;
using System.IO;

public class DataController : Controller
{
  public JsonResult CheckLogin(string username, string password)
  {
    string connectionString = $"Data Source={Server.MapPath("~/App_Data/webdata.db")};Version=3;";
    var key = "b14ca5898a4e4133bbce2ea2315a1916";

    using (SQLiteConnection db = new SQLiteConnection(connectionString))
    {
      db.Open();

      string query = "SELECT * FROM login WHERE username = @username";
      using (SQLiteCommand command = new SQLiteCommand(query, db))
      {
        command.Parameters.AddWithValue("@username", username);

        using (SQLiteDataReader reader = command.ExecuteReader())
        {
          if (reader.Read()) //if it reads, username is found
          {
            string storedPassword = reader["password"].ToString();
            storedPassword = DecryptString(key, storedPassword);

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

}
