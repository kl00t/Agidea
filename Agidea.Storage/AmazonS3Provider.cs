using System;
using System.Configuration;
using System.IO;
using Agidea.Core.Interfaces;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;

namespace Agidea.Storage
{
    public class AmazonS3Provider : IFileStorageProvider
    {
        private static readonly string bucketName = ConfigurationManager.AppSettings["BucketName"];
        private const string folder = "C:\\temp";

        public void GetFile(string fileName)
        {
            GetObject(fileName);
        }

        public void ListFiles()
        {
            ListObjects();
        }

        public string GetFileUrl(string fileName)
        {
            return GetObjectUrl(fileName);
        }

        private static string GetObjectUrl(string fileName)
        {
            using (var s3Client = new AmazonS3Client(RegionEndpoint.EUWest1))
            {
                var request = new GetPreSignedUrlRequest
                {
                    BucketName = bucketName,
                    Key = fileName,
                    Expires = DateTime.Now.AddHours(1),
                    Protocol = Protocol.HTTP
                };

                return s3Client.GetPreSignedURL(request);
            }
        }

        private static void ListObjects()
        {
            using (var s3Client = new AmazonS3Client(RegionEndpoint.EUWest1))
            {
                try
                {
                    var listObjectsRequest = new ListObjectsRequest
                    {
                        BucketName = bucketName
                    };

                    var listObjectsResponse = s3Client.ListObjects(listObjectsRequest);
                    foreach (var entry in listObjectsResponse.S3Objects)
                    {
                        Console.WriteLine("Found object with key {0}, size {1}, last modification date {2}", entry.Key, entry.Size, entry.LastModified);
                    }
                }
                catch (AmazonS3Exception e)
                {
                    Console.WriteLine("Object listing has failed.");
                    Console.WriteLine("Amazon error code: {0}",
                        string.IsNullOrEmpty(e.ErrorCode) ? "None" : e.ErrorCode);
                    Console.WriteLine("Exception message: {0}", e.Message);
                }
            }
        }

        private static void GetObject(string fileName)
        {
            using (var s3Client = new AmazonS3Client(RegionEndpoint.EUWest1))
            {
                try
                {
                    var getObjectRequest = new GetObjectRequest
                    {
                        BucketName = bucketName,
                        Key = fileName
                    };

                    var getObjectResponse = s3Client.GetObject(getObjectRequest);
                    //var metadataCollection = getObjectResponse.Metadata;

                    getObjectResponse.WriteResponseStreamToFile(Path.Combine(folder, fileName), true);

                    //var keys = metadataCollection.Keys;
                    //foreach (var key in keys)
                    //{
                    //    Console.WriteLine("Metadata key: {0}, value: {1}", key, metadataCollection[key]);
                    //}

                    //using (var stream = getObjectResponse.ResponseStream)
                    //{
                    //    var length = stream.Length;
                    //    var bytes = new byte[length];
                    //    var bytesToRead = (int) length;
                    //    var numBytesRead = 0;
                    //    do
                    //    {
                    //        var chunkSize = 1000;
                    //        if (chunkSize > bytesToRead)
                    //        {
                    //            chunkSize = bytesToRead;
                    //        }

                    //        var n = stream.Read(bytes, numBytesRead, chunkSize);
                    //        numBytesRead += n;
                    //        bytesToRead -= n;
                    //    } while (bytesToRead > 0);

                    //    var contents = Encoding.UTF8.GetString(bytes);
                    //    Console.WriteLine(contents);
                    //}
                }
                catch (AmazonS3Exception e)
                {
                    Console.WriteLine("Object download has failed.");
                    Console.WriteLine("Amazon error code: {0}", string.IsNullOrEmpty(e.ErrorCode) ? "None" : e.ErrorCode);
                    Console.WriteLine("Exception message: {0}", e.Message);
                }
                catch (Exception exception)
                {
                    Console.WriteLine("Exception message: {0}", exception.Message);
                }
            }
        }
    }
}