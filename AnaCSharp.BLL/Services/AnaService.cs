using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnaCSharp.DAL;
using AnaCSharp.DAL.Model;
using Microsoft.EntityFrameworkCore;

namespace AnaCSharp.BLL.Services
{
    public class AnaService
    {
        private readonly AnaContext _anaContext;

        public AnaService(AnaContext anaContext)
        {
            _anaContext = anaContext;
        }

        
    }
}
