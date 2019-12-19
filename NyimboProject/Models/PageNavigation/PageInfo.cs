using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NyimboProject.Models.PageNavigation
{
    public class PageInfo
    {
        // номер текущей страницы
        public int PageNumber { get; set; }

        //кол-во объектов на странице
        public int PageSize { get; set; }

        //Всего объектов
        public int TotalItems { get; set; }

        //Всего страниц
        public int TotalPage
        {
            get
            {
                return (int)Math.Ceiling((decimal)TotalItems / PageSize);
            }
        }
    }
}