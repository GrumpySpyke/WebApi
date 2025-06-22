using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Moq;
using NUnit.Framework;
using WebApplication1.Common.DTO;
using WebApplication1.Controllers;
using WebApplication1.Repository.Models;
using WebApplication1.Repository.Services.Contracts;

[TestFixture]
public class CustomersControllerTests
{
    private Mock<ICustomerService> _serviceMock;
    private CustomersController _controller;

    [SetUp]
    public void Setup()
    {
        _serviceMock = new Mock<ICustomerService>();
        _controller = new CustomersController(_serviceMock.Object);
    }

    [Test]
    public async Task Create_ReturnsCreated_WhenValid()
    {
        var customer = new Customer { CustomerId = 1, FirstName = "test", LastName = "test", Email = "abc@abc.com", Phone = "1234567890" };
        _serviceMock.Setup(s => s.CreateAsync(customer)).ReturnsAsync(customer);

        var result = await _controller.Create(customer);

        Assert.That(result, Is.InstanceOf<CreatedAtActionResult>());
    }

    [Test]
    public async Task Create_ReturnsValidationProblem_WhenInvalid()
    {
        _controller.ModelState.AddModelError("FirstName", "Required");
        var customer = new Customer();

        var result = await _controller.Create(customer);

        Assert.That(result, Is.InstanceOf<ObjectResult>());
        var objectResult = result as ObjectResult;
        Assert.That(objectResult?.StatusCode, Is.EqualTo(400));
    }

    [Test]
    public async Task Update_ReturnsBadRequest_WhenIdMismatch()
    {
        var customer = new Customer { CustomerId = 2 };
        var result = await _controller.Update(1, customer);

        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
    }

    [Test]
    public async Task Delete_ReturnsNotFound_WhenNotFound()
    {
        _serviceMock.Setup(s => s.DeleteAsync(1)).ReturnsAsync(false);

        var result = await _controller.Delete(1);

        Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
    }
}