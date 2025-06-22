using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Moq;
using NUnit.Framework;
using WebApplication1.Common.DTO;
using WebApplication1.Controllers;
using WebApplication1.Repository.Services.Contracts;

[TestFixture]
public class TransactionsControllerTests
{
    private Mock<ITransactionService> _serviceMock;
    private TransactionsController _controller;

    [SetUp]
    public void Setup()
    {
        _serviceMock = new Mock<ITransactionService>();
        _controller = new TransactionsController(_serviceMock.Object);
    }

    [Test]
    public async Task Create_ReturnsCreated_WhenValid()
    {
        var dto = new NewTransactionDTO
        {
            CustomerId = 1,
            Articles = new List<ArticleDTO> { new ArticleDTO(1, 2) },
            Payments = new List<PaymentDTO> { new PaymentDTO { Amount = 10 } }
        };
        var created = new TransactionDTO
        {
            Id = 1,
            CustomerId = dto.CustomerId,
            Date = DateTime.UtcNow,
            Articles = dto.Articles,
            Payments = new List<PaymentSummaryDTO> { new PaymentSummaryDTO(1, 100, DateTime.UtcNow) }
        };
        _serviceMock.Setup(s => s.CreateAsync(dto)).ReturnsAsync(created);

        var result = await _controller.Create(dto);

        Assert.That(result, Is.InstanceOf<CreatedAtActionResult>());
    }

    [Test]
    public async Task Create_ReturnsValidationProblem_WhenInvalid()
    {
        _controller.ModelState.AddModelError("CustomerId", "Required");
        var dto = new NewTransactionDTO
        {
            Articles = new List<ArticleDTO> { new ArticleDTO(1, 2) },
            Payments = new List<PaymentDTO> { new PaymentDTO { Amount = 10 } }
        };

        var result = await _controller.Create(dto);

        Assert.That(result, Is.InstanceOf<ObjectResult>());
        var objectResult = result as ObjectResult;
        Assert.That(objectResult?.StatusCode, Is.EqualTo(400));
    }

    [Test]
    public async Task Create_ReturnsNotFound_WhenArgumentException()
    {
        var dto = new NewTransactionDTO
        {
            CustomerId = 1,
            Articles = new List<ArticleDTO> { new ArticleDTO(1, 2) },
            Payments = new List<PaymentDTO> { new PaymentDTO { Amount = 10 } }
        };
        _serviceMock.Setup(s => s.CreateAsync(dto)).ThrowsAsync(new ArgumentException("Customer not found"));

        var result = await _controller.Create(dto);

        Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
    }

    [Test]
    public async Task Create_ReturnsBadRequest_WhenInvalidOperationException()
    {
        var dto = new NewTransactionDTO
        {
            CustomerId = 1,
            Articles = new List<ArticleDTO> { new ArticleDTO(1, 2) },
            Payments = new List<PaymentDTO> { new PaymentDTO { Amount = 10 } }
        };
        _serviceMock.Setup(s => s.CreateAsync(dto)).ThrowsAsync(new InvalidOperationException("Not enough inventory"));

        var result = await _controller.Create(dto);

        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
    }

    [Test]
    public async Task Create_ReturnsInternalServerError_WhenException()
    {
        var dto = new NewTransactionDTO
        {
            CustomerId = 1,
            Articles = new List<ArticleDTO> { new ArticleDTO(1, 2) },
            Payments = new List<PaymentDTO> { new PaymentDTO { Amount=10} }
        };
        _serviceMock.Setup(s => s.CreateAsync(dto)).ThrowsAsync(new Exception("Unexpected"));

        var result = await _controller.Create(dto);

        Assert.That(result, Is.InstanceOf<ObjectResult>());
        var objectResult = result as ObjectResult;
        Assert.That(objectResult?.StatusCode, Is.EqualTo(500));
    }
}