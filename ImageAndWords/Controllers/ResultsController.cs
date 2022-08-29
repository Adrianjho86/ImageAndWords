using ImageAndWords.Helpers;
using ImageAndWords.Models;
using System;
using System.Web.Mvc;

namespace ImageAndWords.Controllers
{
    public class ResultsController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Url input returns all images and words ranked by appearence in the HTML
        /// </summary>
        /// <param name="urlRequest"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Results(string urlRequest)
        {
            UrlResponse response = new UrlResponse();
            try
            {
                //URL validations
                ImagesHelper _imageHelper = new ImagesHelper();
                response = _imageHelper.UrlFormatValidation(urlRequest);
                if (response.IsValid)
                {
                    //Get images with HTMLAgilityPack
                    response.ImagesUrls = _imageHelper.GetImagesFromUrl(urlRequest, response.Scheme, response.HostPath);

                    // Get Words
                    WordsHelper _wordsHelper = new WordsHelper();
                    response.RankedWords = _wordsHelper.GetWordsFromUrl(urlRequest);
                }
                //ViewBag.Model = response;
                return View(response);
            }
            catch (Exception ex)
            {
                response.IsValid = false;
                response.NotValidMessage = String.Concat("Error has ocurred: " + ex.Message);
                return View(response);
            }

        }
    }
}