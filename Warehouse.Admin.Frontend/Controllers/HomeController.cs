using AutoMapper;
using System;
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
			var query = _unitOfWork.ArticlesRepository
				.All()
				.Select(x => new { Article = x, DevicesCount = x.Devices.Count });

			if (filter != null)
			{
				if (!string.IsNullOrWhiteSpace(filter.Code))
				{
					query = query.Where(x => x.Article.Code.StartsWith(filter.Code));
				}

				if (!string.IsNullOrWhiteSpace(filter.Name))
				{
					query = query.Where(x => x.Article.Name.StartsWith(filter.Name));
				}
			}

			var model = new ArticlesList
			{
				Items = query.ToList()
					.Select(x =>
					{
						// set devices count to use in mapper
						x.Article.DevicesCount = x.DevicesCount;
						return _mapper.Map<Article>(x.Article);
					})
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
		public RedirectToRouteResult AddArticle(Article item)
		{
			AddOrEditArticle(item);
			return RedirectToAction("Index");
		}

		public ViewResult EditArticle(int id)
		{
			var repo = _unitOfWork.ArticlesRepository;
			var article = repo.FindById(id);
			var model = _mapper.Map<Article>(article);

			return View(model);
		}

		[HttpPost]
		public RedirectToRouteResult EditArticle(Article item)
		{
			AddOrEditArticle(item);
			return RedirectToAction("Index");
		}

		// [HttpPost]
		public ActionResult DeleteArticle(int id, ArticleFilter filter)
		{
			var repo = _unitOfWork.ArticlesRepository;

			var hasDevices = _unitOfWork.DevicesRepository.All().Any(x => x.ArticleId == id);
			var hasOrders = _unitOfWork.OrdersRepository.All().Any(x => x.Articles.Any(a => a.Id == id));

			if (!hasDevices && !hasOrders)
			{
				repo.Delete(id);
				_unitOfWork.SaveChanges();
				return RedirectToAction("Index", new { filter });
			}

			ModelState.AddModelError("", "Can't delete article because it has devices or orders.");
			return View("CantDeleteArticle");
		}

		public ViewResult DevicesList(int articleId)
		{
			var article = _unitOfWork.ArticlesRepository
				.Include(x => x.Devices)
				.FirstOrDefault(x => x.Id == articleId);

			var model = _mapper.Map<Article>(article);
			return View(model);

		}

		public ViewResult AddDevices(int articleId)
		{
			return View(new Device
			{
				ArticleId = articleId
			});
		}

		[HttpPost]
		public ActionResult AddDevices(int articleId, string devices)
		{

			if (_unitOfWork.ArticlesRepository.All().Any(x => x.Id == articleId))
			{
				var devicesToAdd = devices
					.Split(new[] { ';', ',', ' ' }, StringSplitOptions.RemoveEmptyEntries)
					.Select(x => new Data.Device
					{
						ArticleId = articleId,
						Code = x
					});
				_unitOfWork.DevicesRepository.AddRange(devicesToAdd);
				_unitOfWork.SaveChanges();


				return RedirectToAction("DevicesList", new { articleId });
			}
			else
			{
				ModelState.AddModelError("", "Article " + articleId + " does not exist.");
				return View(articleId);
			}


		}
	}
}