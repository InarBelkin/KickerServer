using DAL;

namespace BLLAdapter.Repositories.Base;

public abstract class EfReposBase
{
    protected KickerContext _db;

    protected EfReposBase(KickerContext db)
    {
        _db = db;
    }
}