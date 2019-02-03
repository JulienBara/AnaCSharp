using System.Linq;

namespace AnaCSharp.DAL.Repositories
{
    public class MaxMarkovDegreeRepository
    {
        private readonly AnaContext _anaContext;

        public MaxMarkovDegreeRepository(AnaContext anaContext)
        {
            _anaContext = anaContext;
        }

        public int GetMarkovDegree()
        {
            var markovDegree = _anaContext.MaxMarkovDegrees.FirstOrDefault();
            if (markovDegree != null)
            {
                return markovDegree.Value;
            }

            return 0;
        }

        public void SetMarkovDegree(int value)
        {
            var markovDegree = _anaContext.MaxMarkovDegrees.FirstOrDefault();
            if (markovDegree != null)
            {
                markovDegree.Value = value;
                _anaContext.SaveChanges();
            }
        }
    }
}
