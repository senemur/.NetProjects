using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using UdemyKitapSitesi.Models;
using UdemyKitapSitesi.Utility;

namespace UdemyKitapSitesi.Controllers
{
    [Authorize(Roles = UserRoles.Role_Admin)]
    public class KitapTuruController : Controller
    {
        private readonly IKitapTuruRepository _kitapTuruRepository;

        public object?[]? Id { get; private set; }

        public KitapTuruController(IKitapTuruRepository context)
        {
            _kitapTuruRepository = context;
        }

        public IActionResult Index()
        {
            List<KitapTuru> objKitapTuruList = _kitapTuruRepository.GetAll().ToList();
            return View(objKitapTuruList);
        }




        public IActionResult Ekle()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Ekle(KitapTuru kitapTuru)
        {
            //VALIDATION
            if(ModelState.IsValid)
            {
                _kitapTuruRepository.Ekle(kitapTuru);
                _kitapTuruRepository.Kaydet();
                TempData["basarili"] = "Yeni kitap türü başarıyla oluşturuldu!";
            return RedirectToAction("Index","KitapTuru");
            }
            return View();
        } 
        
        
        
        
        
        public IActionResult Guncelle(int? Id)
        {
            if(Id== null || Id==0 )
            { 
            return NotFound();
            }

            KitapTuru? kitapTuruVt = _kitapTuruRepository.Get(u=>u.Id==Id); //Expression<Func<T, bool>> filtre

            if (kitapTuruVt == null) 
            { return NotFound(); }
            
            return View(kitapTuruVt);
        }

        [HttpPost]
        public IActionResult Guncelle(KitapTuru kitapTuru)
        {
            //VALIDATION
            if(ModelState.IsValid)
            {
                _kitapTuruRepository.Guncelle(kitapTuru);
                _kitapTuruRepository.Kaydet();
                TempData["basarili"] = "Kitap türü başarıyla güncellendi!";
                return RedirectToAction("Index","KitapTuru");
            }
            return View();
        }
        
        


        
        
        public IActionResult Sil(int? Id)
        {
            if(Id== null || Id==0 )
            { 
            return NotFound();
            }

            KitapTuru? kitapTuruVt = _kitapTuruRepository.Get(u => u.Id == Id);

            if (kitapTuruVt == null) 
            { return NotFound(); }
            
            return View(kitapTuruVt);
        }

        [HttpPost, ActionName("Sil")]
        public IActionResult SilPost(int? Id)
        {
            KitapTuru? kitapTuru = _kitapTuruRepository.Get(u => u.Id == Id);
            if (kitapTuru == null)
            {
                return NotFound();
            }
            _kitapTuruRepository.Sil(kitapTuru);
            _kitapTuruRepository.Kaydet();
            TempData["basarili"] = "Kayıt silme işlemi başarılı!";
            return RedirectToAction("Index", "KitapTuru");
        }







    }
}
