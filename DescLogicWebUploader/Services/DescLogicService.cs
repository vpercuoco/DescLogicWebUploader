using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FirstASPNETCOREProject.ViewModels;
using DescLogicFramework;
using System.IO;
using Microsoft.Extensions.Logging;

namespace FirstASPNETCOREProject
{
    public class DescLogicService : IDescLogicService
    {
        private readonly ILogger _logger;


        public async Task Convert(TestModel model)
        {

            await Task.Run(() =>
             {
                 _logger.LogInformation($"Starting a file conversion at: {DateTime.Now.ToString()} ");
                 FileSegregator descriptionFileCollection = new FileSegregator();
                 descriptionFileCollection.AddFiles(model.DescriptionImportDirectory, "*.csv");
                 descriptionFileCollection.ExportDirectory = model.DescriptionExportDirectory;
                 FileSegregator measurementFilesCollection = new FileSegregator();
                 measurementFilesCollection.AddFiles(model.MeasurementImportDirectory, "*.csv");
                 measurementFilesCollection.ExportDirectory = model.MeasurementExportDirectory;

                 ProgramWorkFlowHandler pwfh = new ProgramWorkFlowHandler(descriptionFileCollection, measurementFilesCollection);

                 _logger.LogInformation($"Completed a file conversion at: {DateTime.Now.ToString()} ");
             }
             ).ConfigureAwait(true);
            
        }

        public DescLogicService(ILogger<DescLogicService> logger)
        {
            _logger = logger;

        }
    }
}
