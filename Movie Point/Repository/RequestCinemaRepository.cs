﻿using Movie_Point.Data;
using Movie_Point.Models;
using Movie_Point.Repository.IRepository;
using Movie_Point.Repositroy;

namespace Movie_Point.Repository
{
    public class RequestCinemaRepository : Repository<RequestCinema> , IRequestCinemaRepository
    {
        public RequestCinemaRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
