using Microsoft.AspNet.Identity.Owin;
using NyimboProject.Models;
using NyimboProject.Models.Authentication;
using NyimboProject.Models.PageNavigation;
using NyimboProject.Models.UploadedModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace NyimboProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly string _DefaultImage = "/Content/img/DefaultImage.jpg";

        private static string _Filter;

        private ApplicationDBContext _Context
        {
            get
            {
                return HttpContext.GetOwinContext().Get<ApplicationDBContext>();
            }
        }

        private ApplicationUserMenager _UserMenager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationUserMenager>();
            }
        }

        public async Task<ActionResult> Index()
        {
            _Filter = null; ///Сброс фильтра поиска

            ViewBag.FirstSong = await _Context.Songs.FirstOrDefaultAsync() ?? new Song() { Name = "Музыка еще не загружена"};

            return View();
        }

        /// Получение всеx объекты из базе и передача их в представление 
        public ActionResult GetSongs(int page = 1, string filter = null)
        {
            /// Проверка на то, был ли передан параметр
            if (filter != null)
                _Filter = filter;

            /// Фильтр не равен Null в том случае если пользоваель ввел данные для поиска
            if (_Filter != null)
                /// Поиск вхождений и возврат частичного представления
                return PartialView(CreatePages(_Context.Songs
                    .Where(s => s.Name.Contains(_Filter) || s.Performer.Contains(_Filter)).ToList(), page));

            /// Возврат частичного представления
            return PartialView(CreatePages(_Context.Songs, page));
        }

        /// <summary>
        /// Создает стриницы
        /// </summary>
        /// <param name="songs">Список всех оъектов</param>
        /// <param name="page">Номер страницы</param>
        /// <param name="pageSize">Кол-во объектов на странице</param>
        /// <returns>Список объектов на указанной странице</returns>
        private IndexViewModel CreatePages(IEnumerable<Song> songs, int page, int pageSize = 6)
        {
            return new IndexViewModel() // Общая модель
            {
                /// Информация о страницах
                PageInfo = new PageInfo()
                {
                    PageNumber = page,
                    PageSize = pageSize,
                    TotalItems = songs.Count()
                },

                /// Получение песен соответствующих выбранной странице
                Songs = songs
                 .OrderBy(s => s.Id)
                 .Skip((page - 1) * pageSize)
                 .Take(pageSize)
                 .ToList()
            };
        }

        [Authorize]
        public ActionResult Uploaded()
        {
            return View();
        }

        ///Загрузка музыки на сервер
        [HttpPost, Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Uploaded(UploadedModelView uploaded, string nameSong, string performer)
        {
            string pathToMusic = "/Content/Music/";
            string pathToImage = "/Content/UploadImage/";

            if (uploaded.Image != null && !(ModelState.IsValidField("Image"))) //!!!!
            {
                var Errors = ModelState.Values.SelectMany(m => m.Errors).ToList();

                foreach (var error in Errors)
                    ModelState.AddModelError("", error.ErrorMessage);

                return View();
            }

            if (ModelState.IsValidField("Music"))
            {
                var imgPath = SaveFile(uploaded.Image.File, pathToImage);

                _Context.Songs.Add(new Song
                {
                    Name = nameSong,
                    Performer = performer,
                    SongPaht = SaveFile(uploaded.Music.File, pathToMusic),
                    ImgPaht = string.IsNullOrEmpty(imgPath) ? _DefaultImage : imgPath, // Поставить по умолчанию!!!!!!
                    User = await _UserMenager.FindByEmailAsync(User.Identity.Name)
                });

                _Context.SaveChanges();

                return RedirectToAction("Index");
            }

            var allErrors = ModelState.Values.SelectMany(m => m.Errors).ToList();

            foreach (var error in allErrors)
                ModelState.AddModelError("", error.ErrorMessage);

            return View();
        }

        /// Модальное Окно
        public ActionResult SetImage()
        {
            return PartialView("SetImage");
        }

        ///Загрузка новой картинки
        [HttpPost, Authorize]
        public async Task<ActionResult> SetImage(UploadedImage uploadepFile, int id)
        {
            string pathToImage = "/Content/UploadImage/";
            var song = await _Context.Songs.Where(s => s.Id == id).FirstAsync();

            DeleteFile(song.ImgPaht); /// Удалять старую картинку

            if (ModelState.IsValid)
                song.ImgPaht = SaveFile(uploadepFile.File, pathToImage);

            song.ImgPaht = _DefaultImage;

            _Context.SaveChanges();

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Загружает файл в указанную дирикторию
        /// </summary>
        /// <param name="file">Загруженный с клиента файл</param>
        /// <param name="pathToSave">Путь по которому будет сохранён файл</param>
        /// <returns>Путь к сохранённому файлу</returns>
        private string SaveFile(HttpPostedFileBase file, string pathToSave)
        {
            if (file != null)
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + pathToSave;
                string newFileName = $"{file.GetHashCode()}_{Path.GetFileName(file.FileName)}";

                if (newFileName != null)
                {
                    file.SaveAs(Path.Combine(path, newFileName));

                    return pathToSave + newFileName;
                }
            }

            return String.Empty;
        }

        /// <summary>
        /// Удаляет файл
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        private void DeleteFile(string path)
        {
            try
            {
                /// Проверна для избежания удаления стандартной картинки
                if (path != _DefaultImage)
                    System.IO.File.Delete(Server.MapPath(path)); /// Удаление старой картинки
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public ActionResult Download(int id)
        {
            try
            {
                /// Получение данных о файле по указанному id
                var file = _Context.Songs
                    .Where(i => i.Id == id)
                    .Select(i => new { i.SongPaht, i.Name, i.Performer })
                    .First();

                /// Отправка файла на клиент
                return File(Server.MapPath(file.SongPaht),
                 System.Net.Mime.MediaTypeNames.Application.Octet, $"{file.Performer}_{file.Name}_(Nyimbo).mp3");
            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound); // Заменить на ModelError!
            }
        }
    }
}