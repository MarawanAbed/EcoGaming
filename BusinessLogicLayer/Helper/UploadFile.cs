

//using Microsoft.AspNetCore.Http;

//namespace BusinessLogicLayer.Helper
//{
//    public class UploadFile
//    {
//        public static string UploadImage(IFormFile file, string destinationFolder)
//        {
//            var uniqueFileName = string.Empty;

//            if (file != null && file.Length > 0)
//            {
//                var uploadsFolder = Path.Combine(@"./wwwroot/", destinationFolder);
//                uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
//                var filePath = Path.Combine(uploadsFolder, uniqueFileName);
//                //   "./wwwroot/Images/6394736969856_Mohamed.jpg"
//                //handle unmanaged resources so that when they are no longer needed, they are released immediately
//                using (var fileStream = new FileStream(filePath, FileMode.Create))
//                {
//                    file.CopyTo(fileStream);
//                }
//            }
//            return uniqueFileName;
//        }

//    }
//}
using Microsoft.AspNetCore.Http;

namespace BusinessLogicLayer.Helper
{
    public class UploadFile
    {
        public static string UploadImage(IFormFile file, string destinationFolder)
        {
            if (file == null || file.Length <= 0)
                return string.Empty;

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", destinationFolder);
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            try
            {
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
                return uniqueFileName;
            }
            catch (Exception ex)
            {
                // Log the error if necessary
                throw new Exception("Error uploading file: " + ex.Message);
            }
        }
    }
}
