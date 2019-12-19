using NyimboProject.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NyimboProject.Models.UploadedModel
{

    public abstract class UploadedFile
    {
        ///Загруженый пользователем фаил
        public HttpPostedFileBase File { get; set; }

        public virtual string FileName
        {
            get
            {
                if (File != null)
                    return File.FileName;
                else
                    return String.Empty;
            }
        }
    }

    public class UploadedMusic : UploadedFile
    {
        [Display(Name = "Mp3 File")]
        [FileExtensions(Extensions = "mp3", ErrorMessage = "Please upload an music.mp3 file.")]
        public override string FileName => base.FileName;
    }

    public class UploadedImage : UploadedFile
    {
        [Display(Name = "Jpg File")]
        [FileExtensions(Extensions = "jpg", ErrorMessage = "Please upload an image.jpg file.")]
        public override string FileName => base.FileName ?? "/Content/img/DefaultImage.jpg";
    }

    public class UploadedModelView
    {
        public UploadedMusic Music { get; set; }
        public UploadedImage Image { get; set; }
    }
}