using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using week5.Models;

namespace week5.Pages
{
    public class IndexModel : PageModel
    {
        public static List<ClassInformationModel> Classes { get; set; } = new List<ClassInformationModel>();
        public List<ClassInformationTable> FilteredClasses { get; set; } = new List<ClassInformationTable>();

        [BindProperty(SupportsGet = true)]
        public string? FilterClassName { get; set; }

        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;

        public int PageSize { get; set; } = 10;
        public int TotalPages { get; set; }

        [BindProperty]
        public ClassInformationModel ClassInfo { get; set; } = new ClassInformationModel
        {
            ClassName = string.Empty,
            Description = string.Empty,
            StudentCount = 0
        };

        [BindProperty]
        public int? EditId { get; set; }

        public void OnGet()
        {
            
            if (!Classes.Any())
            {
                for (int i = 1; i <= 100; i++)
                {
                    Classes.Add(new ClassInformationModel
                    {
                        Id = i,
                        ClassName = $"Class {i}",
                        Description = $"This is the description for class {i}.",
                        StudentCount = i * 2
                    });
                }
            }

            var query = Classes.AsQueryable();

            if (!string.IsNullOrWhiteSpace(FilterClassName))
            {
                query = query.Where(c => c.ClassName.Contains(FilterClassName, StringComparison.OrdinalIgnoreCase));
            }

            query = query.Where(c => c.StudentCount > 0);

            int totalItems = query.Count();
            TotalPages = (int)Math.Ceiling(totalItems / (double)PageSize);

            var paginated = query
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .Select(c => new ClassInformationTable
                {
                    Id = c.Id,
                    ClassName = c.ClassName,
                    Description = c.Description,
                    StudentCount = c.StudentCount
                })
                .ToList();

            FilteredClasses = paginated;
        }
        
        public IActionResult OnPostDelete(int id)
        {
            var item = Classes.FirstOrDefault(c => c.Id == id);
            if (item != null)
                Classes.Remove(item);

            return RedirectToPage(new { FilterClassName, CurrentPage });
        }

        public IActionResult OnPost()
        {
            if (EditId.HasValue)
            {
                var existing = Classes.FirstOrDefault(c => c.Id == EditId.Value);
                if (existing != null)
                {
                    existing.ClassName = ClassInfo.ClassName;
                    existing.Description = ClassInfo.Description;
                    existing.StudentCount = ClassInfo.StudentCount;
                }
            }
            else
            {
                int newId = Classes.Any() ? Classes.Max(c => c.Id) + 1 : 1;
                ClassInfo.Id = newId;
                Classes.Add(ClassInfo);
            }

            return RedirectToPage("Index", new { FilterClassName, CurrentPage });
        }

        public void OnGetEdit(int id)
        {
            var classToEdit = Classes.FirstOrDefault(c => c.Id == id);
            if (classToEdit != null)
            {
                ClassInfo = new ClassInformationModel
                {
                    Id = classToEdit.Id,
                    ClassName = classToEdit.ClassName,
                    Description = classToEdit.Description,
                    StudentCount = classToEdit.StudentCount
                };
                EditId = classToEdit.Id;
            }

            OnGet();
        }
    }
}