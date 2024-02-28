using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using UdemyKitapSitesi.Models;
using UdemyKitapSitesi.Utility;

namespace UdemyKitapSitesi.Controllers
{
    [Authorize(Roles = UserRoles.Role_Admin)]
    public class KiralamaController : Controller
    {
        private readonly IKiralamaRepository _kiralamaRepository;
        private readonly IKitapRepository _kitapRepository;
        public readonly IWebHostEnvironment _webHostEnvironment;

        //public int Id { get; private set; }



        public KiralamaController(IKiralamaRepository kiralamaRepository,IKitapRepository kitapRepository, IWebHostEnvironment webHostEnvironment)
        {
            _kiralamaRepository = kiralamaRepository;
            _kitapRepository = kitapRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            
            List<Kiralama> objKiralamaList = _kiralamaRepository.GetAll(includeProps:"Kitap").ToList();
           

            return View(objKiralamaList);
        }




        public IActionResult EkleGuncelle(int? id)
        {

            IEnumerable<SelectListItem> KitapList = _kitapRepository.GetAll().Select(k => new SelectListItem
            {
                Text = k.KitapAdi,
                Value = k.Id.ToString()
            });

            ViewBag.KitapList= KitapList;


            if(id==null || id == 0)
            {
                //ekle
                return View();
            }
            else
            {
                //guncelle
                Kiralama? kiralamaVt = _kiralamaRepository.Get(u => u.Id == id); //Expression<Func<T, bool>> filtre

                if (kiralamaVt == null)
                { return NotFound(); }

                return View(kiralamaVt);
            }

            
        }

        [HttpPost]
        public IActionResult EkleGuncelle(Kiralama kiralama)
        {
            //VALIDATION
            if(ModelState.IsValid)
            {

                if (kiralama.Id == 0)
                {
                    _kiralamaRepository.Ekle(kiralama);
                    TempData["basarili"] = "Kiralama kaydı başarıyla oluşturuldu!";
                }
                else
                {
                    _kiralamaRepository.Guncelle(kiralama);
                    TempData["basarili"] = "Kiralama kaydı başarıyla güncellendi!";
                }

               
                _kiralamaRepository.Kaydet();
                 return RedirectToAction("Index","Kiralama");
            }
            return View();
        } 
        
        
        

        
        


        
        
        public IActionResult Sil(int? Id)
        {

            IEnumerable<SelectListItem> KitapList = _kitapRepository.GetAll().Select(k => new SelectListItem
            {
                Text = k.KitapAdi,
                Value = k.Id.ToString()
            });

            ViewBag.KitapList = KitapList;



            if (Id== null || Id==0 )
            { 
            return NotFound();
            }

            Kiralama? kiralamaVt = _kiralamaRepository.Get(u => u.Id == Id);

            if (kiralamaVt == null) 
            { return NotFound(); }
            
            return View(kiralamaVt);
        }

        [HttpPost, ActionName("Sil")]
        public IActionResult SilPost(int? Id)
        {
            Kiralama? kiralama = _kiralamaRepository.Get(u => u.Id == Id);
            if (kiralama == null)
            {
                return NotFound();
            }
            _kiralamaRepository.Sil(kiralama);
            _kiralamaRepository.Kaydet();
            TempData["basarili"] = "Kayıt silme işlemi başarılı!";
            return RedirectToAction("Index", "Kiralama");
        }







    }
}
