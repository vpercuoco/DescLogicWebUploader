using FirstASPNETCOREProject.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FirstASPNETCOREProject;
//using System.Net.Http.Headers;
using Microsoft.Net.Http.Headers;
using System.IO;
using System.IO.Compression;
using Microsoft.Extensions.Logging;

namespace FirstASPNETCOREProject.Controllers
{

    // This is my class, a controller that returns a view of a model

    public class AppController : Controller
    {
        //When the Appcontroller is instantiated a new instance of desclogicservice is registered in the service layer and implemented here:
        private readonly IDescLogicService _descLogicService;
        private readonly ILogger _logger;
        public AppController(IDescLogicService descLogicService, ILogger<AppController> logger)
        {
            _descLogicService = descLogicService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();

        }
        public IActionResult UploadFilePage()
        {
            return View();
        }

        [HttpPost]
        public IActionResult UploadFilePage(UploadFilePageModel Model)
        {
            return View();
        }

        public IActionResult TestView()
        {
            TestModel model = new TestModel();
            return View(model);
        }

        [HttpPost("~/App/TestView")]
        [RequestSizeLimit(1073741824)]
        public async Task<IActionResult> TestView(TestModel Model)
        {
            if (ModelState.IsValid)
            {
               await Model.OnPostAsync();
                _descLogicService.Convert(Model);
            }

            return View(Model);
        }

        public IActionResult DownloadZipFile(string sessionID, string path )
        {

           string tempLocation = Directory.GetCurrentDirectory() + @"\uploads\Zips\" + sessionID + @"\";

            if (Directory.Exists(tempLocation))
            {
                Directory.Delete(tempLocation,true);
            }
            var tempFolder = Directory.CreateDirectory(tempLocation);

            foreach (var file in Directory.GetFiles(path).Where(file => file.Split(@"\").Last().StartsWith(sessionID)).ToList())
            {
                System.IO.File.Copy(file, tempFolder.FullName + file.Split(@"\").Last());
            }


            //Create the zip folder
            string zipPath = Directory.GetCurrentDirectory() + @"\uploads\Zips\" + sessionID + ".zip";

            if (System.IO.File.Exists(zipPath))
            {
                System.IO.File.Delete(zipPath);
            }
            ZipFile.CreateFromDirectory(tempFolder.FullName, zipPath);


            //THe part below will package up a file to be sent through a byte stream
            var cd = new ContentDispositionHeaderValue("attachment")
            {
                FileNameStar = zipPath.Split(@"\").Last()
            };

            Response.Headers.Add(HeaderNames.ContentDisposition, cd.ToString());

            byte[] bytes = System.IO.File.ReadAllBytes(zipPath);

            using (FileStream fs = new FileStream(zipPath, FileMode.Open, FileAccess.Read))
            {
                fs.Read(bytes, 0, System.Convert.ToInt32(fs.Length));
                fs.Close();
            }


            return File(bytes, "application/zip");


        }
    }
}
