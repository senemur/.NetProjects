using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using UdemyKitapSitesi.Models;
using UdemyKitapSitesi.Utility;

namespace UdemyKitapSitesi.Controllers
{
    
    public class KitapController : Controller
    {
        private readonly IKitapRepository _kitapRepository;
        private readonly IKitapTuruRepository _kitapTuruRepository;
        public readonly IWebHostEnvironment _webHostEnvironment;

        public int Id { get; private set; }


        //public object?[]? Id { get; private set; }

        public KitapController(IKitapRepository kitapRepository,IKitapTuruRepository kitapTuruRepository, IWebHostEnvironment webHostEnvironment)
        {
            _kitapRepository = kitapRepository;
            _kitapTuruRepository = kitapTuruRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        [Authorize(Roles = "Admin,Ogrenci")]
        public IActionResult Index()
        {
            //List<Kitap> objKitapList = _kitapRepository.GetAll().ToList();
            List<Kitap> objKitapList = _kitapRepository.GetAll(includeProps:"KitapTuru").ToList();
           

            return View(objKitapList);
        }



        
        //BURADA ADMİNE YETKİLENDİRME YAPILDI SADECE. YANİ SADECE ADMİN GÖREBİLİR BU SAYFALARI
        [Authorize(Roles = UserRoles.Role_Admin)]
        public IActionResult EkleGuncelle(int? id)
        {

            IEnumerable<SelectListItem> KitapTuruList = _kitapTuruRepository.GetAll().Select(k => new SelectListItem
            {
                Text = k.Name,
                Value = k.Id.ToString()
            });

            ViewBag.KitapTuruList= KitapTuruList;


            if(id==null || id == 0)
            {
                //ekle
                return View();
            }
            else
            {
                //guncelle
                Kitap? kitapVt = _kitapRepository.Get(u => u.Id == id); //Expression<Func<T, bool>> filtre

                if (kitapVt == null)
                { return NotFound(); }

                return View(kitapVt);
            }

            
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.Role_Admin)]
        public IActionResult EkleGuncelle(Kitap kitap, IFormFile? file )
        {
            //VALIDATION
            if(ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string kitapPath = Path.Combine(wwwRootPath, @"img");

                //if resim dosyası seçilmediği zaman error vermemesi için yapıldı.
                if ( file != null ) { 
                using (var fileStream = new FileStream(Path.Combine(kitapPath, file.FileName), FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
                kitap.ResimUrl = @"\img\" + file.FileName;
                }

                if (kitap.Id == 0)
                {
                    _kitapRepository.Ekle(kitap);
                    TempData["basarili"] = "Yeni kitap başarıyla oluşturuldu!";
                }
                else
                {
                    _kitapRepository.Guncelle(kitap);
                    TempData["basarili"] = "Kitap başarıyla güncellendi!";
                }

               
                _kitapRepository.Kaydet();
                 return RedirectToAction("Index","Kitap");
            }
            return View();
        }





        //public IActionResult Guncelle(int? Id)
        //{
        //    if(Id== null || Id==0 )
        //    { 
        //    return NotFound();
        //    }

        //    Kitap? kitapVt = _kitapRepository.Get(u=>u.Id==Id); //Expression<Func<T, bool>> filtre

        //    if (kitapVt == null) 
        //    { return NotFound(); }

        //    return View(kitapVt);
        //}


        //[HttpPost]
        //public IActionResult Guncelle(Kitap kitap)
        //{
        //    //VALIDATION
        //    if(ModelState.IsValid)
        //    {
        //        _kitapRepository.Guncelle(kitap);
        //        _kitapRepository.Kaydet();
        //        TempData["basarili"] = "Kitap başarıyla güncellendi!";
        //        return RedirectToAction("Index","Kitap");
        //    }
        //    return View();
        //}





        [Authorize(Roles = UserRoles.Role_Admin)]
        public IActionResult Sil(int? Id)
        {
            if(Id== null || Id==0 )
            { 
            return NotFound();
            }

            Kitap? kitapVt = _kitapRepository.Get(u => u.Id == Id);

            if (kitapVt == null) 
            { return NotFound(); }
            
            return View(kitapVt);
        }

        [HttpPost, ActionName("Sil")]
        [Authorize(Roles = UserRoles.Role_Admin)]
        public IActionResult SilPost(int? Id)
        {
            Kitap? kitap = _kitapRepository.Get(u => u.Id == Id);
            if (kitap == null)
            {
                return NotFound();
            }
            _kitapRepository.Sil(kitap);
            _kitapRepository.Kaydet();
            TempData["basarili"] = "Kayıt silme işlemi başarılı!";
            return RedirectToAction("Index", "Kitap");
        }







    }
}
