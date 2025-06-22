using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using WebApplication1.Controllers;
using WebApplication1.Repository.Models;
using WebApplication1.Repository.Services.Contracts;

[TestFixture]
public class PaymentsControllerTests
{
    private Mock<IPaymentService> _serviceMock;
    private PaymentsController _controller;

    [SetUp]
    public void Setup()
    {
        _serviceMock = new Mock<IPaymentService>();
        _controller = new PaymentsController(_serviceMock.Object);
    }

    [Test]
    public async Task GetAll_ReturnsOk()
    {
        _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<Payment>());
        var result = await _controller.GetAll();
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
    }

    [Test]
    public async Task GetById_ReturnsOk_WhenFound()
    {
        var payment = new Payment { PaymentId = 1, Amount = 100, TransactionId = 1 };
        _serviceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(payment);

        var result = await _controller.GetById(1);

        Assert.That(result, Is.InstanceOf<OkObjectResult>());
    }

    [Test]
    public async Task GetById_ReturnsNotFound_WhenNotFound()
    {
        _serviceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync((Payment?)null);

        var result = await _controller.GetById(1);

        Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
    }

    [Test]
    public async Task Create_ReturnsCreated_WhenValid()
    {
        var payment = new Payment { PaymentId = 1, Amount = 100, TransactionId = 1 };
        _serviceMock.Setup(s => s.CreateAsync(payment)).ReturnsAsync(payment);

        var result = await _controller.Create(payment);

        Assert.That(result, Is.InstanceOf<CreatedAtActionResult>());
    }

    [Test]
    public async Task Create_ReturnsValidationProblem_WhenInvalid()
    {
        _controller.ModelState.AddModelError("Amount", "Required");
        var payment = new Payment();

        var result = await _controller.Create(payment);

        Assert.That(result, Is.InstanceOf<ObjectResult>());
        var objectResult = result as ObjectResult;
        Assert.That(objectResult?.StatusCode, Is.EqualTo(400));
    }

    [Test]
    public async Task Update_ReturnsNoContent_WhenValid()
    {
        var payment = new Payment { PaymentId = 1, Amount = 100, TransactionId = 1 };
        _serviceMock.Setup(s => s.UpdateAsync(payment)).ReturnsAsync(true);

        var result = await _controller.Update(1, payment);

        Assert.That(result, Is.InstanceOf<NoContentResult>());
    }

    [Test]
    public async Task Update_ReturnsBadRequest_WhenIdMismatch()
    {
        var payment = new Payment { PaymentId = 2, Amount = 100, TransactionId = 1 };

        var result = await _controller.Update(1, payment);

        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
    }

    [Test]
    public async Task Update_ReturnsValidationProblem_WhenInvalid()
    {
        _controller.ModelState.AddModelError("Amount", "Required");
        var payment = new Payment { PaymentId = 1 };

        var result = await _controller.Update(1, payment);

        Assert.That(result, Is.InstanceOf<ObjectResult>());
        var objectResult = result as ObjectResult;
        Assert.That(objectResult?.StatusCode, Is.EqualTo(400));
    }

    [Test]
    public async Task Update_ReturnsNotFound_WhenNotFound()
    {
        var payment = new Payment { PaymentId = 1, Amount = 100, TransactionId = 1 };
        _serviceMock.Setup(s => s.UpdateAsync(payment)).ReturnsAsync(false);

        var result = await _controller.Update(1, payment);

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