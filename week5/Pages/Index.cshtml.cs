using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using week5.Models;
using week5.Helpers;
using System.Linq;

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
        public ClassInformationModel ClassInfo { get; set; } = new ClassInformationModel();

        [BindProperty]
        public int? EditId { get; set; }

        public List<int> FilteredClassIds { get; set; } = new List<int>();

        public void OnGet()
        {
            string[] classNames = { "MIS", "CENG", "SENG", "MAN" };
            string[] descriptions = { "Management Information Systems", "Computer Engineering", "Software Engineering", "Management" };

            if (!Classes.Any())
            {
                Random random = new Random();

                for (int i = 1; i <= 100; i++)
                {
                    string className = classNames[random.Next(classNames.Length)];
                    int courseCode = random.Next(101, 405);
                    string fullClassName = $"{className} {courseCode}";
                    string classDescription = descriptions[Array.IndexOf(classNames, className)] + $" {courseCode}";

                    Classes.Add(new ClassInformationModel
                    {
                        Id = i,
                        ClassName = fullClassName,
                        Description = classDescription,
                        StudentCount = random.Next(20, 100)
                    });
                }
            }

            var query = Classes.AsQueryable();

            if (!string.IsNullOrWhiteSpace(FilterClassName))
            {
                query = query.Where(c => c.ClassName.Contains(FilterClassName, StringComparison.OrdinalIgnoreCase));
            }

            query = query.Where(c => c.StudentCount > 0);
            FilteredClassIds = query.Select(c => c.Id).ToList();

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

        public IActionResult OnGetEdit(int id)
        {
            var classToEdit = Classes.FirstOrDefault(c => c.Id == id);
            if (classToEdit != null)
            {
                ClassInfo = classToEdit;
                EditId = id;
            }

            OnGet();
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

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

            return RedirectToPage("./Index");
        }

        public IActionResult OnPostDelete(int id)
        {
            var item = Classes.FirstOrDefault(c => c.Id == id);
            if (item != null)
            {
                Classes.Remove(item);
            }
            return RedirectToPage("./Index");
        }

        public IActionResult OnGetExportJson(bool isFiltered, string selectedColumns, string? filterClassName)
        {
            var columnList = (selectedColumns ?? "")
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(c => c.Trim())
                .ToList();

            List<ClassInformationModel> exportData;

            if (isFiltered)
            {
                var query = Classes.AsQueryable();

                if (!string.IsNullOrWhiteSpace(filterClassName))
                {
                    query = query.Where(c => c.ClassName.Contains(filterClassName, StringComparison.OrdinalIgnoreCase));
                }

                query = query.Where(c => c.StudentCount > 0);
                exportData = query.ToList();
            }
            else
            {
                exportData = Classes;
            }

            string json = Utils.Instance.SerializeToJson(exportData, columnList);

            return File(System.Text.Encoding.UTF8.GetBytes(json), "application/json", "export.json");
        }

        public List<object> GetPaginationPages()
        {
            var pages = new List<object>();

            if (TotalPages <= 5)
            {
                for (int i = 1; i <= TotalPages; i++)
                    pages.Add(i);
                return pages;
            }

            pages.Add(1);

            if (CurrentPage > 3)
                pages.Add("...");

            int start = Math.Max(2, CurrentPage - 2);
            int end = Math.Min(TotalPages - 1, CurrentPage + 2);

            for (int i = start; i <= end; i++)
            {
                if (!pages.Contains(i))
                    pages.Add(i);
            }

            if (CurrentPage + 2 < TotalPages - 1)
                pages.Add("...");

            if (!pages.Contains(TotalPages))
                pages.Add(TotalPages);

            return pages;
        }
    }
}