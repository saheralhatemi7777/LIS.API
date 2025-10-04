using APiUsers.Data;
using APiUsers.DTOs;
using APiUsers.Repository.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using مشروع_ادار_المختبرات.Models;

namespace APiUsers.Repository
{

    public class MainRepository<T> : IReopsitory<T> where T : class
    {
        private readonly DbSet<T> _dbSet;
        public  MainRepository(AppDbContext context)
        {
            
                _context = context;
                _dbSet = _context.Set<T>();
            
        }


        private readonly AppDbContext _context;
        public async void AddOne(T entity)
        {
            _context.Set<T>().Add(entity);
            _context.SaveChanges();
        }

        public void UpdateOne(T entity)
        {
            _context.Set<T>().Update(entity);
            _context.SaveChanges();
        }
        public void DeleteOne(T entity)
        {
            _context.Set<T>().Remove(entity);
            _context.SaveChanges();
        }

        public async Task<IEnumerable<T>> FillDataAsync(params string[] arges)
        {
            IQueryable<T> Query = _context.Set<T>();
            if (arges.Length > 0)
            {
                foreach (var agr in arges)
                {
                    Query = Query.Include(agr);
                }
            }
            return await Query.ToListAsync();
        }

        public async Task<T> FindByidAsync(int id)
        {
            var result =await _context.Set<T>().FindAsync(id);
            if(result==null)
            { 
                return null;
            }
            return result;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> selectone(Expression<Func<T, bool>> match)
        {
            return await _context.Set<T>().SingleOrDefaultAsync(match);
        }

        public async Task<IEnumerable<T>> GetAllIncluding(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>();

            if (includes != null)
            {
                foreach (var includeExpression in includes)
                {
                    query = query.Include(includeExpression);
                }
            }

            return await query.ToListAsync();
        }

        public async Task<T> selectAll(Expression<Func<T, bool>> match)
        {
            return await _context.Set<T>().SingleOrDefaultAsync(match);
        }

        async Task<List<T>> IReopsitory<T>.selectAll(Expression<Func<T, bool>> match)
        {

            return await _context.Set<T>().Where(match).ToListAsync();

        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {

            return await _dbSet.Where(predicate).ToListAsync();

        }

        public async Task<IEnumerable<T>> AddListAsync(IEnumerable<T> entities)
        {
            _context.Set<T>().AddRange(entities);
            await _context.SaveChangesAsync();
            return entities;
        }

        public async Task AddTestsToRequestAsync(int requestId, List<int> testIds)
        {
            var newItems = testIds.Select(testId => new RequestTest
            {
                RequestID = requestId,  // ربط بالطلب
                TestID =  testId 
            }).ToList();

            _context.RequestTest.AddRange(newItems);
            await _context.SaveChangesAsync();
        }


        public async Task UpdateListAsync(int requestTestId, List<int> testIds)
        {
            var newItems = testIds.Select(testId => new RequestTest
            {
                RequestID = requestTestId,  // ربط بالطلب
                TestID = testId
            }).ToList();

            _context.RequestTest.UpdateRange(newItems);
            await _context.SaveChangesAsync();
        }

       

    
        public async Task AddTestsResultToRequestAsync(int RequsestTest, List<int> testIds, List<string> ResultValue, int Requestid, DateTime CreatedAt, int LabTechniciansUserID)
        {
            if (testIds.Count != ResultValue.Count)
                throw new ArgumentException("عدد التحاليل لا يساوي عدد النتائج");

            var newItems = testIds.Select((testId, index) => new TestResult
            {
                RequestTestID = RequsestTest,
                TestId = testId,
                ResultValue = ResultValue[index],
                CreatedAt = CreatedAt,
                LabTechniciansUserID = LabTechniciansUserID
            }).ToList();

            _context.testResult.AddRange(newItems);
            await _context.SaveChangesAsync();
        }

     
        public async Task<List<T>> FindListByIdAsync(List<int> ids)
        {
            // نجيب تعريف الكيان (Entity) من الكونتكست
            var entityType = _context.Model.FindEntityType(typeof(T));

            // نجيب اسم المفتاح الأساسي
            var keyName = entityType.FindPrimaryKey().Properties
                                    .Select(x => x.Name)
                                    .Single(); // نفترض المفتاح الأساسي واحد فقط

            // نبني الـ query بشكل ديناميكي باستخدام EF.Property
            return await _context.Set<T>()
                                 .Where(e => ids.Contains(EF.Property<int>(e, keyName)))
                                 .ToListAsync();
        }

        public async Task<List<int>> AddTestsToRequestAsyncS(int requestTestId, List<int> testIds)
        {
            var addedRecords = new List<RequestTest>();

            foreach (var testId in testIds)
            {
                var newRecord = new RequestTest
                {
                    RequestID = requestTestId,
                    TestID = testId
                };
                _context.RequestTest.Add(newRecord);
                addedRecords.Add(newRecord);
            }

            await _context.SaveChangesAsync();

            return addedRecords.Select(r => r.RequestTestID).ToList();
        }

        public void AddList(T entity)
        {
            _context.Set<T>().AddRange(entity);
            _context.SaveChanges();
        }

        public async Task<List<int>> AddRecordRequestTest(int requestTestId, List<int> testIds)
        {
            var addedRecords = new List<RecordRequestTest>();

            foreach (var testId in testIds)
            {
                var newRecord = new RecordRequestTest
                {
                    RecordId = requestTestId,
                    RequestTestId = testId
                };
                _context.RecordRequestTests.Add(newRecord);
                addedRecords.Add(newRecord);
            }

            await _context.SaveChangesAsync();

            return addedRecords.Select(r => r.Id).ToList();
        }
       //داله لجلب بيانات التحاليل للفاتورة
        public async Task<List<Test>> FindByListidAsync(List<int> ids)
        {
            if (ids == null || !ids.Any())
                return new List<Test>();

            var result = await _context.Test
                .Where(i => ids.Contains(i.TestId))
                .ToListAsync();

            return result;
        }

    }
}
