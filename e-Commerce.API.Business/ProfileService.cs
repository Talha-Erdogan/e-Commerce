using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Dynamic.Core;
using e_Commerce.API.Business.Interfaces;
using Microsoft.Extensions.Configuration;
using e_Commerce.API.Data.Entity;
using System.Linq;
using e_Commerce.API.Business.Models;
using e_Commerce.API.Data;
using e_Commerce.API.Business.Models.Profile;
using Microsoft.EntityFrameworkCore;

namespace e_Commerce.API.Business
{
    public class ProfileService : IProfileService
    {
        private IConfiguration _config;

        public ProfileService(IConfiguration config)
        {
            _config = config;
        }

        public PaginatedList<Profile> GetAllPaginatedWithDetailBySearchFilter(ProfileSearchFilter searchFilter)
        {
            PaginatedList<Profile> resultList = new PaginatedList<Profile>(new List<Profile>(), 0, searchFilter.CurrentPage, searchFilter.PageSize, searchFilter.SortOn, searchFilter.SortDirection);

            using (AppDBContext dbContext = new AppDBContext(_config))
            {
                var query = from p in dbContext.Profile
                            where p.IsDeleted == false
                            select p;

                // filtering
                if (!string.IsNullOrEmpty(searchFilter.Filter_Code))
                {
                    query = query.Where(r => r.Code.Contains(searchFilter.Filter_Code));
                }

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


                resultList = new PaginatedList<Profile>(
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

        public List<Profile> GetAll()
        {
            List<Profile> resultList = new List<Profile>();

            using (AppDBContext dbContext = new AppDBContext(_config))
            {
                var query = dbContext.Profile.Where(x => x.IsDeleted == false).AsNoTracking();
                resultList.AddRange(query.ToList());
            }
            return resultList;
        }

        public Profile GetById(int id)
        {
            Profile result = null;

            using (AppDBContext dbContext = new AppDBContext(_config))
            {
                result = dbContext.Profile.Where(a => a.Id == id && a.IsDeleted == false).AsNoTracking().SingleOrDefault();
            }

            return result;
        }

        public int Add(Profile record)
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

        public int Update(Profile record)
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
