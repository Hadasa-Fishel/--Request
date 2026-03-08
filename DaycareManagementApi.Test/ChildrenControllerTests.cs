
using Daycare.Core.Interfaces; 
using Daycare.Data;
using Daycare.Service;         
using DaycareManagementApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace DaycareManagementApi.Test
{
    public class ChildrenControllerTests
    {
        private readonly ChildrenController _controller;
        private readonly IDataContext _fakeContext;
        private readonly IChildService _childService; 

        public ChildrenControllerTests()
        {
            _fakeContext = new FakeContext();

            _childService = new ChildService(_fakeContext);

            _controller = new ChildrenController(_childService);
        }

        [Fact]
        public void Get_ReturnsOk()
        {
            var result = _controller.GetChildren();

            Assert.IsType<OkObjectResult>(result.Result);
        }


        [Fact]
        public void GetById_ExistingId_ReturnsOk()
        {
          
            var existingId = 1; 

            var result = _controller.GetChild(existingId);

            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public void GetById_NonExistingId_ReturnsNotFound()
        {
            
            var nonExistingId = 999;

            var result = _controller.GetChild(nonExistingId);

            Assert.IsType<NotFoundObjectResult>(result.Result);
        }
    }
}