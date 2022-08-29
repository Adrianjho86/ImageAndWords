using ImageAndWords.Models;
using System;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace ImageAndWords.Helpers
{
    public class ImagesHelper
    {
        //Validates the Url format and Http 
        public UrlResponse UrlFormatValidation(string testUrl)
        {
            UrlResponse statusResponse = new UrlResponse();
            try
            {
                HttpWebRequest httpWebRequest = WebRequest.Create(testUrl) as HttpWebRequest;

                httpWebRequest.Method = "HEAD";
                //httpWebRequest.AllowAutoRedirect = false;
                HttpWebResponse httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse;

                Uri uriResult;
                bool uriFormatResult = Uri.TryCreate(testUrl, UriKind.Absolute, out uriResult)
                    && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps)
                    && !uriResult.IsFile;

                if (uriFormatResult && httpWebResponse.StatusCode==HttpStatusCode.OK)
                {
                    statusResponse.IsValid = true;
                    statusResponse.Scheme = uriResult.Scheme;
                    statusResponse.HostPath = uriResult.Host;
                }
                else
                {
                    if (!uriFormatResult)
                    {
                        statusResponse.IsValid = false;
                        statusResponse.NotValidMessage = "Invalid URL format";
                    }
                    if (httpWebResponse.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        statusResponse.IsValid = false;
                        statusResponse.NotValidMessage = "Your are not authorized";
                    }
                    if (httpWebResponse.StatusCode == HttpStatusCode.Redirect || (int)httpWebResponse.StatusCode == 307)
                    {
                        statusResponse.IsValid = false;
                        statusResponse.NotValidMessage = "Url has redirections";
                    }
                }
                return statusResponse;
            }
            catch (WebException wex)
            {
                statusResponse.IsValid = false;
                statusResponse.NotValidMessage = wex.Message;
                return statusResponse;
            }
        }

        /// <summary>
        /// Get Images from the Url validated
        /// scheme and hostPath are used to create the full path for relatives path sources
        /// </summary>
        /// <param name="validUrl"></param>
        /// <param name="scheme"></param>
        /// <param name="hostPath"></param>
        /// <returns></returns>
        public IEnumerable<string> GetImagesFromUrl(string validUrl, string scheme, string hostPath)
        {
            try
            {
                var document = new HtmlWeb().Load(validUrl);
                IEnumerable<string> imagesUrl = document.DocumentNode.Descendants("img")
                    .Select(x => x.GetAttributeValue("src", null))
                    .Where(s => !String.IsNullOrEmpty(s));

                List<string> imagesAbsolutePath = new List<string>();
                foreach (string url in imagesUrl)
                {
                    if (url.StartsWith("/"))
                    {
                        imagesAbsolutePath.Add(String.Concat(scheme, "://", hostPath, url));
                    }
                    else
                    {
                        imagesAbsolutePath.Add(url);
                    };
                }
                imagesAbsolutePath.RemoveAll(x => !ImageUrlExist(x));

                return imagesAbsolutePath;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }  

        //Check if the url image exist to avoid broken images in the gallery
        private bool ImageUrlExist(string imageUrl)
        {
            try
            {
                HttpWebRequest httpWebRequest = WebRequest.Create(imageUrl) as HttpWebRequest;
                httpWebRequest.Method = "HEAD";
                //httpWebRequest.AllowAutoRedirect = false;
                HttpWebResponse httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse;
                return httpWebResponse.StatusCode == HttpStatusCode.OK;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}