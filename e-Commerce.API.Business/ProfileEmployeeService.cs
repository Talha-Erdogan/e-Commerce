using e_Commerce.API.Business.Models;
using e_Commerce.API.Data;
using e_Commerce.API.Data.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Dynamic.Core;
using e_Commerce.API.Business.Interfaces;
using Microsoft.Extensions.Configuration;
using e_Commerce.API.Business.Models.ProfileEmployee;

namespace e_Commerce.API.Business
{
    public class ProfileEmployeeService : IProfileEmployeeService
    {
        private IConfiguration _config;

        public ProfileEmployeeService(IConfiguration config)
        {
            _config = config;
        }

        public PaginatedList<Employee> GetAllEmployeePaginatedWithDetailBySearchFilter(ProfileEmployeeSearchFilter searchFilter)
        {
            PaginatedList<Employee> resultList = new PaginatedList<Employee>(new List<Employee>(), 0, searchFilter.CurrentPage, searchFilter.PageSize, searchFilter.SortOn, searchFilter.SortDirection);

            using (AppDBContext dbContext = new AppDBContext(_config))
            {
                var query = from pe in dbContext.ProfileEmployee
                            join e in dbContext.Employee on pe.EmployeeId equals e.Id
                            where pe.ProfileId == searchFilter.Filter_ProfileId
                            select e;


                if (!string.IsNullOrEmpty(searchFilter.Filter_Employee_Name))
                {
                    query = query.Where(r => r.Name.Contains(searchFilter.Filter_Employee_Name));
                }

                if (!string.IsNullOrEmpty(searchFilter.Filter_Employee_LastName))
                {
                    query = query.Where(r => r.LastName.Contains(searchFilter.Filter_Employee_LastName));
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


                resultList = new PaginatedList<Employee>(
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
        public PaginatedList<Employee> GetAllEmployeeWhichIsNotIncludedPaginatedWithDetailBySearchFilter(ProfileEmployeeSearchFilter searchFilter)
        {
            PaginatedList<Employee> resultList = new PaginatedList<Employee>(new List<Employee>(), 0, searchFilter.CurrentPage, searchFilter.PageSize, searchFilter.SortOn, searchFilter.SortDirection);

            using (AppDBContext dbContext = new AppDBContext(_config))
            {
                var queryIdList = dbContext.ProfileEmployee.Where(x => x.ProfileId == searchFilter.Filter_ProfileId).AsNoTracking().Select(x => x.EmployeeId);

                var query = from e in dbContext.Employee
                            where !queryIdList.Contains(e.Id)
                            select e;

                if (!string.IsNullOrEmpty(searchFilter.Filter_Employee_Name))
                {
                    query = query.Where(r => r.Name.Contains(searchFilter.Filter_Employee_Name));
                }

                if (!string.IsNullOrEmpty(searchFilter.Filter_Employee_LastName))
                {
                    query = query.Where(r => r.LastName.Contains(searchFilter.Filter_Employee_LastName));
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


                resultList = new PaginatedList<Employee>(
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
        public List<Profile> GetAllProfileByCurrentUser(int employeeId)
        {
            List<Profile> resultList = new List<Profile>();

            using (AppDBContext dbContext = new AppDBContext(_config))
            {
                var query = from pe in dbContext.ProfileEmployee
                            join p in dbContext.Profile on pe.ProfileId equals p.Id
                            where
                                pe.EmployeeId == employeeId && p.IsDeleted == false
                            select p;

                // as no tracking
                query = query.AsNoTracking();

                resultList.AddRange(query.ToList());

            }
            return resultList;
        }
        public List<Profile> GetAllProfileByEmployeeIdAndAuthCode(int employeeId, string authCode)
        {
            List<Profile> resultList = new List<Profile>();
            using (AppDBContext dbContext = new AppDBContext(_config))
            {
                var query = from pe in dbContext.ProfileEmployee
                            join p in dbContext.Profile on pe.ProfileId equals p.Id
                            join pd in dbContext.ProfileDetail on pe.ProfileId equals pd.ProfileId
                            join a in dbContext.Auth on pd.AuthId equals a.Id
                            where pe.EmployeeId == employeeId && a.Code == authCode && p.IsDeleted == false && a.IsDeleted == false
                            select new Profile()
                            {
                                Code = p.Code,
                                DeletedBy = p.DeletedBy,
                                DeletedDateTime = p.DeletedDateTime,
                                Id = p.Id,
                                IsDeleted = p.IsDeleted,
                                NameEN = p.NameEN,
                                NameTR = p.NameTR,
                            };

                // as no tracking
                query = query.AsNoTracking();

                resultList.AddRange(query.ToList());

            }


            return resultList;
        }
        public int Add(ProfileEmployee record)
        {
            int result = 0;

            using (AppDBContext dbContext = new AppDBContext(_config))
            {
                ProfileEmployee existrecord = dbContext.ProfileEmployee.Where(pd => pd.ProfileId == record.ProfileId && pd.EmployeeId == record.EmployeeId).FirstOrDefault();
                if (existrecord == null)
                {
                    dbContext.Entry(record).State = EntityState.Added;
                    result = dbContext.SaveChanges();
                }
            }

            return result;
        }
        public int DeleteByProfileIdAndEmployeeId(int profileId, int employeeId)
        {
            int result = 0;

            using (AppDBContext dbContext = new AppDBContext(_config))
            {
                ProfileEmployee record = dbContext.ProfileEmployee.Where(pd => pd.ProfileId == profileId && pd.EmployeeId == employeeId).AsNoTracking().SingleOrDefault();
                dbContext.Entry(record).State = EntityState.Deleted;
                result = dbContext.SaveChanges();
            }

            return result;
        }

    }
}
