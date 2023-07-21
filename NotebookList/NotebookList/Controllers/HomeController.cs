using Microsoft.AspNetCore.Mvc;
using NotebookList.Models;
using System.Diagnostics;

namespace NotebookList.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly TestBananaContext _bananaContext;
        
        public HomeController(ILogger<HomeController> logger, TestBananaContext bananaContext)
        {
            _logger = logger;
            _bananaContext = bananaContext;

        }

        public IActionResult Index()
        {
            var Apple = from xxx in _bananaContext.Notebooks
                        select new TestBananaContext { 
                            NBT = xxx.NotebookTitle,
                            NBCT = xxx.NotebookContent,
                        };
            return View(Apple.ToList());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult NBPage(int? SortNum)
        {

            var NB_Bag = from Howhow in _bananaContext.Notebooks
                         join Diedie in _bananaContext.Members on Howhow.MemberId equals Diedie.MemberId
                         where Howhow.ProgramId == 1
                         select new TestBananaContext
                         {
                             NBT = Howhow.NotebookTitle,
                             NBCT = Howhow.NotebooAddDate.ToString(),
                             NBImg = Diedie.MemberPhoto
                         };
            switch (SortNum)
            {
                case 1:
                    NB_Bag = NB_Bag.OrderByDescending(x => x.NBCT);
                    break;

                case 2:
                    NB_Bag = NB_Bag.OrderBy(x => x.NBCT);
                    break;

                default:
                    break;
            }
            //if (SerchWd != null)
            //{
            //    NB_Bag = NB_Bag.Where(item => item.NBT.Contains(SerchWd));
            //}

            return View(NB_Bag.ToList());
        }

        [HttpPost]
        public IActionResult NBPage(string SerchWd)
        {
            if (SerchWd == null)
            {
                return RedirectToAction("NBPage");
            }
            else
            {
                var NB_Bag = from Howhow in _bananaContext.Notebooks
                             join Diedie in _bananaContext.Members on Howhow.MemberId equals Diedie.MemberId
                             where Howhow.ProgramId == 1 && Howhow.NotebookTitle.Contains(SerchWd)
                             select new TestBananaContext
                             {
                                 NBT = Howhow.NotebookTitle,
                                 NBCT = Howhow.NotebooAddDate.ToString(),
                                 NBImg = Diedie.MemberPhoto
                             };

                ViewBag.SrWd = SerchWd;
                return View(NB_Bag.ToList());
            }
            
        }


        public IActionResult NoteBookEdit(string? NBTitle)
        {
            
            var EditBag = from EB in _bananaContext.Notebooks
                          where EB.NotebookTitle == NBTitle
                          select new TestBananaContext
                          {
                              NBT = EB.NotebookTitle,
                              NBCT = EB.NotebooAddDate.ToString(),
                              NBOverview = EB.NotebookContent
                          };

            return View(EditBag.ToList());
        }

        [HttpPost]
        public IActionResult NoteBookEdit(string ChangeTitle, string ChangeOverView,string NBTime)
        {
            var notebook = _bananaContext.Notebooks.FirstOrDefault(n => n.NotebooAddDate.ToString() == NBTime);
            if (notebook != null)
            {
                notebook.NotebookTitle = ChangeTitle;
                notebook.NotebookContent = ChangeOverView;
                _bananaContext.SaveChanges();  // 儲存變更
            }

            return RedirectToAction("NBPage");
        }

        public IActionResult NoteBookAdd() { 
            return View();
        }

        [HttpPost]
        public IActionResult NoteBookAdd(string TitleAdd,string ContentAdd)
        {
            
            var AddNB = new Notebook
            { 
                NotebookTitle = TitleAdd,
                NotebookContent = ContentAdd,
                NotebooAddDate = DateTime.Now,
                ProgramId = 1,
                MemberId = 3                         
            };
            _bananaContext.Notebooks.Add(AddNB);
            _bananaContext.SaveChanges();
            return RedirectToAction("NBPage");
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}