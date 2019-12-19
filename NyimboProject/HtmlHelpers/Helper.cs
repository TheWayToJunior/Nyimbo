using NyimboProject.Models.PageNavigation;
using System;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Mvc.Html;

namespace NyimboProject.HtmlHelpers
{
    public enum DirectionMode { Left = -1, Right = 1 }

    public static class Helper
    {
        public static MvcHtmlString PageLinks(this HtmlHelper helper, PageInfo pageInfo, Func<int, string> pageUrl)
        {
            var stringBilder = new StringBuilder();

            // проверка на то случай если у нас всего одна стр.
            if (pageInfo.TotalPage > 1)
                // создание нужного кол-во страниц
                for (int i = 1; i <= pageInfo.TotalPage; i++)
                {
                    // создание тега <a>
                    var tag = new TagBuilder("a");

                    // добавление атоебута href="" и формирование ссылкт на страницу
                    tag.MergeAttribute("href", pageUrl(i));
                    tag.InnerHtml = i.ToString();

                    // добавление классов CSS для выбраной страницы 
                    if (i == pageInfo.PageNumber)
                        tag.AddCssClass("btn__link__select"); // подключить стили!!!
                    else
                        //  добавление классов CSS для остальных страниц
                        tag.AddCssClass("btn__link");

                    stringBilder.Append(tag.ToString());
                }

            return MvcHtmlString.Create(stringBilder.ToString());
        }

        /// <summary>
        /// AjaxHelper для создания постраничной навигации
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="pageInfo"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public static MvcHtmlString PageLinks(this AjaxHelper helper, PageInfo pageInfo, DirectionMode mode)
        {
            var ajax = new AjaxOptions
            {
                UpdateTargetId = "songsList"
            };

            var stringBilder = new StringBuilder();

            var tag = new TagBuilder("div");
            tag.SetInnerText("●");
            tag.AddCssClass("btn_link_page_Stop");

            switch (mode)
            {
                case DirectionMode.Left:
                    //Проверка левой кнопки на выход за паределы
                    if (pageInfo.PageNumber > 1)
                        stringBilder.Append(helper.ActionLink("●", "GetSongs", new { page = pageInfo.PageNumber + mode },
                            ajax, new { @class = "btn__link" }));
                    else
                        stringBilder.Append(tag.ToString());
                    break;
                case DirectionMode.Right:
                    //Проверка правой кнопки на выход за паределы 
                    if (pageInfo.PageNumber < pageInfo.TotalPage)
                        stringBilder.Append(helper.ActionLink("●", "GetSongs", new { page = pageInfo.PageNumber + mode },
                            ajax, new { @class = "btn__link" }));
                    else
                        stringBilder.Append(tag.ToString());
                    break;
            }

            return MvcHtmlString.Create(stringBilder.ToString());
        }
    }
}