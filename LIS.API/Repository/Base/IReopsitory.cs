using System.Linq.Expressions;
using مشروع_ادار_المختبرات.Models;

namespace APiUsers.Repository.Base
{
    public interface IReopsitory<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T>  FindByidAsync(int id);
        Task<List<Test>> FindByListidAsync(List<int> ids);

        Task<List<T>> FindListByIdAsync(List<int> ids);

        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

        Task<T> selectone(Expression<Func<T, bool>> match);
        void AddOne(T entity);
        void AddList(T entity);


        Task<List<int>> AddTestsToRequestAsyncS(int requestTestId, List<int> testIds);
        Task<List<int>> AddRecordRequestTest(int requestTestId, List<int> testIds);

        Task AddTestsResultToRequestAsync(int RequsestTest,List<int> testIds,List<string> ResultValue,int Requestid,DateTime CreatedAt, int LabTechniciansUserID);

        Task<List<T>> selectAll(Expression<Func<T, bool>> match);
        void UpdateOne(T entity);
        void DeleteOne(T entity);

        Task UpdateListAsync(int requestTestId, List<int> testIds);


        //نفس الي تحت
        Task<IEnumerable<T>> FillDataAsync(params string[]arges);


        //عمل علاقات لاكثر من جدول باستخدام الديزاين باترن
        Task<IEnumerable<T>> GetAllIncluding(params Expression<Func<T, object>>[] includes);
        
    }
}
