using Application.Common.ViewModel;
using Application.Product.Interface;
using HMProductAPI.Controllers;
using HMProductAPITest.Data;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace HMProductAPITest
{
    public class ProductControllerTest
    {
        private readonly Mock<IProductCRUDOperations> _productCrudOp;
        public ProductControllerTest(Mock<IProductCRUDOperations> productCrudOp)
        {
            _productCrudOp = productCrudOp;
        }
        [Fact]
        public void GetProduct()
        {
            //arrange
            Guid id = new Guid("11223344-5566-7788-99AA-BBCCDDEEFF00");
            var product = GetProductsData();
            _productCrudOp.Setup(x => x.GetProduct(id))
                .Returns(product);
            var productsController = new ProductsController(_productCrudOp.Object);

            //act
            var productResult = productsController.GetProduct(id);

            //assert
            Assert.NotNull(productResult);
            Assert.Equal(GetProductsData().ToString(), productResult.ToString());
            Assert.True(product.Equals(productResult));
        }

        [Fact]
        public void AddProduct() //Need work
        {
            //arrange
            Guid id = new Guid("11223344-5566-7788-99AA-BBCCDDEEFF01");
            var product = getStaticProduct();
            product.productID = id;
            _productCrudOp.Setup(x => x.AddProduct(product))
                .Returns(HttpStatusCode.OK);
            var productsController = new ProductsController(_productCrudOp.Object);

            //act
            var productCreation = productsController.CreateProduct(getStaticProduct());
            var productResult = productsController.GetProduct(id);
            string jsonData = JsonConvert.SerializeObject(product);

            //assert
            Assert.NotNull(productResult);
            Assert.Equal(jsonData, productResult.ToString());
            Assert.True(jsonData.Equals(productResult));
        }

        private ProductVM getStaticProduct()
        {
            var data = new ProductVM();
            var path = Constants.filePathAdd;
            using (StreamReader r = new StreamReader(path))
            {
                string jsonData = r.ReadToEnd();
                data = JsonConvert.DeserializeObject<ProductVM>(jsonData);
            }
            return data;
        }

        private string GetProductsData()
        {
            string jsonData = String.Empty;
            var path = Constants.filePath;
            using (StreamReader r = new StreamReader(path))
            {
                jsonData = r.ReadToEnd();
            }
            return jsonData;
        }
    }
}
