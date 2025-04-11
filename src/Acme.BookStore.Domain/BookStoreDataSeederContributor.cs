using System;
using System.Threading.Tasks;
using Acme.BookStore.Books;
using Acme.BookStore.Assignments;
using Acme.BookStore.Students;
using Acme.BookStore.StudentAssignments;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace Acme.BookStore
{
    public class BookStoreDataSeederContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IRepository<Book, Guid> _bookRepository;
        private readonly IRepository<Student, Guid> _studentRepository;
        private readonly IRepository<Assignment, Guid> _assignmentRepository;
        private readonly IRepository<StudentAssignment, Guid> _studentAssignmentRepository;
        private readonly IGuidGenerator _guidGenerator;

        public BookStoreDataSeederContributor(
            IRepository<Book, Guid> bookRepository,
            IRepository<Student, Guid> studentRepository,
            IRepository<Assignment, Guid> assignmentRepository,
            IRepository<StudentAssignment, Guid> studentAssignmentRepository,
            IGuidGenerator guidGenerator)
        {
            _bookRepository = bookRepository;
            _studentRepository = studentRepository;
            _assignmentRepository = assignmentRepository;
            _studentAssignmentRepository = studentAssignmentRepository;
            _guidGenerator = guidGenerator;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            if (await _bookRepository.GetCountAsync() <= 0)
            {
                await _bookRepository.InsertAsync(
                    new Book(
                        _guidGenerator.Create(),
                        "1984",
                        BookType.Dystopia,
                        new DateTime(1949, 6, 8),
                        19.84f
                    ),
                    autoSave: true
                );

                await _bookRepository.InsertAsync(
                    new Book(
                        _guidGenerator.Create(),
                        "The Hitchhiker's Guide to the Galaxy",
                        BookType.ScienceFiction,
                        new DateTime(1995, 9, 27),
                        42.0f
                    ),
                    autoSave: true
                );
            }

            if (await _studentRepository.GetCountAsync() <= 0)
            {
                var student1 = new Student(_guidGenerator.Create(), 
                                            "John Doe", "john.doe@example.com");
                var student2 = new Student(_guidGenerator.Create(), 
                                            "Jane Smith", "jane.smith@example.com");

                await _studentRepository.InsertAsync(student1);
                await _studentRepository.InsertAsync(student2);
            }

            if (await _assignmentRepository.GetCountAsync() <= 0)
            {
                var assignment1 = new Assignment(_guidGenerator.Create(), 
                                                "Math Assignment", 
                                                "Solve the problems", 
                                                new DateTime(2023, 12, 31));
                var assignment2 = new Assignment(_guidGenerator.Create(), 
                                                "Science Assignment", 
                                                "Write a report", 
                                                new DateTime(2023, 12, 31));

                await _assignmentRepository.InsertAsync(assignment1);
                await _assignmentRepository.InsertAsync(assignment2);
            }

            if (await _studentAssignmentRepository.GetCountAsync() <= 0)
            {
                var student1 = await _studentRepository.FirstOrDefaultAsync(s => s.Name == "John Doe");
                var student2 = await _studentRepository.FirstOrDefaultAsync(s => s.Name == "Jane Smith");
                var assignment1 = await _assignmentRepository.FirstOrDefaultAsync(a => a.Title == "Math Assignment");
                var assignment2 = await _assignmentRepository.FirstOrDefaultAsync(a => a.Title == "Science Assignment");

                if (student1 != null && assignment1 != null)
                {
                    var studentAssignment1 = new StudentAssignment(_guidGenerator.Create(), 
                                                                    student1.Id, assignment1.Id, 
                                                                    DateTime.Now, 
                                                                    "A");
                    await _studentAssignmentRepository.InsertAsync(studentAssignment1);
                }

                if (student2 != null && assignment2 != null)
                {
                    var studentAssignment2 = new StudentAssignment(_guidGenerator.Create(), 
                                                                    student2.Id, 
                                                                    assignment2.Id, 
                                                                    DateTime.Now, "B");
                    await _studentAssignmentRepository.InsertAsync(studentAssignment2);
                }
            }
        }
    }
}
