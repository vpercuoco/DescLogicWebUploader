using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace FirstASPNETCOREProject.ViewModels
{
    public class TestModel
    {

        [BindProperty]
        [Required]
        public IEnumerable<IFormFile> DescriptionFiles { get; set; }


        [BindProperty]
        [Required]
        public IEnumerable<IFormFile> MeasurementFiles { get; set; }

        [BindProperty]
        public List<string> MeasurementFilenames { get; set; } = new List<string>();
        [BindProperty]
        public List<string> DescriptionFilenames { get; set; } = new List<string>();


        public string DescriptionImportDirectory { get; set; } = Directory.GetCurrentDirectory() + @"\uploads\Imports\Descriptions\";
        public string DescriptionExportDirectory { get; set; } = Directory.GetCurrentDirectory() + @"\uploads\Exports\Descriptions\";

        public string MeasurementImportDirectory { get; set; } = Directory.GetCurrentDirectory() + @"\uploads\Imports\Measurements\";
        public string MeasurementExportDirectory { get; set; } = Directory.GetCurrentDirectory() + @"\uploads\Exports\Measurements\";

        public string type { get; set; } = "description";

        [BindProperty]
        public string SessionID { get; set; } = null;


        public List<DirectoryInfo> GetSessionFilenames(string sessionID, string directory)
        {

            List<DirectoryInfo> files = new List<DirectoryInfo>();

            foreach (var filepath in Directory.GetFiles(directory).Where(file => file.Split(@"\").Last().StartsWith(sessionID)).ToList())
            {
                files.Add(new DirectoryInfo(filepath));
            }

            return files;
        }


        public async Task OnPostAsync()
        {

            if (SessionID == null)
            {
                SessionID = DateTime.Now.Ticks.ToString();
            }

            foreach (var ifile in MeasurementFiles)
            {
                if (!MeasurementFilenames.Contains(ifile.FileName))
                {
                    MeasurementFilenames.Add(ifile.FileName);
                }

                var filepath = Path.Combine(MeasurementImportDirectory, SessionID + "_" + ifile.FileName);
                using (var fileStream = new FileStream(filepath, FileMode.Create))
                {
                    await ifile.CopyToAsync(fileStream).ConfigureAwait(true);
                }
            }


            foreach (var ifile in DescriptionFiles)
            {
                if (!DescriptionFilenames.Contains(ifile.FileName))
                {
                    DescriptionFilenames.Add(ifile.FileName);


                    var filepath = Path.Combine(DescriptionImportDirectory, SessionID + "_" + ifile.FileName);
                    using (var fileStream = new FileStream(filepath, FileMode.Create))
                    {
                        await ifile.CopyToAsync(fileStream).ConfigureAwait(true);
                    }
                }

            }
        }
    }
}
    
