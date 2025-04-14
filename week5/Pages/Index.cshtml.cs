using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using week5.Models;
using System.Collections.Generic;
using System.Linq;

namespace week5.Pages {
    public class IndexModel : PageModel {
        [BindProperty]
        public ClassInformationModel NewClass { get; set; } = new ClassInformationModel();

        public static List<ClassInformationModel> ClassList { get; set; } = new List<ClassInformationModel>();

        private static int _idCounter = 1;

        public void OnGet() {
            NewClass = new ClassInformationModel();
        }

        public IActionResult OnPostAdd() {
            if (!ModelState.IsValid)
                return Page();

            NewClass.Id = _idCounter++;
            ClassList.Add(NewClass);
            NewClass = new ClassInformationModel();

            return RedirectToPage();
        }

        public IActionResult OnPostDelete(int id) {
            var item = ClassList.FirstOrDefault(x => x.Id == id);
            if (item != null)
                ClassList.Remove(item);

            return RedirectToPage();
        }

        public IActionResult OnPostEdit(int id) {
            var item = ClassList.FirstOrDefault(x => x.Id == id);
            if (item != null) {
                NewClass = new ClassInformationModel {
                    Id = item.Id,
                    ClassName = item.ClassName,
                    StudentCount = item.StudentCount,
                    Description = item.Description
                };
            }
            return Page();
        }

        public IActionResult OnPostUpdate() {
            var existing = ClassList.FirstOrDefault(x => x.Id == NewClass.Id);
            if (existing != null) {
                existing.ClassName = NewClass.ClassName;
                existing.StudentCount = NewClass.StudentCount;
                existing.Description = NewClass.Description;
            }
            NewClass = new ClassInformationModel();
            return RedirectToPage();
        }
    }
}