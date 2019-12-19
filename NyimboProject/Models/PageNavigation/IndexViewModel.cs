using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NyimboProject.Models.PageNavigation
{
    public class IndexViewModel
    {
        /// Информация о странице
        public PageInfo PageInfo { get; set; }

        /// Данные для вывода на указаную страницу
        public IEnumerable<Song> Songs { get; set; }
    }
}