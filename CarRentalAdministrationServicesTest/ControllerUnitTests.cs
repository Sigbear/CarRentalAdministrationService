using CarRentalAdministrationService.Controllers;
using CarRentalAdministrationService.Dto;
using CarRentalAdministrationService.Model;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace CarRentalAdministrationServicesTest
{
    public class OrdersControllerTest
    {
        OrdersController _controller;
        DataBaseContextFake _dbContext;

        [SetUp]
        public void Setup()
        {
            _dbContext = new DataBaseContextFake();
            _controller = new OrdersController(_dbContext);
        }

        [Test]
        public async Task POST_WhenCalledAnNoCarsInCategoryAvailable_Returns404()
        {
            var order = CreateOrderObject("Premium");

            var result = await _controller.PostOrder(order);
            var actual = ((ObjectResult)result.Result).StatusCode;
            Assert.AreEqual(404, actual);
        }

        [Test]
        public async Task POST_WhenCalledAndCarIsAvailable_Returns201()
        {
            var order = CreateOrderObject("Compact");

            var result = await _controller.PostOrder(order);
            var actual = ((ObjectResult)result.Result).StatusCode;
            Assert.AreEqual(201, actual);
        }

        [Test]
        public async Task CreatingAndReturningOrder_POSTandPATCHWhenCalled_Returns200()
        {
            var order = CreateOrderObject("Compact");
            var result = await _controller.PostOrder(order);
            var createdOrder = ((Order)((ObjectResult)result.Result).Value);

            var returnOrderRequest = CreateValidReturnOrderRequestDto(createdOrder);            
            var returnOrderResult = await _controller.PatchOrder(returnOrderRequest);
            var actual = ((ObjectResult)returnOrderResult.Result).StatusCode;

            Assert.AreEqual(200, actual);
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Database.EnsureDeleted();
        }


        private CreateOrderDto CreateOrderObject(string carCategory)
        {
            return new CreateOrderDto
            {
                CarCategory = carCategory,
                Created = DateTime.Today,
                CustomerDateOfBirth = DateTime.Parse("1987-01-12")
            };
        }

        private ReturnOrderDto CreateValidReturnOrderRequestDto(Order createdOrder)
        {
            return new ReturnOrderDto
            {
                BookingNr = createdOrder.OrderId,
                MileageInKm = createdOrder.Car.MileageInKilometers + 25,
                ReturnDate = createdOrder.Created.AddDays(2)
            };
        }
    }
}