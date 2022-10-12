using Application.Common.Entities;
using Application.Common.Interfaces;
using Application.Common.ViewModel;
using Application.Product.Interface;
using Domain.Master;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net;

namespace Application.Product.Service
{
    public class ProductCRUDOperations : IProductCRUDOperations
    {
        private readonly IApplicationDBContext _dbContext;
        private readonly IColourCRUDOperations _colourCRUDOperations;
        private readonly ISizeCRUDOperations _sizeCRUDOperations;
        private readonly IExternalAPIService _externalAPIService;
        private static Random random = new Random();
        private string colourAPI;
        private string sizeAPI;

        public ProductCRUDOperations(IApplicationDBContext dbContext, IColourCRUDOperations colourCRUDOperations,
            ISizeCRUDOperations sizeCRUDOperations,IConfiguration configuration,IExternalAPIService externalAPIService)
        {
            _dbContext = dbContext;
            _colourCRUDOperations = colourCRUDOperations;
            _sizeCRUDOperations = sizeCRUDOperations;
            colourAPI = configuration.GetConnectionString("ColoursAPI");
            sizeAPI = configuration.GetConnectionString("SizesAPI");
            _externalAPIService = externalAPIService;
        }

        public StatusVM AddProduct(ProductVM productVM)
        {
            try
            {
                if (ValidateData(productVM).statusCode != HttpStatusCode.OK)
                    return ValidateData(productVM);
                else
                {
                    Products product = new Products();
                    product.productID = productVM.productID;
                    product.productName = productVM.productName;
                    product.productYear = productVM.productYear;
                    product.channelID = productVM.channelID;
                    product.CreatedBy = productVM.createdBy; // need to assign actual user
                    product.CreatedDate = DateTime.Now;
                    product.productCode = GenerateProductCode(productVM);
                    _dbContext.Products.Add(product);

                    foreach(var item in productVM.articles)
                    {
                        Colour colour = new Colour();
                        colour.ColourID = item.ColourID;
                        colour.ArticleID = item.ArticleID;
                        colour.productCode = product.productCode;
                        _dbContext.Colours.Add(colour);
                    }
                    foreach(var item in productVM.sizes)
                    {
                        Size size = new Size();
                        size.SizeID = item.SizeID;
                        size.productCode = product.productCode;
                        _dbContext.Sizes.Add(size);
                    }
                }
                _dbContext.SaveChangesAsync();
                StatusVM status = new StatusVM();
                status.statusCode = HttpStatusCode.Created;
                status.statusMessage = "Product Created Successfully";
                return status;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public string GetProduct(Guid productId)
        {
            try
            {
                var productVM = new ProductVM();
                var product = _dbContext.Products.FirstOrDefault(x => x.productID == productId);
                if(product != null)
                {
                    var colours = _colourCRUDOperations.GetColour(product.productCode).ToList();
                    var sizes = _sizeCRUDOperations.GetSizeList(product.productCode).ToList();
                    productVM.productID = product.productID;
                    productVM.productCode = product.productCode;
                    productVM.productName = product.productName;
                    productVM.productYear = product.productYear;
                    productVM.channelID = product.channelID;
                    productVM.createdBy = product.CreatedBy;
                    productVM.createdDate = product.CreatedDate;
                    foreach(var colour in colours)
                    {
                        ColourVM colourVM = new ColourVM();
                        colourVM.ArticleID = colour.ArticleID;
                        colourVM.ColourID = colour.ColourID;
                        ColourEntity colourEntity = new ColourEntity();
                        colourEntity = _externalAPIService.GetColourEntity(colourAPI, colour.ColourID);
                        colourVM.ColourCode = colourEntity.colourCode;
                        colourVM.ColourName = colourEntity.colourName;
                        colourVM.ArticleName = product.productName + "-" + colourEntity.colourCode;
                        productVM.articles.Add(colourVM);
                    }
                    foreach(var size in sizes)
                    {
                        SizeVM sizeVM = new SizeVM();
                        sizeVM.SizeID = size.SizeID;
                        SizeEntity sizeEntity = new SizeEntity();
                        sizeEntity = _externalAPIService.GetSizeEntity(sizeAPI, size.SizeID);
                        sizeVM.SizeName = sizeEntity.sizeName;
                        productVM.sizes.Add(sizeVM);
                    }

                    return JsonConvert.SerializeObject(productVM);
                }
                return String.Empty;
                //return productVM;
                
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private StatusVM ValidateData(ProductVM product)
        {
            StatusVM status = new StatusVM();
            status.statusCode = HttpStatusCode.OK;
            if (product == null)
            {
                status.statusCode = HttpStatusCode.BadRequest;
                status.statusMessage = "Product is not valid";
            }
            else
            {
                if(string.IsNullOrEmpty(product.productName) || product.productName.Length > 100)
                {
                    status.statusCode = HttpStatusCode.NotAcceptable;
                    status.statusMessage = "Product Name is not valid";
                }
                if(product.productYear < 2020)
                {
                    status.statusCode = HttpStatusCode.BadRequest;
                    status.statusMessage = "Product year is not valid";
                }
                if(product.channelID<1 || product.channelID>3)
                {
                    status.statusCode = HttpStatusCode.BadRequest;
                    status.statusMessage = "Product Channel is not valid";
                }
                if (product.articles == null || product.articles.Count==0)
                {
                    status.statusCode = HttpStatusCode.BadRequest;
                    status.statusMessage = "Product article is not valid";
                }
                else
                {
                    foreach (var article in product.articles)
                    {
                        if (_externalAPIService.GetColourEntity(colourAPI, article.ColourID)==null)
                        {
                            status.statusCode = HttpStatusCode.BadRequest;
                            status.statusMessage = "Product article is not valid";
                            break;
                        }
                    }
                }
                if (product.sizes == null || product.sizes.Count==0)
                {
                    status.statusCode = HttpStatusCode.BadRequest;
                    status.statusMessage = "Product size is not valid";
                }
                else
                {
                    foreach (var size in product.sizes)
                    {
                        if (_externalAPIService.GetSizeEntity(colourAPI, size.SizeID) == null)
                        {
                            status.statusCode = HttpStatusCode.BadRequest;
                            status.statusMessage = "Product size is not valid";
                            break;
                        }
                    }
                }
            }
            return status;
        }
        private string GenerateProductCode(ProductVM productVM)
        {
            if(productVM.channelID == 1) 
            {
                // this logic needs to be improved.. there is a probablity that at some point of time this productcode
                // will conflict with channel id 3 productcode.. 
                var lastCode = _dbContext.Products.Where(x => x.productYear == productVM.productYear && x.channelID == productVM.channelID).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
                if(lastCode != null)
                {
                    int code = Convert.ToInt32(lastCode.productCode.Substring(4))+1;
                    if(code.ToString().Length == 1)
                        return productVM.productYear.ToString() + "00" +code.ToString();
                    else if(code.ToString().Length == 2)
                        return productVM.productYear.ToString() + "0" + code.ToString();
                    else
                        return productVM.productYear.ToString() + code.ToString();
                }
                else
                    return productVM.productYear.ToString() + "001";
            }
            else if(productVM.channelID == 2)
            {
                string alphaCode = String.Empty;
                var codelist = _dbContext.Products.Where(x => x.channelID == productVM.channelID).OrderByDescending(x => x.CreatedDate).ToList();
                do
                {
                    alphaCode = GenerateAlphaNumericCode();
                }
                while (codelist.Where(x => x.productCode == alphaCode).Any());

                return alphaCode;
            }
            else
            {
                var lastCode = _dbContext.Products.Where(x => x.channelID == productVM.channelID).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
                if (lastCode == null)
                    return "10000000";
                else
                    return (Convert.ToInt32(lastCode.productCode) + 1).ToString();
            }
        }

        private string GenerateAlphaNumericCode()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 6)
                    .Select(s => s[random.Next(s.Length)]).ToArray());
        }

    }
}
