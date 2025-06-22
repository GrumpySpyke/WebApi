using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using WebApplication1.Controllers;
using WebApplication1.Repository.Models;
using WebApplication1.Repository.Services.Contracts;

[TestFixture]
public class ArticlesControllerTests
{
    private Mock<IArticleService> _serviceMock;
    private ArticlesController _controller;

    [SetUp]
    public void Setup()
    {
        _serviceMock = new Mock<IArticleService>();
        _controller = new ArticlesController(_serviceMock.Object);
    }

    [Test]
    public async Task GetAll_ReturnsOk()
    {
        _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<Article>());
        var result = await _controller.GetAll();
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
    }

    [Test]
    public async Task GetById_ReturnsOk_WhenFound()
    {
        var article = new Article { ArticleId = 1, Name = "Test", Stock = 10 };
        _serviceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(article);

        var result = await _controller.GetById(1);

        Assert.That(result, Is.InstanceOf<OkObjectResult>());
    }

    [Test]
    public async Task GetById_ReturnsNotFound_WhenNotFound()
    {
        _serviceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync((Article?)null);

        var result = await _controller.GetById(1);

        Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
    }

    [Test]
    public async Task Create_ReturnsCreated_WhenValid()
    {
        var article = new Article { ArticleId = 1, Name = "Test", Stock = 10 };
        _serviceMock.Setup(s => s.CreateAsync(article)).ReturnsAsync(article);

        var result = await _controller.Create(article);

        Assert.That(result, Is.InstanceOf<CreatedAtActionResult>());
    }

    [Test]
    public async Task Create_ReturnsValidationProblem_WhenInvalid()
    {
        _controller.ModelState.AddModelError("Name", "Required");
        var article = new Article();

        var result = await _controller.Create(article);

        Assert.That(result, Is.InstanceOf<ObjectResult>());
        var objectResult = result as ObjectResult;
        Assert.That(objectResult?.StatusCode, Is.EqualTo(400));
    }

    [Test]
    public async Task Update_ReturnsNoContent_WhenValid()
    {
        var article = new Article { ArticleId = 1, Name = "Test", Stock = 10 };
        _serviceMock.Setup(s => s.UpdateAsync(article)).ReturnsAsync(true);

        var result = await _controller.Update(1, article);

        Assert.That(result, Is.InstanceOf<NoContentResult>());
    }

    [Test]
    public async Task Update_ReturnsBadRequest_WhenIdMismatch()
    {
        var article = new Article { ArticleId = 2, Name = "Test", Stock = 10 };

        var result = await _controller.Update(1, article);

        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
    }

    [Test]
    public async Task Update_ReturnsValidationProblem_WhenInvalid()
    {
        _controller.ModelState.AddModelError("Name", "Required");
        var article = new Article { ArticleId = 1 };

        var result = await _controller.Update(1, article);

        Assert.That(result, Is.InstanceOf<ObjectResult>());
        var objectResult = result as ObjectResult;
        Assert.That(objectResult?.StatusCode, Is.EqualTo(400));
    }

    [Test]
    public async Task Update_ReturnsNotFound_WhenNotFound()
    {
        var article = new Article { ArticleId = 1, Name = "Test", Stock = 10 };
        _serviceMock.Setup(s => s.UpdateAsync(article)).ReturnsAsync(false);

        var result = await _controller.Update(1, article);

        Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
    }

    [Test]
    public async Task Delete_ReturnsNoContent_WhenDeleted()
    {
        _serviceMock.Setup(s => s.DeleteAsync(1)).ReturnsAsync(true);

        var result = await _controller.Delete(1);

        Assert.That(result, Is.InstanceOf<NoContentResult>());
    }

    [Test]
    public async Task Delete_ReturnsNotFound_WhenNotFound()
    {
        _serviceMock.Setup(s => s.DeleteAsync(1)).ReturnsAsync(false);

        var result = await _controller.Delete(1);

        Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
    }
}