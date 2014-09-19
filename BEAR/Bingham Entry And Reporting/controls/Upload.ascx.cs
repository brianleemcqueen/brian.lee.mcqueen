using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.IO;

namespace BEAR.controls
{
    public partial class Upload : System.Web.UI.UserControl
    {
        public string uploadText = "Upload file:";
        public string statusText = "Status:";
        public string submitText = "Upload File";
        public string uploadFolder = "c:\\temp\\";
        public string nextPage = "default.aspx";

        protected void Page_Load(object sender, EventArgs e)
        {
            LabelUp.Text = uploadText;
            LabelStatus.Text = statusText;
            ButtonUpload.Text = submitText;
        }

        protected void ButtonUpload_Click(object sender, EventArgs e)
        {
            if(fileName.PostedFile != null) 
            {
                String sPath=uploadFolder;

                String sFileInfo = "<br /><br />FileName: "
                    + fileName.PostedFile.FileName
                    + "<br /><br />ContentType: " 
                    + fileName.PostedFile.ContentType
                    ;

                LabelStatus.Visible = true;
                LabelStatusMessage.Visible = true;

                try 
                {
                    String fileToUpload = fileName.PostedFile.FileName.Substring(fileName.PostedFile.FileName.LastIndexOf('\\')+1);
                    String uploadFilePathAndName = sPath + fileToUpload;
                    fileName.PostedFile.SaveAs(uploadFilePathAndName);
                    LabelStatusMessage.Text = "File uploaded successfully." + sFileInfo;
                    ButtonNext.Visible = true;
                    ButtonUpload.Visible = false;
                    ButtonCancel.Visible = true;
                    fileName.Visible = false;
                    LabelUp.Visible = false;
                    Session["uploadedFilePathAndName"] = uploadFilePathAndName;


                } 
                catch(Exception ex) 
                {
                    LabelStatusMessage.Text = "Error saving file " + sFileInfo + "<br />" + ex.Message.ToString();
                }

            }
        }


        protected void ButtonNext_Click(object sender, EventArgs e)
        {

            Response.Redirect(nextPage);

        }


        protected void ButtonCancel_Click(object sender, EventArgs e)
        {
            ButtonNext.Visible = false;
            ButtonUpload.Visible = true;
            ButtonCancel.Visible = false;
            fileName.Visible = true;
            LabelUp.Visible = true;
            LabelStatusMessage.Text = "Process Canceled<br />Ready to Upload a New File.";
            if (Session["uploadedFilePathAndName"] != null)
            {
                try
                {
                    System.IO.File.Delete(Session["uploadedFilePathAndName"].ToString());
                }
                catch (Exception ex)
                {
                    LabelStatusMessage.Text += "<br />" + ex.Message.ToString();
                }
            }



        }
    }
}