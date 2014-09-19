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

namespace BEAR.controls
{
    public partial class UploadSaveAs : System.Web.UI.UserControl
    {
        public string uploadText = "Upload file:";
        public string saveText = "Save as:";
        public string statusText = "Status:";
        public string submitText = "Upload File";
        public string uploadFolder = "c:\\temp\\";

        protected void Page_Load(object sender, EventArgs e)
        {
            LabelUp.Text = uploadText;
            LabelSave.Text = saveText;
            LabelStatus.Text = statusText;
            uploadButton.Text = submitText;
        }

        protected void uploadButton_Click(object sender, EventArgs e)
        {
            if (savename.Value == "")
            {
                LabelStatusMessage.Text = "Please provide a 'save as' name.";
                return;
            }
            else if (filename.PostedFile != null)
            {
                String sPath = uploadFolder;

                String sFileInfo = "<br />FileName: "
                    + filename.PostedFile.FileName
                    + "<br />ContentType: "
                    + filename.PostedFile.ContentType
                    + "<br />Saved As: "

                    ;

                try
                {
                    filename.PostedFile.SaveAs(sPath + savename.Value);
                    LabelStatusMessage.Text = "File uploaded successfully." + sFileInfo;
                }
                catch (Exception ex)
                {
                    LabelStatusMessage.Text = "Error saving file " + sFileInfo + "<br />" + ex.Message.ToString();
                }

            }
        }
    }
}