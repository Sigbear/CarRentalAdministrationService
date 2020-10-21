using CarRentalAdministrationService.Controllers;
using CarRentalAdministrationService.Dto;
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
            var order = new CreateOrderDto
            {
                CarCategory = "Premium",
                Created = DateTime.Today,
                CustomerDateOfBirth = DateTime.Parse("1987-01-12")
            };

            var result = await _controller.PostOrder(order);
            var actual = ((ObjectResult)result.Result).StatusCode;
            Assert.AreEqual(404, actual);
        }

        [Test]
        public async Task POST_WhenCalledAndCarIsAvailable_Returns201()
        {
            var order = new CreateOrderDto
            {
                CarCategory = "Compact",
                Created = DateTime.Today,
                CustomerDateOfBirth = DateTime.Parse("1987-01-12")
            };

            var result = await _controller.PostOrder(order);
            var actual = ((ObjectResult)result.Result).StatusCode;
            Assert.AreEqual(201, actual);
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Database.EnsureDeleted();
        }
    }
}