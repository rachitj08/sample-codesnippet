using Microsoft.AspNetCore.Mvc;
using ParkingProvider.Portal.Model;
using ParkingProvider.Portal.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using uhv.Customer.Model.Model.StorageModel;
using System.Collections.Generic;
using Common.Enum.StorageEnum;
using Core.Storage.ServiceWorker.IMediaUploadServices;
using Core.Storage.Helper;
using Microsoft.Extensions.Options;
using uhv.Customer.Model.Model;
using Net.Codecrete.QrCodeGenerator;
using System;
using Common.Model;
using Microsoft.AspNetCore.Http;
using Core.Storage.ServiceWorker.MediaUploadServices;

namespace ParkingProvider.Portal
{
    public class QRGeneratorController : Controller
    {
        private static readonly QrCode.Ecc[] errorCorrectionLevels = { QrCode.Ecc.Low, QrCode.Ecc.Medium, QrCode.Ecc.Quartile, QrCode.Ecc.High };
        private readonly IImageUploadService imageUploadService;
        private readonly IDatabaseContext db;
        private readonly CustomerBaseUrlsConfig _appSettings;
        private readonly CommonConfig _commonConfig;
        private readonly IUploadFileService _uploadFileService;
        public QRGeneratorController(IImageUploadService imageUploadService, IDatabaseContext databaseContext, IOptions<CustomerBaseUrlsConfig> appSettings, 
            IOptions<CommonConfig> commonConfig, IUploadFileService uploadFileService)
        {
            this.imageUploadService = imageUploadService;
            this.db = databaseContext;
            this._appSettings = appSettings.Value;
            _commonConfig = commonConfig.Value;
            _uploadFileService = uploadFileService;
        }
        public IActionResult Index()
        {
            QRModel model = new QRModel();
            ViewData["BasePath"] = "https://demo13.damcogroup.com:32783/provider-web";//$"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
            return View(FillDropDown(model));
        }
    
        #region Post Request for QR
        [HttpPost]
        public IActionResult Index(QRModel model)
        {
            ViewData["BasePath"] = "https://demo13.damcogroup.com:32783/provider-web";//$"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
            //int ecc = Math.Clamp(ecc ?? 1, 0, 3);
            //int borderWidth = Math.Clamp(borderWidth ?? 3, 0, 999999);
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            byte[] byteArray = null;
            string pipe = "|";

            if (!string.IsNullOrEmpty(model.SubLocation))
                sb.Append(model.SubLocation + pipe);
            if (!string.IsNullOrEmpty(model.SubLocationType))
                sb.Append(model.SubLocationType + pipe);
            if (!string.IsNullOrEmpty(model.ActivityCode))
                sb.Append(model.ActivityCode + pipe);
            if (!string.IsNullOrEmpty(model.ParkingSpotId))
                sb.Append(model.ParkingSpotId);
            string generalText = sb.ToString();
            if (generalText.EndsWith("|"))
                generalText = generalText.Remove(generalText.Length - 1, 1);
            string ecryptedText = EncryptString(generalText, _commonConfig.ScanEncryptionKey);
            var qrCode = QrCode.EncodeText(ecryptedText, errorCorrectionLevels[1]);
            model.EncryptedData = ecryptedText;
            byteArray = qrCode.ToPng(20, 1);
            int qrResult = SaveQR(model, byteArray, System.Convert.ToBase64String(byteArray));
            if (qrResult > 0)
                model.QRString = "data:image/png;base64," + System.Convert.ToBase64String(byteArray);
            else
                ViewBag.Message = "Something went wrong.";
           return View(FillDropDown(model));
        }

        #endregion

        public QRModel FillDropDown(QRModel qRModel)
        {
            qRModel.ParkingProviderList = new SelectList(db.GetParkingProvider(), "Id", "Name");
            qRModel.ParkingProviderLocationList = new SelectList(Enumerable.Empty<DropDownMaster>(), "Id", "Name");
            qRModel.SubLocationList = new SelectList(Enumerable.Empty<DropDownMaster>(), "Id", "Name");
            qRModel.ActivityCodeList = new SelectList(db.GetActivityCode(), "Id", "Name");
            qRModel.ParkingSpotIdList = new SelectList(Enumerable.Empty<DropDownMaster>(), "Id", "Name");
            return qRModel;
        }
        
        public string EncryptString(string input, string key)
        {
            byte[] clearBytes = System.Text.Encoding.Unicode.GetBytes(input);
            using (System.Security.Cryptography.Aes encryptor = System.Security.Cryptography.Aes.Create())
            {
                System.Security.Cryptography.Rfc2898DeriveBytes pdb = new System.Security.Cryptography.Rfc2898DeriveBytes(key, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                {
                    using (System.Security.Cryptography.CryptoStream cs = new System.Security.Cryptography.CryptoStream(ms, encryptor.CreateEncryptor(), System.Security.Cryptography.CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    input = System.Convert.ToBase64String(ms.ToArray());
                }
            }
            return input;
        }
        //public void SaveQR(string fileName, byte[] fileByte, string base64,string providerId)
        public int SaveQR(QRModel model, byte[] fileByte, string base64)
        {
            var mediaUploadView = new MediaUploadView();
            Document document = new Document()
            {
                FileGuid = System.Guid.NewGuid(),
                FileName = model.ActivityCode,
                FileExtenstion = "*.png",
                Filebytes = fileByte,
                FileUniqueName = "QRCode",
                PicBase64 = base64
            };

            mediaUploadView.Documents = new List<Document>();
            mediaUploadView.ReferenceId = System.Convert.ToInt64(model.ParkingProvider);
            mediaUploadView.ReferenceType = (int)MediaReferenceEnum.QRGenerator;
            mediaUploadView.CreatedBy = System.Convert.ToInt64(model.ParkingProviderLocation);
            mediaUploadView.Documents.Add(document);

            var result = this.imageUploadService.SaveImages(mediaUploadView);
            if (result.SavedPathList.Count > 0)
                model.SavedPath = result.SavedPathList[0];
            model.ActivityId = db.GetActivityIdByCode(model.ActivityCode);
            model.QRMappingCode = model.ActivityCode + " " + model.SubLocation;
            model.ActivityName = db.GetActivityNameByCode(model.ActivityCode);

            int subLocationId = db.CheckSubLocationExist(model);
            if (subLocationId == 0)
                return db.SaveQRData(model);
            else
                return db.UpdateQRData(model);
        }
      
        [HttpGet]
        public IEnumerable<DropDownMaster> GetParkingProviderLocation(string providerId)
        {
            if (string.IsNullOrEmpty(providerId))
                return Enumerable.Empty<DropDownMaster>();
            var data = db.GetParkingProviderLocation(System.Convert.ToInt64(providerId));
            return data;
        }

        [HttpGet]
        public IEnumerable<DropDownMaster> GetSubLocation(string providerLocationId)
        {
            if (string.IsNullOrEmpty(providerLocationId))
                return Enumerable.Empty<DropDownMaster>();
            var data = db.GetSubLocation(System.Convert.ToInt64(providerLocationId));
            return data;
        }

        [HttpGet]
        public IEnumerable<DropDownMaster> GetParkingSpot(string subLocationId)
        {
            if (string.IsNullOrEmpty(subLocationId))
                return Enumerable.Empty<DropDownMaster>();
            var data = db.GetParkingSpotId(System.Convert.ToInt64(subLocationId));
            return data;
        }

        public IActionResult GetQRList(string providerId)
        {
            ViewData["BasePath"] = "https://demo13.damcogroup.com:32783/provider-web";//$"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
            //providerId = "7";
            QRDataModel model = new QRDataModel();
            model.ParkingProviderList = new SelectList(db.GetParkingProvider(), "Id", "Name");
            model.ParkingProviderLocationList = new SelectList(db.GetParkingProviderLocation(System.Convert.ToInt64(providerId)), "Id", "Name");
            if (string.IsNullOrEmpty(providerId))
            {

                return View(model);
            }

            List<QRListModel> qRListModel = new List<QRListModel>();
            var data = db.GetQRList(System.Convert.ToInt64(providerId));
            foreach (var item in data)
            {
                QRListModel qrModel = new QRListModel();
                if (!string.IsNullOrEmpty(item.EncryptedData))
                {
                    qrModel.Name = item.Name;
                    qrModel.QRPath = _appSettings.StorageRootPath + item.QRPath;
                    qrModel.EncryptedData = item.EncryptedData;
                    qRListModel.Add(qrModel);
                }
            }
            model.qRListModels = qRListModel;
            return View(model);
        }
    }
}
