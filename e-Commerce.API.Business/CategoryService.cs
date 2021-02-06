using e_Commerce.API.Business.Interfaces;
using e_Commerce.API.Business.Models;
using e_Commerce.API.Business.Models.Category;
using e_Commerce.API.Data;
using e_Commerce.API.Data.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Dynamic.Core;

namespace e_Commerce.API.Business
{
    public class CategoryService : ICategoryService
    {
        private IConfiguration _config;

        public CategoryService(IConfiguration config)
        {
            _config = config;
        }

        public PaginatedList<Category> GetAllPaginatedWithDetailBySearchFilter(CategorySearchFilter searchFilter)
        {
            PaginatedList<Category> resultList = new PaginatedList<Category>(new List<Category>(), 0, searchFilter.CurrentPage, searchFilter.PageSize, searchFilter.SortOn, searchFilter.SortDirection);

            using (AppDBContext dbContext = new AppDBContext(_config))
            {
                var query = from a in dbContext.Category
                            where a.IsDeleted == false
                            select a;

                // filtering
                if (!string.IsNullOrEmpty(searchFilter.Filter_NameTR))
                {
                    query = query.Where(r => r.NameTR.Contains(searchFilter.Filter_NameTR));
                }

                if (!string.IsNullOrEmpty(searchFilter.Filter_NameEN))
                {
                    query = query.Where(r => r.NameEN.Contains(searchFilter.Filter_NameEN));
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


                resultList = new PaginatedList<Category>(
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

        public List<Category> GetAll()
        {
            List<Category> resultList = new List<Category>();
            using (AppDBContext dbContext = new AppDBContext(_config))
            {
                resultList.AddRange(dbContext.Category.Where(x => x.IsDeleted == false).AsNoTracking().ToList());
            }
            return resultList;
        }

        public Category GetById(int id)
        {
            Category result = null;
            using (AppDBContext dbContext = new AppDBContext(_config))
            {
                result = dbContext.Category.Where(a => a.Id == id && a.IsDeleted == false).AsNoTracking().SingleOrDefault();
            }

            return result;
        }

        public int Add(Category record)
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

        public int Update(Category record)
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
