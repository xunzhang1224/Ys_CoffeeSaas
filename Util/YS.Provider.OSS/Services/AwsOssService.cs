namespace YS.Provider.OSS.Services
{
    using Amazon;
    using Amazon.S3;
    using Amazon.S3.Model;
    using Microsoft.Extensions.Logging;
    using System.Net;
    using YS.Provider.OSS.Enum;
    using YS.Provider.OSS.Interface;
    using YS.Provider.OSS.Interface.Base;
    using YS.Provider.OSS.Model;

    /// <summary>
    /// AwsOssService
    /// </summary>
    public class AwsOssService : BaseOSSService, IAwsOssService
    {
        private readonly ILogger<AwsOssService> _logger;
        private readonly AmazonS3Config amazonS3Config;
        private readonly AmazonS3Client _s3Client;
        private readonly OSSOptions _options;

        /// <summary>
        /// AwsOssService
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="options"></param>
        /// <param name="logger"></param>
        public AwsOssService(ICacheProvider cache, OSSOptions options, ILoggerFactory logger) : base(cache, options)
        {
            _options = options;
            amazonS3Config = new Amazon.S3.AmazonS3Config()
            {
                ServiceURL = $"https://{_options.Endpoint}",
                SignatureVersion = "2",
                SignatureMethod = Amazon.Runtime.SigningAlgorithm.HmacSHA1,
            };
            _s3Client = new AmazonS3Client(options.AccessKey, options.SecretKey, amazonS3Config);
            _logger = logger.CreateLogger<AwsOssService>();
        }

        /// <summary>
        /// BucketExistsAsync
        /// </summary>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        public async Task<bool> BucketExistsAsync(string bucketName)
        {
            try
            {
                // 列出所有存储桶
                var response = await _s3Client.ListBucketsAsync();
                var bucket = response.Buckets.FirstOrDefault(b => string.Equals(b.BucketName, bucketName, StringComparison.OrdinalIgnoreCase));
                return bucket != null;
            }
            catch (AmazonS3Exception e)
            {
                // S3异常处理
                Console.WriteLine($"Error: {e.Message}");
                return false;
            }
            catch (Exception ex)
            {
                // 其他异常处理
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// CopyObjectAsync
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="objectName"></param>
        /// <param name="destBucketName"></param>
        /// <param name="destObjectName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<bool> CopyObjectAsync(string bucketName, string objectName, string destBucketName, string destObjectName = null)
        {
            if (string.IsNullOrEmpty(bucketName))
            {
                throw new ArgumentNullException(nameof(bucketName));
            }
            objectName = FormatObjectName(objectName);
            if (string.IsNullOrEmpty(destBucketName))
            {
                destBucketName = bucketName;
            }
            destObjectName = FormatObjectName(destObjectName);
            var response = new CopyObjectResponse();
            try
            {
                var request = new CopyObjectRequest
                {
                    SourceBucket = bucketName,
                    SourceKey = objectName,
                    DestinationBucket = destBucketName,
                    DestinationKey = destObjectName,
                };
                response = await _s3Client.CopyObjectAsync(request);
            }
            catch (AmazonS3Exception ex)
            {
                Console.WriteLine($"Error copying object: '{ex.Message}'");
                return false;
            }
            return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
        }

        /// <summary>
        /// CreateBucketAsync
        /// </summary>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<bool> CreateBucketAsync(string bucketName)
        {
            if (string.IsNullOrEmpty(bucketName))
            {
                throw new ArgumentNullException(nameof(bucketName));
            }
            try
            {
                var request = new PutBucketRequest
                {
                    BucketName = bucketName,
                    UseClientRegion = true,
                };

                var response = await _s3Client.PutBucketAsync(request);
                return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (AmazonS3Exception ex)
            {
                Console.WriteLine($"Error creating bucket: '{ex.Message}'");
                return false;
            }
        }

        /// <summary>
        /// GetObjectAclAsync
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="objectName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<AccessModeEnum> GetObjectAclAsync(string bucketName, string objectName)
        {
            if (string.IsNullOrEmpty(bucketName))
            {
                throw new ArgumentNullException(nameof(bucketName));
            }
            objectName = FormatObjectName(objectName);

            try
            {
                var request = new GetACLRequest
                {
                    BucketName = bucketName,
                    Key = objectName
                };

                var result = await _s3Client.GetACLAsync(request);

                List<S3Permission> permissions = new List<S3Permission>();
                foreach (var item in result.AccessControlList.Grants)
                {
                    if (item.Permission != null)
                    {
                        permissions.Add(item.Permission);
                    }
                }

                if (permissions.Count > 0)
                {
                    if (permissions.Any(p => p == S3Permission.FULL_CONTROL)
                        || permissions.Any(p => p == S3Permission.WRITE)
                        || (permissions.Any(p => p == S3Permission.READ) && permissions.Any(p => p == S3Permission.WRITE)))
                    {
                        return AccessModeEnum.PublicReadWrite;
                    }
                    else if (permissions.Any(p => p == S3Permission.READ))
                    {
                        return AccessModeEnum.PublicRead;
                    }
                }
                return AccessModeEnum.Private;
            }
            catch (AmazonS3Exception ex)
            {
                Console.WriteLine($"Error creating bucket: '{ex.Message}'");
                return AccessModeEnum.Private;
            }
        }

        /// <summary>
        /// GetObjectAsync
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="objectName"></param>
        /// <param name="callback"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task GetObjectAsync(string bucketName, string objectName, Action<Stream> callback, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(bucketName))
            {
                throw new ArgumentNullException(nameof(bucketName));
            }
            objectName = FormatObjectName(objectName);
            GetObjectRequest request = new GetObjectRequest()
            {
                BucketName = bucketName,
                Key = objectName
            };
            using (var response = await _s3Client.GetObjectAsync(request))
            {
                callback(response.ResponseStream);
            }
        }

        /// <summary>
        /// GetObjectAsync
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="objectName"></param>
        /// <param name="fileName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task GetObjectAsync(string bucketName, string objectName, string fileName, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(bucketName))
            {
                throw new ArgumentNullException(nameof(bucketName));
            }
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            string fullPath = Path.GetFullPath(fileName);
            string parentPath = Path.GetDirectoryName(fullPath);
            if (!string.IsNullOrEmpty(parentPath) && !Directory.Exists(parentPath))
            {
                Directory.CreateDirectory(parentPath);
            }
            objectName = FormatObjectName(objectName);

            GetObjectRequest request = new GetObjectRequest()
            {
                BucketName = bucketName,
                Key = objectName
            };

            using (var response = await _s3Client.GetObjectAsync(request))
            using (var fileStream = File.Create(parentPath))
            {
                // 将对象数据复制到本地文件流
                await response.ResponseStream.CopyToAsync(fileStream);
            }
        }

        /// <summary>
        /// GetObjectMetadataAsync
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="objectName"></param>
        /// <param name="versionID"></param>
        /// <param name="matchEtag"></param>
        /// <param name="modifiedSince"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<ItemMeta> GetObjectMetadataAsync(string bucketName, string objectName, string versionID = null, string matchEtag = null, DateTime? modifiedSince = null)
        {
            if (string.IsNullOrEmpty(bucketName))
            {
                throw new ArgumentNullException(nameof(bucketName));
            }
            objectName = FormatObjectName(objectName);

            GetObjectMetadataRequest request = new GetObjectMetadataRequest()
            {
                BucketName = bucketName,
                Key = objectName,
                VersionId = versionID,

            };
            GetObjectMetadataResponse response = await _s3Client.GetObjectMetadataAsync(request);
            var newMeta = new ItemMeta()
            {
                ObjectName = objectName,
                ContentType = "",
                Size = response.ContentLength,
                LastModified = response.LastModified,
                ETag = response.ETag,
                IsEnableHttps = Options.IsEnableHttps,
                MetaData = new Dictionary<string, string>(),
            };
            if (response.Metadata != null && response.Metadata.Count > 0)
            {
                foreach (var item in response.Metadata.Keys)
                {
                    newMeta.MetaData.Add(item, response.Metadata[item]);
                }
            }

            return newMeta;
        }

        /// <summary>
        /// ListBucketsAsync
        /// </summary>
        /// <returns></returns>
        public async Task<List<Bucket>> ListBucketsAsync()
        {
            ListBucketsResponse response = await _s3Client.ListBucketsAsync();
            var buckets = response.Buckets;
            if (buckets == null)
            {
                return null;
            }
            if (buckets.Count() == 0)
            {
                return null;
            }
            var resultList = new List<Bucket>();
            foreach (var item in buckets)
            {
                resultList.Add(new Bucket()
                {
                    Location = "",
                    Name = item.BucketName,
                    CreationDate = item.CreationDate.ToString("yyyy-MM-dd HH:mm:ss"),
                });
            }
            return resultList;
        }

        /// <summary>
        /// ListObjectsAsync
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<List<Item>> ListObjectsAsync(string bucketName, string prefix = null)
        {
            if (string.IsNullOrEmpty(bucketName))
            {
                throw new ArgumentNullException(nameof(bucketName));
            }

            try
            {
                List<Item> result = new List<Item>();
                var request = new ListObjectsV2Request
                {
                    BucketName = bucketName,
                    Prefix = prefix,
                    MaxKeys = 100,
                };
                ListObjectsV2Response response;
                do
                {
                    response = await _s3Client.ListObjectsV2Async(request);

                    response.S3Objects
                        .ForEach(item => result.Add(new Item()
                        {
                            Key = item.Key,
                            LastModified = item.LastModified.ToString(),
                            ETag = item.ETag,
                            Size = (ulong)item.Size,
                            BucketName = bucketName,
                            IsDir = !string.IsNullOrWhiteSpace(item.Key) && item.Key[^1] == '/',
                            LastModifiedDateTime = item.LastModified,
                        }));

                    // If the response is truncated, set the request ContinuationToken
                    // from the NextContinuationToken property of the response.
                    request.ContinuationToken = response.NextContinuationToken;
                }
                while (response.IsTruncated);

                return result;
            }
            catch (AmazonS3Exception ex)
            {
                Console.WriteLine($"Error encountered on server. Message:'{ex.Message}' getting list of objects.");
                // return false;
            }
            return null;
        }

        /// <summary>
        /// ObjectsExistsAsync
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="objectName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<bool> ObjectsExistsAsync(string bucketName, string objectName)
        {
            if (string.IsNullOrEmpty(bucketName))
            {
                throw new ArgumentNullException(nameof(bucketName));
            }
            objectName = FormatObjectName(objectName);
            var response = await _s3Client.GetObjectMetadataAsync(bucketName, objectName);
            return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
        }

        /// <summary>
        /// PresignedGetObjectAsync
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="objectName"></param>
        /// <param name="expiresInt"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Task<string> PresignedGetObjectAsync(string bucketName, string objectName, int expiresInt)
        {
            if (string.IsNullOrEmpty(bucketName))
            {
                throw new ArgumentNullException(nameof(bucketName));
            }

            return PresignedObjectAsync(bucketName
                , objectName
                , expiresInt
                , PresignedObjectTypeEnum.Get
                , async (bucketName, objectName, expiresInt) =>
                {
                    string objectUrl = null;
                    AccessModeEnum accessMode = await this.GetObjectAclAsync(bucketName, objectName);
                    if (accessMode == AccessModeEnum.PublicRead || accessMode == AccessModeEnum.PublicReadWrite)
                    {
                        // objectUrl = $"{(Options.IsEnableHttps ? "https" : "http")}://{bucketName}.{Options.Endpoint}{(objectName.StartsWith("/") ? "" : "/")}{objectName}";
                        objectUrl = GetCdnUrl(bucketName, objectName);
                        return objectUrl;
                    }
                    var request = new GetPreSignedUrlRequest()
                    {
                        BucketName = bucketName,
                        Expires = DateTime.UtcNow.AddSeconds(expiresInt),
                        Key = objectName,
                        Verb = HttpVerb.GET,
                    };
                    AWSConfigsS3.UseSignatureVersion4 = true;
                    string url = await _s3Client.GetPreSignedURLAsync(request);
                    return GetCdnUrl(bucketName, objectName);
                });
        }

        /// <summary>
        /// PresignedPutObjectAsync
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="objectName"></param>
        /// <param name="expiresInt"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Task<string> PresignedPutObjectAsync(string bucketName, string objectName, int expiresInt)
        {
            if (string.IsNullOrEmpty(bucketName))
            {
                throw new ArgumentNullException(nameof(bucketName));
            }
            return PresignedObjectAsync(bucketName
                , objectName
                , expiresInt
                , PresignedObjectTypeEnum.Put
                , async (bucketName, objectName, expiresInt) =>
                {
                    var request = new GetPreSignedUrlRequest()
                    {
                        BucketName = bucketName,
                        Expires = DateTime.UtcNow.AddSeconds(expiresInt),
                        Key = objectName,
                        Verb = HttpVerb.PUT,
                    };
                    AWSConfigsS3.UseSignatureVersion4 = true;
                    string url = await _s3Client.GetPreSignedURLAsync(request);
                    return url;
                });
        }

        /// <summary>
        /// PutObjectAsync
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="objectName"></param>
        /// <param name="data"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<bool> PutObjectAsync(string bucketName, string objectName, Stream data, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(bucketName))
            {
                throw new ArgumentNullException(nameof(bucketName));
            }

            objectName = FormatObjectName(objectName);

            PutObjectRequest request = new PutObjectRequest()
            {
                BucketName = bucketName,
                Key = objectName,
                InputStream = data,
                CannedACL = S3CannedACL.PublicRead,
            };
            PutObjectResponse response = await _s3Client.PutObjectAsync(request);
            return response.HttpStatusCode == HttpStatusCode.OK;
        }

        /// <summary>
        /// PutObjectAsync
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="objectName"></param>
        /// <param name="filePath"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="Exception"></exception>
        public async Task<bool> PutObjectAsync(string bucketName, string objectName, string filePath, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(bucketName))
            {
                throw new ArgumentNullException(nameof(bucketName));
            }
            objectName = FormatObjectName(objectName);
            if (!File.Exists(filePath))
            {
                throw new Exception("Upload file is not exist.");
            }

            PutObjectRequest request = new PutObjectRequest()
            {
                BucketName = bucketName,
                Key = objectName,
                FilePath = filePath,
                CannedACL = S3CannedACL.PublicRead,
            };
            PutObjectResponse response = await _s3Client.PutObjectAsync(request);
            return response.HttpStatusCode == HttpStatusCode.OK;
        }

        /// <summary>
        /// RemoveBucketAsync
        /// </summary>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<bool> RemoveBucketAsync(string bucketName)
        {
            if (string.IsNullOrEmpty(bucketName))
            {
                throw new ArgumentNullException(nameof(bucketName));
            }
            DeleteBucketRequest request = new DeleteBucketRequest
            {
                BucketName = bucketName
            };
            DeleteBucketResponse response = await _s3Client.DeleteBucketAsync(request);
            return (response != null && (response.HttpStatusCode == HttpStatusCode.NoContent || response.HttpStatusCode == HttpStatusCode.OK));
        }

        /// <summary>
        /// RemoveObjectAclAsync
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="objectName"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<AccessModeEnum> RemoveObjectAclAsync(string bucketName, string objectName)
        {
            objectName = FormatObjectName(objectName);
            if (!await SetObjectAclAsync(bucketName, objectName, AccessModeEnum.Default))
            {
                throw new Exception("Save new policy info failed when remove object acl.");
            }
            return await GetObjectAclAsync(bucketName, objectName);
        }

        /// <summary>
        /// RemoveObjectAsync
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="objectName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<bool> RemoveObjectAsync(string bucketName, string objectName)
        {
            if (string.IsNullOrEmpty(bucketName))
            {
                throw new ArgumentNullException(nameof(bucketName));
            }
            objectName = FormatObjectName(objectName);
            DeleteObjectRequest request = new DeleteObjectRequest()
            {
                BucketName = bucketName,
                Key = objectName
            };
            DeleteObjectResponse response = await _s3Client.DeleteObjectAsync(request);
            return (response != null && (response.HttpStatusCode == HttpStatusCode.NoContent || response.HttpStatusCode == HttpStatusCode.OK));
        }

        /// <summary>
        /// RemoveObjectAsync
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="objectNames"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> RemoveObjectAsync(string bucketName, List<string> objectNames)
        {
            DeleteObjectsRequest request = new DeleteObjectsRequest
            {
                BucketName = bucketName,
                Quiet = false
            };
            foreach (var item in objectNames)
            {
                request.AddKey(FormatObjectName(item));
            }
            DeleteObjectsResponse response = await _s3Client.DeleteObjectsAsync(request);
            if (response.DeletedObjects != null && response.DeletedObjects.Count == objectNames.Count)
            {
                return true;
            }
            else
            {
                if (response.DeletedObjects != null && response.DeletedObjects.Count > 0)
                {
                    throw new Exception($"Some file delete failed. files {string.Join(",", response.DeletedObjects.Select(p => p.Key))}");
                }
                else
                {
                    throw new Exception($"Some file delete failed.");
                }
            }
        }

        /// <summary>
        /// SetObjectAclAsync
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="objectName"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="Exception"></exception>
        public async Task<bool> SetObjectAclAsync(string bucketName, string objectName, AccessModeEnum mode)
        {
            if (string.IsNullOrEmpty(bucketName))
            {
                throw new ArgumentNullException(nameof(bucketName));
            }
            objectName = FormatObjectName(objectName);
            if (!await this.ObjectsExistsAsync(bucketName, objectName))
            {
                throw new Exception($"Object '{objectName}' not in bucket '{bucketName}'.");
            }
            var canned = mode switch
            {
                AccessModeEnum.Default => S3CannedACL.Private,
                AccessModeEnum.Private => S3CannedACL.Private,
                AccessModeEnum.PublicRead => S3CannedACL.PublicRead,
                AccessModeEnum.PublicReadWrite => S3CannedACL.PublicReadWrite,
                _ => S3CannedACL.Private,
            };
            PutACLRequest request = new PutACLRequest
            {
                BucketName = bucketName,
                Key = objectName,
                CannedACL = canned
            };
            var response = await _s3Client.PutACLAsync(request);
            return response != null && response.HttpStatusCode == HttpStatusCode.OK;
        }
    }
}
