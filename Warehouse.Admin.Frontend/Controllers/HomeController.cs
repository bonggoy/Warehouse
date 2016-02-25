using AutoMapper;
using System.Linq;
using System.Web.Mvc;
using Warehouse.Admin.Frontend.Models;

namespace Warehouse.Admin.Frontend.Controllers
{
	public class HomeController : Controller
	{
		private Data.IUnitOfWork _unitOfWork;
		private IMapper _mapper;

		public HomeController(Data.IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;

		}

		public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}

		// [OutputCache(VaryByParam = "filter")]
		public ViewResult Index(ArticleFilter filter)
		{
			var repo = _unitOfWork.ArticlesRepository;
			IQueryable<Data.Article> query = _unitOfWork.ArticlesRepository.All();
			if (filter != null)
			{
				if (!string.IsNullOrWhiteSpace(filter.Code))
				{
					var a = query.ToList();
					query = query.Where(x => x.Code.StartsWith(filter.Code));
				}

				if (!string.IsNullOrWhiteSpace(filter.Name))
				{
					query = query.Where(x => x.Name.StartsWith(filter.Name));
				}
			}

			var model = new ArticlesList
			{
				Items = query.ToList()
					.Select(x => _mapper.Map<Article>(x))
					.ToList(),
				Filter = filter
			};

			return View(model);
		}

		private void AddOrEditArticle(Article item)
		{
			var article = _mapper.Map<Data.Article>(item);
			var repo = _unitOfWork.ArticlesRepository;
			if (item.Id == 0)
			{
				repo.Add(article);
				item.Id = article.Id;
			}
			else
			{
				repo.Attach(article);
				repo.Update(article);
			}

			_unitOfWork.SaveChanges();
		}

		public ViewResult AddArticle()
		{
			return View("EditArticle");
		}

		[HttpPost]
		public ViewResult AddArticle(Article item)
		{
			AddOrEditArticle(item);
			return View("ArticleSaved", item);
		}

		public ViewResult EditArticle(int id)
		{
			var repo = _unitOfWork.ArticlesRepository;
			var article = repo.FindById(id);
			var model = _mapper.Map<Article>(article);

			return View(model);
		}

		[HttpPost]
		public ViewResult EditArticle(Article item)
		{
			AddOrEditArticle(item);
			return View("ArticleSaved", item);
		}
	}
}