using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.Hosting;
using CommonClasses;
using System.Drawing.Imaging;

public partial class UploadImage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    [System.Web.Services.WebMethod()]
    public static string SaveImage(string image, string Acode)
    {
        System.Threading.Thread.Sleep(2000);
        if (image != "")
        {
            try
            {
                // Save Image to DB  & Create Image ID

                // Finish Save DB Code

                string ItemID = GetNumericImageID();
                
                string filepath = HostingEnvironment.MapPath("~/UploadeImages/" + Acode);
                if (!Directory.Exists(filepath)) Directory.CreateDirectory(filepath);

                filepath = HostingEnvironment.MapPath("~/UploadeImages/" + Acode + "/Large/");
                if (!Directory.Exists(filepath)) Directory.CreateDirectory(filepath); 

               // DirectoryInfo di = new DirectoryInfo(filepath);
               // int numJpg = di.GetFiles("*.jpg", SearchOption.AllDirectories).Length;

                FileStream fs;
                byte[] photo = Convert.FromBase64String(image);
                string fileName = HostingEnvironment.MapPath("~/UploadeImages/" + Acode + "/Large/" + ItemID + ".jpg");
                fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write);
                BinaryWriter br = new BinaryWriter(fs);
                br.Write(photo);
                br.Flush();
                br.Close();
                fs.Close();
                ImageProcessing objImg = new ImageProcessing();
                objImg.FileReadFrom = fileName;
                objImg.ImageFormat = ImageFormat.Jpeg;
                objImg.keepOriginal = false;
                

                objImg.ThumbWidth = 150;
                objImg.FileSaveTo = HostingEnvironment.MapPath("~/UploadeImages/" + Acode + "/Small/" + ItemID + ".jpg");
                filepath = HostingEnvironment.MapPath("~/UploadeImages/" + Acode + "/Small/");
                if (!Directory.Exists(filepath)) Directory.CreateDirectory(filepath);
                objImg.SaveThumbnailImage(objImg);

                objImg.ThumbWidth = 220;
                objImg.FileSaveTo = HostingEnvironment.MapPath("~/UploadeImages/" + Acode + "/Medium/" + ItemID + ".jpg");
                filepath = HostingEnvironment.MapPath("~/UploadeImages/" + Acode + "/Medium/");
                if (!Directory.Exists(filepath)) Directory.CreateDirectory(filepath);
                objImg.SaveThumbnailImage(objImg);

                //objImg.ThumbWidth = 550;
                //objImg.WaterMarkText = "DEEPAK";
                //objImg.FileSaveTo = HostingEnvironment.MapPath("~/UploadeImages/" + Aname + "/Large/" + (numJpg + 1).ToString() + ".jpg");
                //filepath = HostingEnvironment.MapPath("~/UploadeImages/" + Aname + "/Large/");
                //if (!Directory.Exists(filepath)) Directory.CreateDirectory(filepath);
                //objImg.SaveThumbnailImage(objImg);

            }
            catch (Exception e)
            {
                e.Message.ToString();
                return "false";
            }

        }
        return "true";
    }


    public static string GetNumericImageID()
    {
        DateTime dt = DateTime.Now;
        string NumericPOID = dt.Year.ToString() + dt.Month.ToString("00") + dt.Day.ToString("00") + dt.Hour.ToString("00") + dt.Minute.ToString("00") + dt.Second.ToString("00") + dt.Millisecond.ToString("000");
        return NumericPOID;
    }
}