﻿using System;
using System.Collections.Generic;
using System.Text;

namespace e_Commerce.Web.Business.Models
{
    public class PaginatedList<T>
    {
        public int CurrentPage { get; set; }

        public List<T> Items { get; set; }//private kaldırıldı. 13.03.2020 -> profileEmployee sayfasında batch editte hata alınmaktadır.
        public int PageSize { get; set; }

        public int From { get; set; }
        public int To { get; set; }

        public int TotalCount { get; set; }
        public int TotalPages { get; set; }

        public string SortOn { get; set; }
        public string SortDirection { get; set; }

        public PaginatedList()
        {

        }

        public PaginatedList(List<T> items, int totalCount, int currentPage, int pageSize, string sortOn, string sortDirection)
        {
            CurrentPage = currentPage;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            TotalCount = totalCount;
            PageSize = pageSize;
            From = ((currentPage - 1) * pageSize) + 1;
            To = (From + pageSize) - 1;

            SortOn = sortOn;
            SortDirection = sortDirection;

            Items = items;
        }

        public bool HasPreviousPage
        {
            get
            {
                return (CurrentPage > 1);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (CurrentPage < TotalPages);
            }
        }



    }
}
