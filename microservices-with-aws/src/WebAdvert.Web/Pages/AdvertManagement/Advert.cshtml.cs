using System;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebAdvert.Models;
using WebAdvert.Web.Models.AdvertManagement;
using WebAdvert.Web.Services.Clients;

namespace WebAdvert.Web.Pages.AdvertManagement
{
    public class AdvertModel : PageModel
    {
        private readonly IMapper _mapper;
        private readonly AdvertApiClient _advertClient;
        private readonly FilesApiClient _filesClient;

        public AdvertModel(IMapper mapper, AdvertApiClient advertClient, FilesApiClient filesClient)
        {
            _mapper = mapper;
            _advertClient = advertClient;
            _filesClient = filesClient;
        }

        [BindProperty]
        public CreateAdvertViewModel Input { get; set; }

        [BindProperty]
        public IFormFile ImageFile { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var isValid = ModelState.IsValid && await ExecuteAsync();
            if (isValid)
            {
                return RedirectToPage("/advertmanagement/list");
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private async Task<bool> ExecuteAsync()
        {
            var advertModel = _mapper.Map<AdvertViewModel>(Input);
            advertModel.UserName = User.Identity.Name;

            // create advert
            var result = await _advertClient.CreateAsync(advertModel);
            if (result.Object != null)
            {
                var advertId = result.Object.Id;
                string filePath = string.Empty;
                bool isFileUploadSuccessful = false;

                if (ImageFile != null)
                {
                    var fileName = !string.IsNullOrEmpty(ImageFile.FileName) ? Path.GetFileName(ImageFile.FileName) : advertId;
                    filePath = $"{advertId}/{fileName}";

                    try
                    {
                        isFileUploadSuccessful = await _filesClient.UploadAsync(ImageFile).ConfigureAwait(false);
                        if (!isFileUploadSuccessful)
                            throw new Exception(
                                "Could not upload the image to file repository. Please see the logs for details.");
                    }
                    catch (Exception e)
                    {
                        var confirmModel = new ConfirmAdvertViewModel()
                        {
                            Id = advertId,
                            FilePath = filePath,
                            Status = AdvertStatus.Pending
                        };

                        await _advertClient.ConfirmAsync(confirmModel).ConfigureAwait(false);
                        Console.WriteLine(e);
                        return false;
                    }
                }

                if (isFileUploadSuccessful)
                {
                    var confirmModel = new ConfirmAdvertViewModel()
                    {
                        Id = advertId,
                        FilePath = filePath,
                        Status = AdvertStatus.Active
                    };

                    await _advertClient.ConfirmAsync(confirmModel).ConfigureAwait(false);
                }
            }

            //foreach (var error in result.Errors)
            //{
            //    var propName = error.Contains("code", StringComparison.OrdinalIgnoreCase) ? "Code" : "Email";
            //    ModelState.AddModelError($"Input.{propName}", error);
            //}

            return true;
        }
    }
}