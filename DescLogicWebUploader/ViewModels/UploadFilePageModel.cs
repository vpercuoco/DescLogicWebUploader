using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FirstASPNETCOREProject.ViewModels
{
    public class UploadFilePageModel
    {
        public void OnGet()
        {

        }
        private IWebHostEnvironment _environment;
        public UploadFilePageModel(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        public UploadFilePageModel()
        {

        }

        [BindProperty]
        [Required]
        public IFormFile Upload { get; set; }
       public async Task OnPostAsync()
        {
                var file = Path.Combine(@"C:\Users\percuoco\source\repos\FirstASPNETCOREProject\FirstASPNETCOREProject\uploads\", Upload.FileName);
                using (var fileStream = new FileStream("", FileMode.Create))
                {
                    await Upload.CopyToAsync(fileStream);
                }
               
        }
      
    }
}