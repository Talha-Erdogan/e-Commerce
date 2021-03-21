using e_Commerce.API.Business.Interfaces;
using e_Commerce.API.Business.Models;
using e_Commerce.API.Business.Models.Product;
using e_Commerce.API.Data;
using e_Commerce.API.Data.Entity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Dynamic.Core;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace e_Commerce.API.Business
{
    public class ProductService : IProductService
    {
        private IConfiguration _config;

        public ProductService(IConfiguration config)
        {
            _config = config;
        }

        public PaginatedList<ProductWithDetail> GetAllPaginatedWithDetailBySearchFilter(ProductSearchFilter searchFilter)
        {
            PaginatedList<ProductWithDetail> resultList = new PaginatedList<ProductWithDetail>(new List<ProductWithDetail>(), 0, searchFilter.CurrentPage, searchFilter.PageSize, searchFilter.SortOn, searchFilter.SortDirection);

            using (AppDBContext dbContext = new AppDBContext(_config))
            {
                var query = from p in dbContext.Product.Where(x => x.IsDeleted == false)
                            from c in dbContext.Category.Where(x => x.Id == p.CategoryId).DefaultIfEmpty()
                            select new ProductWithDetail()
                            {
                                Id = p.Id,
                                IsDeleted = p.IsDeleted,
                                CategoryId = p.CategoryId,
                                DeletedBy = p.DeletedBy,
                                DeletedDateTime = p.DeletedDateTime,
                                DescriptionEN = p.DescriptionEN,
                                DescriptionTR = p.DescriptionTR,
                                ImageFilePath = p.ImageFilePath,
                                NameEN = p.NameEN,
                                NameTR = p.NameTR,

                                Category_NameTR = c == null ? String.Empty : c.NameTR,
                                Category_NameEN = c == null ? String.Empty : c.NameEN
                            };

                // filtering
                if (!string.IsNullOrEmpty(searchFilter.Filter_NameTR))
                {
                    query = query.Where(r => r.NameTR.Contains(searchFilter.Filter_NameTR));
                }

                if (!string.IsNullOrEmpty(searchFilter.Filter_NameEN))
                {
                    query = query.Where(r => r.NameEN.Contains(searchFilter.Filter_NameEN));
                }

                if (searchFilter.Filter_CategoryId.HasValue)
                {
                    query = query.Where(r => r.CategoryId == searchFilter.Filter_CategoryId.Value);
                }


                // asnotracking
                query = query.AsNoTracking();

                //total count
                var totalCount = query.Count();

                //sorting
                if (!string.IsNullOrEmpty(searchFilter.SortOn))
                {
                    // using System.Linq.Dynamic.Core; nuget paketi ve namespace eklenmelidir, dynamic order by yapmak icindir
                    query = query.OrderBy(searchFilter.SortOn + " " + searchFilter.SortDirection.ToUpper());
                }
                else
                {
                    // deefault sıralama vermek gerekiyor yoksa skip metodu hata veriyor ef 6'da -- 28.10.2019 15:40
                    // https://stackoverflow.com/questions/3437178/the-method-skip-is-only-supported-for-sorted-input-in-linq-to-entities
                    query = query.OrderBy(r => r.Id);
                }

                //paging
                query = query.Skip((searchFilter.CurrentPage - 1) * searchFilter.PageSize).Take(searchFilter.PageSize);


                resultList = new PaginatedList<ProductWithDetail>(
                    query.ToList(),
                    totalCount,
                    searchFilter.CurrentPage,
                    searchFilter.PageSize,
                    searchFilter.SortOn,
                    searchFilter.SortDirection
                    );
            }

            return resultList;
        }

        public List<Product> GetAll()
        {
            List<Product> resultList = new List<Product>();
            using (AppDBContext dbContext = new AppDBContext(_config))
            {
                resultList.AddRange(dbContext.Product.Where(x => x.IsDeleted == false).AsNoTracking().ToList());
            }
            return resultList;
        }

        public List<ProductWithDetail> GetAllForCustomer(ProductSearchForCustomer searchForCustomer)
        {
            List<ProductWithDetail> resultList = new List<ProductWithDetail>();
            using (AppDBContext dbContext = new AppDBContext(_config))
            {
                var query = from p in dbContext.Product.Where(x => x.IsDeleted == false)
                            from c in dbContext.Category.Where(x => x.Id == p.CategoryId).DefaultIfEmpty()
                            select new ProductWithDetail()
                            {
                                Id = p.Id,
                                IsDeleted = p.IsDeleted,
                                CategoryId = p.CategoryId,
                                DeletedBy = p.DeletedBy,
                                DeletedDateTime = p.DeletedDateTime,
                                DescriptionEN = p.DescriptionEN,
                                DescriptionTR = p.DescriptionTR,
                                ImageFilePath = p.ImageFilePath,
                                NameEN = p.NameEN,
                                NameTR = p.NameTR,

                                Category_NameTR = c == null ? String.Empty : c.NameTR,
                                Category_NameEN = c == null ? String.Empty : c.NameEN
                            };

                // filtering
                if (!string.IsNullOrEmpty(searchForCustomer.Filter_NameTR))
                {
                    query = query.Where(r => r.NameTR.Contains(searchForCustomer.Filter_NameTR));
                }
                if (!string.IsNullOrEmpty(searchForCustomer.Filter_NameEN))
                {
                    query = query.Where(r => r.NameEN.Contains(searchForCustomer.Filter_NameEN));
                }
                if (searchForCustomer.Filter_CategoryId.HasValue)
                {
                    query = query.Where(r => r.CategoryId == searchForCustomer.Filter_CategoryId.Value);
                }
                // asnotracking
                query = query.AsNoTracking();
                resultList = query.ToList();
            }
            return resultList;
        }

        public Product GetById(long id)
        {
            Product result = null;

            using (AppDBContext dbContext = new AppDBContext(_config))
            {
                result = dbContext.Product.Where(a => a.Id == id && a.IsDeleted == false).AsNoTracking().SingleOrDefault();
            }

            return result;
        }

        public int Add(Product record)
        {
            int result = 0;
            record.IsDeleted = false;
            using (AppDBContext dbContext = new AppDBContext(_config))
            {
                dbContext.Entry(record).State = EntityState.Added;
                result = dbContext.SaveChanges();
            }

            return result;
        }

        public int Update(Product record)
        {
            int result = 0;

            using (AppDBContext dbContext = new AppDBContext(_config))
            {
                dbContext.Entry(record).State = EntityState.Modified;
                result = dbContext.SaveChanges();
            }

            return result;
        }



    }
}
