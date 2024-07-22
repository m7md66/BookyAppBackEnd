using Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Services
{
    public class ManageFiles
    {
        public ManageFiles() { }

        //public async Task<IResponse<List<FileUploadReturnDto>>> CreateFilesAsync(FilesWithFileTypeIdDto dto, string path, int userId)
        //{
        //    var output = new Response<List<FileUploadReturnDto>>();

        //    var fileUploadedList = new List<FileUploadReturnDto>();

        //    try
        //    {
        //        var maxFileSize = 10;//ByMegaBytes
        //        if (dto.Files.Count <= 0) return null;
        //        var ValidationResult = await new FilesValidation(typeof(FileTypeEnums), maxFileSize).ValidateAsync(dto.Files);
        //        if (!ValidationResult.IsValid)
        //        {
        //            return output.AppendErrors(ValidationResult.Errors);
        //        }
        //        foreach (var file in dto.Files)
        //        {
        //            var fileName = file.FileName;

        //            var extention = "." + fileName.Split('.')[fileName.Split('.').Length - 1];

        //            var newFileName = Guid.NewGuid() + extention;
        //            decimal? fileSize = 0;
        //            string root = _hostEnvironment.ContentRootPath;

        //            var pathDirectory = root + "/" + path;

        //            if (!Directory.Exists(pathDirectory))
        //            {
        //                Directory.CreateDirectory(pathDirectory);
        //            }

        //            var pathFile = Path.Combine(pathDirectory, newFileName);

        //            using (var stream = System.IO.File.Create(pathFile))
        //            {
        //                await file.CopyToAsync(stream);
        //                fileSize = file.Length;
        //            }

        //            var fileUpload = new FileUploadReturnDto();
        //            fileUpload.FilePath = _filePathSetting.AttachesPath + newFileName;
        //            // convert size to megabyte and round to 4 number decimal
        //            fileUpload.fileSize = Math.Round((decimal)(fileSize / 1048576), 4);
        //            fileUpload.OriginalName = fileName;
        //            fileUpload.FullPath = _baseUrlSetting.BaseServerURL + "/" + fileUpload.FilePath;
        //            fileUpload.AttachTypeId = dto.AttachTypeId;
        //            fileUploadedList.Add(fileUpload);
        //        }
        //        if (fileUploadedList.Count > 0)
        //        {
        //            foreach (var file in fileUploadedList)
        //            {
        //                var entity = _mapper.Map<PickFileDraft>(file);
        //                entity.CreatedAt = DateTime.Now;
        //                entity.CreatedByUserId = userId;
        //                entity.PageRoute = dto.PageRoute;
        //                entity.AttachTypeId = dto.AttachTypeId;
        //                await _pickedFileRepo.AddAsync(entity);
        //            }
        //        }
        //        await _unitOfWork.CommitAsync();

        //        return output.CreateResponse(fileUploadedList);
        //    }
        //    catch (System.Exception ex)
        //    {
        //        return output.CreateResponse(MessageCodes.Failed, ex.ToString());
        //    }
        //}

        //public string UploadFile(IFormFile file) { 
        
        //}
    }
}
