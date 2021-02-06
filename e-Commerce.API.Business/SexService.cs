using e_Commerce.API.Business.Interfaces;
using e_Commerce.API.Data;
using e_Commerce.API.Data.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace e_Commerce.API.Business
{
    public class SexService : ISexService
    {
        private IConfiguration _config;

        public SexService(IConfiguration config)
        {
            _config = config;
        }

        public List<Sex> GetAll()
        {
            List<Sex> resultList = new List<Sex>();
            using (AppDBContext dbContext = new AppDBContext(_config))
            {
                resultList.AddRange(dbContext.Sex.AsNoTracking().ToList());
            }
            return resultList;
        }

        public Sex GetById(int id)
        {
            Sex result = new Sex();
            using (AppDBContext dbContext = new AppDBContext(_config))
            {
                result = dbContext.Sex.Where(r => r.Id == id).AsNoTracking().SingleOrDefault();
            }
            return result;
        }
    }
}
