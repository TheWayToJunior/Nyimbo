using NyimboProject.Models.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NyimboProject.Models
{
    public class Song
    {
        /// <summary>
        /// Entety song
        /// </summary>
        public int Id { get; set; }

        //Название песни
        public string Name { get; set; }

        // Исполниетль
        public string Performer { get; set; }

        // Путь к файлу .mp3 на сервере
        public string SongPaht { get; set; }

        // Путь к файлу .img на сервере
        public string ImgPaht { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}