using AutoMapper;
using Moq;
using Ploeh.AutoFixture.Xunit2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using Tests.Core;
using Warehouse.Admin.Frontend.Controllers;
using Warehouse.Data;
using Xunit;
using FrontModel = Warehouse.Admin.Frontend.Models;

namespace Warehouse.Tests
{
	public class GeneralTests
	{
		[Theory, WarehouseAutoMoqData]
		public void ListArticlesTest(
			[Frozen] Mock<IRepository<Article>> repo,
			[Frozen] Mock<IUnitOfWork> unitOfWork,
			List<Article> articles,
			HomeController sut)
		{
			// arrange:
			unitOfWork.Setup(x => x.ArticlesRepository).Returns(repo.Object);
			repo.Setup(x => x.All()).Returns(articles.AsQueryable());

			// act:
			var result = sut.Index(null);
			var model = result.Model as FrontModel.ArticlesList;

			// assert:
			Assert.NotNull(model);
			Assert.Null(model.Filter);
			Assert.NotNull(model.Items);
			Assert.NotEmpty(model.Items);
			Assert.Equal(articles.Count, model.Items.Count());
			Assert.Empty(result.ViewName);
		}

		[Theory, WarehouseAutoMoqData]
		public void ListArticlesFilteredTest(
			[Frozen] Mock<IRepository<Article>> repo,
			[Frozen] Mock<IUnitOfWork> unitOfWork,
			List<Article> articles,
			HomeController sut)
		{
			// arrange:
			unitOfWork.Setup(x => x.ArticlesRepository).Returns(repo.Object);
			repo.Setup(x => x.All()).Returns(articles.AsQueryable());
			var article = articles[0];

			var filter = new FrontModel.ArticleFilter
			{
				Name = article.Name.Substring(0, article.Name.Length / 2),
				Code = article.Code.Substring(0, article.Code.Length / 2)
			};

			// act:
			var result = sut.Index(filter);
			var model = result.Model as FrontModel.ArticlesList;

			// assert:

			Assert.NotNull(model);
			Assert.NotNull(model.Filter);
			Assert.NotNull(model.Items);
			Assert.NotEmpty(model.Items);
			Assert.Equal(1, model.Items.Count());
			Assert.Empty(result.ViewName);

			// just mocked vm:
			//var articlevm = model.Items.FirstOrDefault(x => x.Id == article.Id);
			//Assert.NotNull(articlevm);
			//Assert.Equal(article.Name, articlevm.Name);
			//Assert.Equal(article.Code, articlevm.Code);
			//Assert.Equal(article.Price, articlevm.Price);
			//Assert.Equal(article.ExpiryDays, articlevm.ExpiryDays);

		}

		[Theory, WarehouseAutoMoqData]
		public void ListArticlesFilteredNoResultsTest(
			[Frozen] Mock<IRepository<Article>> repo,
			[Frozen] Mock<IUnitOfWork> unitOfWork,
			List<Article> articles,
			HomeController sut)
		{
			// arrange:
			unitOfWork.Setup(x => x.ArticlesRepository).Returns(repo.Object);
			repo.Setup(x => x.All()).Returns(articles.AsQueryable());

			var filter = new FrontModel.ArticleFilter
			{
				Name = articles[0].Name.Substring(0, articles[0].Name.Length / 2),
				Code = articles[1].Code.Substring(0, articles[1].Code.Length / 2)
			};

			// act:
			var result = sut.Index(filter);
			var model = result.Model as FrontModel.ArticlesList;

			// assert:
			Assert.NotNull(model);
			Assert.NotNull(model.Filter);
			Assert.NotNull(model.Items);
			Assert.Empty(model.Items);
			Assert.Empty(result.ViewName);
		}

		[Theory, AutoMoqData]
		public void SaveAddedArticleTest(
			[Frozen] Mock<IRepository<Article>> repo,
			[Frozen] Mock<IUnitOfWork> unitOfWork,
			FrontModel.Article article,
			HomeController sut)
		{
			// arrange:
			article.Id = 0;
			unitOfWork.Setup(x => x.ArticlesRepository).Returns(repo.Object);

			// act:
			var result = sut.AddArticle(article);

			// assert:
			unitOfWork.Verify(x => x.ArticlesRepository, Times.Once(), "ArticlesRepository not invoked.");
			repo.Verify(x => x.Add(It.IsAny<Article>()), Times.Once(), "Add method not invoked.");
			unitOfWork.Verify(x => x.SaveChanges(), Times.Once(), "SaveChanges method not invoked.");

			Assert.True(result.RouteValues.Count() > 0);
			Assert.Equal("Index", result.RouteValues["action"]);
		}

		[Theory, WarehouseAutoMoqData]
		public void ShowArticleToEditTest(
			[Frozen] Mock<IUnitOfWork> unitOfWork,
			[Frozen] Mock<IRepository<Article>> repo,
			Article article,
			HomeController sut)
		{
			// arrange:
			unitOfWork.Setup(x => x.ArticlesRepository).Returns(repo.Object);
			repo.Setup(x => x.FindById(It.IsAny<int>())).Returns(article);

			// act:
			var result = sut.EditArticle(article.Id);
			var model = result.Model as FrontModel.Article;

			// assert:
			Assert.NotNull(model);
			Assert.Equal(article.Id, model.Id);
			Assert.Equal(article.Code, model.Code);
			Assert.Equal(article.Name, model.Name);
			Assert.Equal(article.ExpiryDays, model.ExpiryDays);
			Assert.Equal(article.Price, model.Price);
		}

		[Theory, WarehouseAutoMoqData]
		public void SaveEditedArticleTest(
			[Frozen] Mock<IUnitOfWork> unitOfWork,
			[Frozen] Mock<IRepository<Article>> repo,
			IMapper mapper,
			FrontModel.Article articlevm,
			HomeController sut)
		{
			// arrange:
			var article = mapper.Map<Article>(articlevm);
			unitOfWork.Setup(x => x.ArticlesRepository).Returns(repo.Object);
			repo.Setup(x => x.FindById(It.IsAny<int>())).Returns(article);

			// act:
			var result = sut.EditArticle(articlevm);

			// assert:
			Assert.True(articlevm.Id > 0);
			unitOfWork.Verify(x => x.ArticlesRepository, Times.Once());
			repo.Verify(x => x.Attach(It.IsAny<Article>()), Times.Once(), "Attach method not invoked.");
			repo.Verify(x => x.Update(It.IsAny<Article>()), Times.Once(), "Update method not invoked.");
			unitOfWork.Verify(x => x.SaveChanges(), Times.Once(), "SaveChanges method not invoked.");

			Assert.True(result.RouteValues.Count() > 0);
			Assert.Equal("Index", result.RouteValues["action"]);
		}

		[Theory, WarehouseAutoMoqData]
		public void DeleteArticle(
			[Frozen] Mock<IUnitOfWork> unitOfWork,
			[Frozen] Mock<IRepository<Article>> repo,
			int articleId,
			HomeController sut)
		{
			// nie można usunąć jeśli jest w zamówieniach
			// w historii zamówień
			// lub na magazynie

			// arrange:
			unitOfWork.Setup(x => x.ArticlesRepository).Returns(repo.Object);
			//repo.Setup(x => x.FindById(It.IsAny<int>())).Returns(article);

			// act:
			var result = sut.DeleteArticle(articleId, null) as RedirectToRouteResult;

			// assert:
			Assert.NotNull(result);
			repo.Verify(x => x.Delete(It.IsAny<int>()), Times.Once(), "Delete method not invoked.");
			unitOfWork.Verify(x => x.SaveChanges(), Times.Once(), "SaveChanges method not invoked.");
		}

		[Theory, WarehouseAutoMoqData]
		public void DeleteArticleCantBecauseDevices(
			[Frozen] Mock<IUnitOfWork> unitOfWork,
			[Frozen] Mock<IRepository<Article>> repo,
			[Frozen] Mock<IRepository<Device>> repoDevices,
			List<Device> devices,
			int articleId,
			HomeController sut)
		{
			// arrange:
			devices.ForEach(x => x.ArticleId = articleId);
			repoDevices.Setup(x => x.All()).Returns(devices.AsQueryable());
			unitOfWork.Setup(x => x.ArticlesRepository).Returns(repo.Object);
			unitOfWork.Setup(x => x.DevicesRepository).Returns(repoDevices.Object);

			// act:
			var result = sut.DeleteArticle(articleId, null) as ViewResult;

			// assert:
			Assert.NotNull(result);
			Assert.NotEmpty(result.ViewName);
			Assert.NotNull(result.Model);
			Assert.Equal("CantDeleteArticle", result.ViewName);

			repo.Verify(x => x.Delete(It.IsAny<int>()), Times.Never(), "Delete method invoked.");
			unitOfWork.Verify(x => x.SaveChanges(), Times.Never(), "SaveChanges method invoked.");
		}

		[Theory, WarehouseAutoMoqData]
		public void DeleteArticleCantBecauseOrders(
			[Frozen] Mock<IUnitOfWork> unitOfWork,
			[Frozen] Mock<IRepository<Article>> repo,
			[Frozen] Mock<IRepository<Order>> repoOrders,
			List<Order> orders,
			int articleId,
			HomeController sut)
		{
			// arrange:
			orders.SelectMany(o => o.Articles).ToList().ForEach(x => x.Id = articleId);
			repoOrders.Setup(x => x.All()).Returns(orders.AsQueryable());
			unitOfWork.Setup(x => x.ArticlesRepository).Returns(repo.Object);
			unitOfWork.Setup(x => x.OrdersRepository).Returns(repoOrders.Object);

			// act:
			var result = sut.DeleteArticle(articleId, null) as ViewResult;

			// assert:
			Assert.NotNull(result);
			Assert.NotEmpty(result.ViewName);
			Assert.NotNull(result.Model);
			Assert.Equal("CantDeleteArticle", result.ViewName);

			repo.Verify(x => x.Delete(It.IsAny<int>()), Times.Never(), "Delete method invoked.");
			unitOfWork.Verify(x => x.SaveChanges(), Times.Never(), "SaveChanges method invoked.");
		}

		// uzupełnienie artykułów
		public void SaveAddDevices_ArticleExists_Test(
			IMapper mapper,
			[Frozen] Mock<IUnitOfWork> unitOfWork,
			[Frozen] Mock<IRepository<Article>> repo,
			[Frozen] Mock<IRepository<Device>> repoDevices,
			FrontModel.Device device,
			HomeController sut
			)
		{

			// arrange:
			var dbdevice = mapper.Map<Device>(device);
			repo.Setup(x => x.All().Any(It.IsAny<Expression<Func<Article, bool>>>())).Returns(true);
			unitOfWork.Setup(x => x.ArticlesRepository).Returns(repo.Object);
			unitOfWork.Setup(x => x.DevicesRepository).Returns(repoDevices.Object);

			// act:
			var result = sut.AddDevices(device);

			// assert:
			repo.Verify(x => x.All().Any(d => d.Id == device.ArticleId), Times.Once(), "Any method not invoked.");
			repoDevices.Verify(x => x.AddRange(new[] { dbdevice }), Times.Once(), "AddRange method not invoked.");
			unitOfWork.Verify(x => x.SaveChanges(), Times.Never(), "SaveChanges method invoked.");
		}



		public void OrderOneArticle()
		{

		}

		public void OrderDifferentArticles()
		{

		}

		// przetworzenie zamówienia przez administratora
		public void ProcessOrder()
		{

		}
	}
}
