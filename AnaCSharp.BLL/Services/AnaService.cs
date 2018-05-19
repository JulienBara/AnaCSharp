using AnaCSharp.DAL;

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
