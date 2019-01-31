using ProERP.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProERP.Services.Product
{
    public class ProductService
    {
        private readonly IRepository<Data.Models.ProductMaster> _productRepository;
        private readonly IRepository<Data.Models.ProductCategory> _productCategoryRepository;
        private readonly IRepository<Data.Models.FormulationRequest> _formulationRequestRepository;

        public ProductService(IRepository<Data.Models.ProductMaster> productRepository,
                              IRepository<Data.Models.ProductCategory> productCategoryRepository,
                              IRepository<Data.Models.FormulationRequest> formulationRequestRepository)
        {
            this._productRepository = productRepository;
            this._productCategoryRepository = productCategoryRepository;
            this._formulationRequestRepository = formulationRequestRepository;
        }

        public int SaveProduct(Data.Models.ProductMaster productData)
        {
            this._productRepository.Insert(productData);
            return productData.Id;
        }

        public int UpdateProduct(Data.Models.ProductMaster productData)
        {
            Data.Models.ProductMaster existingData = this._productRepository.Table.FirstOrDefault(W => W.Id == productData.Id);
            if (existingData != null)
            {
                existingData.CategoryId = productData.CategoryId;
                existingData.Code = productData.Code;
                existingData.Name = productData.Name;
                existingData.ShortName = productData.ShortName;
                existingData.Description = productData.Description;
                _productRepository.Update(existingData);
                return productData.Id;
            }
            else
                return 0;
        }

        public List<Data.Models.ProductMaster> GetProdutListForGrid(string productName)
        {
            var allData = this._productRepository.Table;
            if (!string.IsNullOrEmpty(productName))
            {
                allData = allData.Where(W => W.Name.Contains(productName));
            }
            return allData.ToList();
        }

        public Data.Models.ProductMaster GetProductById(int Id)
        {
            return this._productRepository.Table.FirstOrDefault(w => w.Id == Id);
        }

        public int DeleteProduct(int Id)
        {
            Data.Models.ProductMaster objProduct = this._productRepository.Table.FirstOrDefault(W => W.Id == Id);
            if (objProduct != null)
            {
                _productRepository.Delete(objProduct);
                return 1;
            }
            else
                return 0;
        }

        public List<Data.Models.ProductCategory> GetProductCategoryList()
        {
            return this._productCategoryRepository.Table.OrderBy(o => o.Name).ToList();
        }

        public List<Data.Models.ProductMaster> GetProductListById(int CategoryId)
        {
            return this._productRepository.Table.Where(w => w.CategoryId == CategoryId).OrderBy(o => o.Name).ToList();
        }

        public List<Data.Models.ProductMaster> GetProductNameByLine(int LineId)
        {
            int[] productId = this._formulationRequestRepository.Table.Where(w => w.LineId == LineId).Distinct().Select(s => s.ProductId).ToArray();
            return this._productRepository.Table.Where(w => productId.Contains(w.Id)).ToList();
        }
        public Data.Models.ProductMaster GetGradeNameById(int Id)
        {
            if (Id > 0)
            {
                var productId = this._formulationRequestRepository.Table.FirstOrDefault(w => w.Id == Id).ProductId;
                return this._productRepository.Table.FirstOrDefault(w => w.Id == productId);
            }
            else
            {
                return null;
            }
        }

        public Data.Models.ProductMaster GetProductByBatchId(int BatchId)
        {
            var productId = this._formulationRequestRepository.Table.FirstOrDefault(w => w.Id == BatchId).ProductId;
            return this._productRepository.Table.FirstOrDefault(w => w.Id == productId);
        }

    }
}
