using AutoMapper;
using Moq;
using Ploeh.AutoFixture.Xunit2;
using System.Collections.Generic;
using System.Linq;
using Tests.Core;
using Warehouse.Admin.Frontend.Controllers;
using Warehouse.Data;
using Xunit;
using FrontModel = Warehouse.Admin.Frontend.Models;

namespace Warehouse.Tests
{
	public class GeneralTests
	{
		[Theory, AutoMoqData]
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
			var model = result.Model as IEnumerable<Article>;

			// assert:
			Assert.NotNull(model);
			Assert.NotEmpty(model);
			Assert.Equal(articles.Count, model.Count());
			Assert.Empty(result.ViewName);
		}

		[Theory, AutoMoqData]
		public void CreateNewArticleTest(
			[Frozen] Mock<IRepository<Article>> repo,
			[Frozen] Mock<IUnitOfWork> unitOfWork,
			FrontModel.Article article,
			HomeController sut)
		{
			// arrange:
			unitOfWork.Setup(x => x.ArticlesRepository).Returns(repo.Object);

			// act:
			var result = sut.CreateArticle(article);

			// assert:
			unitOfWork.Verify(x => x.ArticlesRepository, Times.Once());
			repo.Verify(x => x.Add(It.IsAny<Article>()), Times.Once());
			unitOfWork.Verify(x => x.SaveChanges(), Times.Once());

			Assert.Equal("ArticleCreated", result.ViewName);
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
		public void SaveEditedArticle(
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
			unitOfWork.Verify(x => x.ArticlesRepository, Times.Once());
			repo.Verify(x => x.Attach(It.IsAny<Article>()), Times.Once());
			unitOfWork.Verify(x => x.SaveChanges(), Times.Once());

			Assert.Empty(result.ViewName);
		}

		public void RemoveArticleById()
		{

		}

		public void SetArticlePrice()
		{

		}

		// uzupełnienie artykułów
		public void ReplenishArticles()
		{

		}

		public void FilterArticlesByName()
		{

		}

		public void FilterArticlesByCode()
		{

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
