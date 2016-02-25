using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

		// [OutputCache(VaryByParam = "filter")]
		public ViewResult Index(ArticleFilter filter)
		{
			var repo = _unitOfWork.ArticlesRepository;
			IQueryable<Data.Article> query = null;
			if (filter != null)
			{
				Expression<Func<Data.Article, bool>> where = x => x.Code.StartsWith(filter.Code, StringComparison.InvariantCultureIgnoreCase);

				if (!string.IsNullOrWhiteSpace(filter.Code))
				{
					query = repo.Where(x => x.Code.StartsWith(filter.Code, StringComparison.InvariantCultureIgnoreCase));
				}

				if (!string.IsNullOrWhiteSpace(filter.Name))
				{
					query = query == null
						? repo.Where(x => string.Equals(filter.Name, x.Name, StringComparison.InvariantCultureIgnoreCase))
						: query.Where(x => string.Equals(filter.Name, x.Name, StringComparison.InvariantCultureIgnoreCase));
				}
			}

			var model = new ArticlesList
			{
				Items = _mapper.Map<List<Article>>(query.ToList()),
				Filter = filter
			};

			return View(model);
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

		public ViewResult CreateArticle()
		{		
			return View(new Article());
		}

		[HttpPost]
		public ViewResult CreateArticle(Article item)
		{
			var article = _mapper.Map<Data.Article>(item);
			var repo = _unitOfWork.ArticlesRepository;
			repo.Add(article);
			item.Id = article.Id;
			_unitOfWork.SaveChanges();

			return View("ArticleCreated", item);
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
			var repo = _unitOfWork.ArticlesRepository;
			var db = _mapper.Map<Data.Article>(item);
			repo.Attach(db);
			_unitOfWork.SaveChanges();

			return View(item);
		}
	}
}