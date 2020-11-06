using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Data;

namespace WinProductImage
{
    public class ProductDB : IDisposable
    {
        MySqlConnection conn;
        public ProductDB()
        {
            string strConn = ConfigurationManager.ConnectionStrings["myDB"].ConnectionString;

            conn = new MySqlConnection(strConn);
            conn.Open();
        }

        public void Dispose()
        {
            conn.Close();
        }

        /// <summary>
        /// 등록된 제품목록을 조회
        /// </summary>
        /// <returns></returns>
        public DataTable GetProductList()
        {
            string sql = @"SELECT productID, productName, productPrice 
                             FROM product";

            MySqlDataAdapter da = new MySqlDataAdapter(sql, conn);
            DataTable dt = new DataTable();

            da.Fill(dt);
            return dt;
        }

        public DataTable GetProductListImage()
        {
            string sql = @"SELECT p.productID, productName, productPrice, productImgFileName
  FROM product p join productimage pi on p.productID = pi.productID
 where pi.IsMainImage = 'Y'";

            MySqlDataAdapter da = new MySqlDataAdapter(sql, conn);
            DataTable dt = new DataTable();

            da.Fill(dt);
            return dt;
        }

        public DataTable GetProductListImageBLOB()
        {
            string sql = @"SELECT p.productID, productName, productPrice, productImage
                             FROM product p join productimage pi on p.productID = pi.productID
                            where productImgFileName is null;";

            MySqlDataAdapter da = new MySqlDataAdapter(sql, conn);
            DataTable dt = new DataTable();

            da.Fill(dt);
            return dt;
        }


        public bool AddProductImage(int pid, string path)
        {
            string sql = @"insert into productimage (productID, productImgFileName)
                                             values (@pid, @path)";

            MySqlCommand cmd = new MySqlCommand(sql, conn);

            cmd.Parameters.Add("@pid", MySqlDbType.Int32);
            cmd.Parameters["@pid"].Value = pid;

            cmd.Parameters.Add("@path", MySqlDbType.VarChar);
            cmd.Parameters["@path"].Value = path;

            int iResult = cmd.ExecuteNonQuery();
            if (iResult > 0)
                return true;
            else
                return false;
        }

        public bool AddProductImage(int pid, byte[] data)
        {
            string sql = @"insert into productimage (productID, productImage)
                             values (@pid, @data)";

            MySqlCommand cmd = new MySqlCommand(sql, conn);

            cmd.Parameters.Add("@pid", MySqlDbType.Int32);
            cmd.Parameters["@pid"].Value = pid;
            cmd.Parameters.Add("@data", MySqlDbType.Blob);
            cmd.Parameters["@data"].Value = data;

            int iRowsAffect = cmd.ExecuteNonQuery();

            if (iRowsAffect > 0)
                return true;
            else
                return false;
        }

        public bool DelProductImage(int pid, string path)
        {
            string sql = @"delete from productimage 
                            where productID=@pid 
                              and productImgFileName=@path";

            MySqlCommand cmd = new MySqlCommand(sql, conn);

            cmd.Parameters.Add("@pid", MySqlDbType.Int32);
            cmd.Parameters["@pid"].Value = pid;
            cmd.Parameters.Add("@path", MySqlDbType.VarChar);
            cmd.Parameters["@path"].Value = path;
            
            int iRowAffect = cmd.ExecuteNonQuery();
            if (iRowAffect > 0)
                return true;
            else
                return false;
        }

        public bool DelProductImage(int imageID)
        {
            string sql = @"delete from productimage where productImageID=@productImageID";

            MySqlCommand cmd = new MySqlCommand(sql, conn);

            cmd.Parameters.Add("@productImageID", MySqlDbType.Int32);
            cmd.Parameters["@productImageID"].Value = imageID;

            int iRowAffect = cmd.ExecuteNonQuery();
            if (iRowAffect > 0)
                return true;
            else
                return false;

        }

        public DataTable GetProductImageList(int pid)
        {
            string sql = @"SELECT productImageID, productID, productImage, 
                ifnull(productImgFileName, concat('BLOB이미지/',productImageID)) productImgFileName 
                             FROM productimage 
                            WHERE productID = @pid";

            DataTable dt = new DataTable();
            MySqlDataAdapter da = new MySqlDataAdapter(sql, conn);

            da.SelectCommand.Parameters.Add("@pid", MySqlDbType.Int32);
            da.SelectCommand.Parameters["@pid"].Value = pid;

            da.Fill(dt);
            return dt;
        }
    }
}
