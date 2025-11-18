namespace YS.Provider.OSS.Services
{
    using Aliyun.OSS;
    using YS.Provider.OSS.Enum;
    using YS.Provider.OSS.Interface;
    using YS.Provider.OSS.Interface.Base;
    using YS.Provider.OSS.Model;
    using Bucket = YS.Provider.OSS.Model.Bucket;
    using Owner = YS.Provider.OSS.Model.Owner;

    /// <summary>
    /// AliyunOSSService
    /// </summary>
    public class AliyunOSSService : BaseOSSService, IAliyunOSSService
    {
        private readonly OssClient _client = null;

        // 字典用于映射文件扩展名到 MIME 类型
        private static readonly Dictionary<string, string> MimeTypes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { ".jpg", "image/jpeg" },
            { ".jpeg", "image/jpeg" },
            { ".png", "image/png" },
            { ".gif", "image/gif" },
            { ".bmp", "image/bmp" },
            { ".webp", "image/webp" },
            { ".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" },
            { ".xls", "application/vnd.ms-excel" },
            { ".csv", "text/csv" },
            { ".apk", "application/vnd.android.package-archive" },
            { ".mp4", "video/mp4" },
            { ".bin", "application/octet-stream" } // 新增
        };

        /// <summary>
        /// Context
        /// </summary>
        public OssClient Context
        {
            get
            {
                return this._client;
            }
        }

        /// <summary>
        /// AliyunOSSService
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="options"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public AliyunOSSService(ICacheProvider cache
            , OSSOptions options) : base(cache, options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options), "The OSSOptions can not null");
            this._client = new OssClient(options.Endpoint, options.AccessKey, options.SecretKey);
        }

        #region Bucket

        /// <summary>
        /// ListBucketsAsync
        /// </summary>
        /// <returns></returns>
        public Task<List<Bucket>> ListBucketsAsync()
        {
            IEnumerable<Aliyun.OSS.Bucket> buckets = _client.ListBuckets();
            if (buckets == null)
            {
                return null;
            }
            if (buckets.Count() == 0)
            {
                return Task.FromResult(new List<Bucket>());
            }
            var resultList = new List<Bucket>();
            foreach (var item in buckets)
            {
                resultList.Add(new Bucket()
                {
                    Location = item.Location,
                    Name = item.Name,
                    Owner = new Owner()
                    {
                        Name = item.Owner.DisplayName,
                        Id = item.Owner.Id
                    },
                    CreationDate = item.CreationDate.ToString("yyyy-MM-dd HH:mm:ss"),
                });
            }
            return Task.FromResult(resultList);
        }

        /// <summary>
        /// BucketExistsAsync
        /// </summary>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Task<bool> BucketExistsAsync(string bucketName)
        {
            if (string.IsNullOrEmpty(bucketName))
            {
                throw new ArgumentNullException(nameof(bucketName));
            }
            return Task.FromResult(_client.DoesBucketExist(bucketName));
        }

        /// <summary>
        /// CreateBucketAsync
        /// </summary>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="Exception"></exception>
        public Task<bool> CreateBucketAsync(string bucketName)
        {
            if (string.IsNullOrEmpty(bucketName))
            {
                throw new ArgumentNullException(nameof(bucketName));
            }

            // 检查桶是否存在
            Bucket bucket = ListBucketsAsync().Result?.Where(p => p.Name == bucketName)?.FirstOrDefault();
            if (bucket != null)
            {
                string localtion = Options.Endpoint?.Split('.')[0];
                if (bucket.Location.Equals(localtion, StringComparison.OrdinalIgnoreCase))
                {
                    throw new Exception($"Bucket '{bucketName}' already exists.");
                }
                else
                {
                    throw new Exception($"There have a same name bucket '{bucketName}' in other localtion '{bucket.Location}'.");
                }
            }

            var request = new CreateBucketRequest(bucketName)
            {
                // 设置存储空间访问权限ACL。
                ACL = CannedAccessControlList.Private,
                // 设置数据容灾类型。
                DataRedundancyType = DataRedundancyType.LRS
            };

            // 创建存储空间。
            var result = _client.CreateBucket(request);
            return Task.FromResult(result != null);
        }

        /// <summary>
        /// RemoveBucketAsync
        /// </summary>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Task<bool> RemoveBucketAsync(string bucketName)
        {
            if (string.IsNullOrEmpty(bucketName))
            {
                throw new ArgumentNullException(nameof(bucketName));
            }

            _client.DeleteBucket(bucketName);
            return Task.FromResult(true);
        }

        /// <summary>
        /// GetBucketLocationAsync
        /// </summary>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Task<string> GetBucketLocationAsync(string bucketName)
        {
            if (string.IsNullOrEmpty(bucketName))
            {
                throw new ArgumentNullException(nameof(bucketName));
            }

            var result = _client.GetBucketLocation(bucketName);

            if (result == null)
            {
                return null;
            }

            return Task.FromResult(result.Location);
        }

        /// <summary>
        /// GetBucketEndpointAsync
        /// </summary>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        public Task<string> GetBucketEndpointAsync(string bucketName)
        {
            var result = _client.GetBucketInfo(bucketName);
            if (result != null
                && result.Bucket != null
                && !string.IsNullOrEmpty(result.Bucket.Name)
                && !string.IsNullOrEmpty(result.Bucket.ExtranetEndpoint))
            {
                string host = $"{(Options.IsEnableHttps ? "https://" : "http://")}{result.Bucket.Name}.{result.Bucket.ExtranetEndpoint}";
                return Task.FromResult(host);
            }

            return Task.FromResult(string.Empty);
        }

        /// <summary>
        /// 设置储存桶的访问权限
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="objectName"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public Task<bool> SetBucketAclAsync(string bucketName, AccessModeEnum mode)
        {
            if (string.IsNullOrEmpty(bucketName))
            {
                throw new ArgumentNullException(nameof(bucketName));
            }

            var canned = mode switch
            {
                AccessModeEnum.Default => CannedAccessControlList.Default,
                AccessModeEnum.Private => CannedAccessControlList.Private,
                AccessModeEnum.PublicRead => CannedAccessControlList.PublicRead,
                AccessModeEnum.PublicReadWrite => CannedAccessControlList.PublicReadWrite,
                _ => CannedAccessControlList.Default,
            };

            var request = new SetBucketAclRequest(bucketName, canned);
            _client.SetBucketAcl(request);
            return Task.FromResult(true);
        }

        /// <summary>
        /// 获取储存桶的访问权限
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="objectName"></param>
        /// <returns></returns>
        public Task<AccessModeEnum> GetBucketAclAsync(string bucketName)
        {
            if (string.IsNullOrEmpty(bucketName))
            {
                throw new ArgumentNullException(nameof(bucketName));
            }

            var result = _client.GetBucketAcl(bucketName);
            var mode = result.ACL switch
            {
                CannedAccessControlList.Default => AccessModeEnum.Default,
                CannedAccessControlList.Private => AccessModeEnum.Private,
                CannedAccessControlList.PublicRead => AccessModeEnum.PublicRead,
                CannedAccessControlList.PublicReadWrite => AccessModeEnum.PublicReadWrite,
                _ => AccessModeEnum.Default,
            };
            return Task.FromResult(mode);
        }

        #endregion

        #region Object

        /// <summary>
        /// GetObjectAsync
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="objectName"></param>
        /// <param name="callback"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Task GetObjectAsync(string bucketName, string objectName, Action<Stream> callback, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(bucketName))
            {
                throw new ArgumentNullException(nameof(bucketName));
            }
            objectName = FormatObjectName(objectName);
            var obj = _client.GetObject(bucketName, objectName);
            callback(obj.Content);
            return Task.CompletedTask;
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
        public Task GetObjectAsync(string bucketName, string objectName, string fileName, CancellationToken cancellationToken = default)
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
            return GetObjectAsync(bucketName, objectName, (stream) =>
            {
                using (FileStream fs = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
                {
                    stream.CopyTo(fs);
                    stream.Dispose();
                    fs.Close();
                }
            }, cancellationToken);
        }

        /// <summary>
        /// ListObjectsAsync
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Task<List<Item>> ListObjectsAsync(string bucketName, string prefix = null)
        {
            if (string.IsNullOrEmpty(bucketName))
            {
                throw new ArgumentNullException(nameof(bucketName));
            }
            List<Item> result = new List<Item>();
            ObjectListing resultObj = null;
            string nextMarker = string.Empty;
            do
            {
                // 每页列举的文件个数通过maxKeys指定，超过指定数将进行分页显示。
                var listObjectsRequest = new ListObjectsRequest(bucketName)
                {
                    Marker = nextMarker,
                    MaxKeys = 100,
                    Prefix = prefix,
                };
                resultObj = _client.ListObjects(listObjectsRequest);
                if (resultObj == null)
                {
                    continue;
                }
                foreach (var item in resultObj.ObjectSummaries)
                {
                    result.Add(new Item()
                    {
                        Key = item.Key,
                        LastModified = item.LastModified.ToString(),
                        ETag = item.ETag,
                        Size = (ulong)item.Size,
                        BucketName = bucketName,
                        IsDir = !string.IsNullOrWhiteSpace(item.Key) && item.Key[^1] == '/',
                        LastModifiedDateTime = item.LastModified
                    });
                }
                nextMarker = resultObj.NextMarker;
            } while (resultObj.IsTruncated);
            return Task.FromResult(result);
        }

        /// <summary>
        /// ObjectsExistsAsync
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="objectName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Task<bool> ObjectsExistsAsync(string bucketName, string objectName)
        {
            if (string.IsNullOrEmpty(bucketName))
            {
                throw new ArgumentNullException(nameof(bucketName));
            }
            objectName = FormatObjectName(objectName);
            return Task.FromResult(_client.DoesObjectExist(bucketName, objectName));
        }

        /// <summary>
        /// PresignedGetObjectAsync
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="objectName"></param>
        /// <param name="expiresInt"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Task<string> PresignedGetObjectAsync(string bucketName, string objectName, int expiresInt)
        {
            var extension = Path.GetExtension(objectName).ToLower();

            // 查找字典，返回对应的 MIME 类型
            MimeTypes.TryGetValue(extension, out var contentType);
            if (string.IsNullOrWhiteSpace(contentType))
                throw new Exception($"暂不支持{extension}格式的文件上传");
            return PresignedObjectAsync(bucketName
                , objectName
                , expiresInt
                , PresignedObjectTypeEnum.Get
                , async (bucketName, objectName, expiresInt) =>
                {
                    objectName = FormatObjectName(objectName);

                    // 生成URL
                    AccessModeEnum accessMode = await this.GetObjectAclAsync(bucketName, objectName);
                    if (accessMode == AccessModeEnum.PublicRead || accessMode == AccessModeEnum.PublicReadWrite)
                    {
                        string bucketUrl = await this.GetBucketEndpointAsync(bucketName);
                        string uri = $"{bucketUrl}{(objectName.StartsWith("/") ? "" : "/")}{objectName}";
                        return uri;
                    }
                    else
                    {
                        var req = new GeneratePresignedUriRequest(bucketName, objectName, SignHttpMethod.Get)
                        {
                            Expiration = DateTime.Now.AddSeconds(expiresInt),
                            ContentType = contentType
                        };
                        var uri = _client.GeneratePresignedUri(req);
                        if (uri == null)
                        {
                            throw new Exception("Generate get presigned uri failed");
                        }
                        return uri.ToString();
                    }
                });
        }

        /// <summary>
        /// PresignedPutObjectAsync
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="objectName"></param>
        /// <param name="expiresInt"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Task<string> PresignedPutObjectAsync(string bucketName, string objectName, int expiresInt)
        {
            var extension = Path.GetExtension(objectName).ToLower();
            // 查找字典，返回对应的 MIME 类型
            MimeTypes.TryGetValue(extension, out var contentType);
            if (string.IsNullOrWhiteSpace(contentType))
                throw new Exception($"暂不支持{extension}格式的文件上传");
            return PresignedObjectAsync(bucketName
                , objectName
                , expiresInt
                , PresignedObjectTypeEnum.Put
                , (bucketName, objectName, expiresInt) =>
                {
                    objectName = FormatObjectName(objectName);
                    var req = new GeneratePresignedUriRequest(bucketName, objectName, SignHttpMethod.Put)
                    {
                        Expiration = DateTime.Now.AddSeconds(expiresInt),
                        ContentType = contentType,
                    };
                    var uri = _client.GeneratePresignedUri(req);
                    if (uri == null)
                    {
                        throw new Exception("Generate put presigned uri failed");
                    }

                    var url = uri.ToString();
                    if (url.IndexOf("http://") == 0 && Options.IsEnableHttps)
                    {
                        url = url.Replace("http://", "https://");
                    }

                    return Task.FromResult(url);
                    //return Task.FromResult(uri.ToString());
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
        public Task<bool> PutObjectAsync(string bucketName, string objectName, Stream data, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(bucketName))
            {
                throw new ArgumentNullException(nameof(bucketName));
            }
            objectName = FormatObjectName(objectName);
            var result = _client.PutObject(bucketName, objectName, data);
            if (result != null)
            {
                return Task.FromResult(true);
            }
            else
            {
                return Task.FromResult(false);
            }
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
        public Task<bool> PutObjectAsync(string bucketName, string objectName, string filePath, CancellationToken cancellationToken = default)
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
            var result = _client.PutObject(bucketName, objectName, filePath);
            if (result != null)
            {
                return Task.FromResult(true);
            }
            else
            {
                return Task.FromResult(false);
            }
        }

        /// <summary>
        /// 文件拷贝，默认采用分片拷贝的方式
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="objectName"></param>
        /// <param name="destBucketName"></param>
        /// <param name="destObjectName"></param>
        /// <returns></returns>
        public Task<bool> CopyObjectAsync(string bucketName, string objectName, string destBucketName = null, string destObjectName = null)
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
            var partSize = 50 * 1024 * 1024;

            // 创建OssClient实例。
            // 初始化拷贝任务。可以通过InitiateMultipartUploadRequest指定目标文件元信息。
            var request = new InitiateMultipartUploadRequest(destBucketName, destObjectName);
            var result = _client.InitiateMultipartUpload(request);

            // 计算分片数。
            var metadata = _client.GetObjectMetadata(bucketName, objectName);
            var fileSize = metadata.ContentLength;
            var partCount = (int)fileSize / partSize;
            if (fileSize % partSize != 0)
            {
                partCount++;
            }

            // 开始分片拷贝。
            var partETags = new List<PartETag>();
            for (var i = 0; i < partCount; i++)
            {
                var skipBytes = (long)partSize * i;
                var size = (partSize < fileSize - skipBytes) ? partSize : (fileSize - skipBytes);
                // 创建UploadPartCopyRequest。可以通过UploadPartCopyRequest指定限定条件。
                var uploadPartCopyRequest = new UploadPartCopyRequest(destBucketName, destObjectName, bucketName, objectName, result.UploadId)
                {
                    PartSize = size,
                    PartNumber = i + 1,
                    // BeginIndex用来定位此次上传分片开始所对应的位置。
                    BeginIndex = skipBytes
                };
                // 调用uploadPartCopy方法来拷贝每一个分片。
                var uploadPartCopyResult = _client.UploadPartCopy(uploadPartCopyRequest);
                partETags.Add(uploadPartCopyResult.PartETag);
            }

            // 完成分片拷贝。
            var completeMultipartUploadRequest =
            new CompleteMultipartUploadRequest(destBucketName, destObjectName, result.UploadId);

            // partETags为分片上传中保存的partETag的列表，OSS收到用户提交的此列表后，会逐一验证每个数据分片的有效性。全部验证通过后，OSS会将这些分片合成一个完整的文件。
            foreach (var partETag in partETags)
            {
                completeMultipartUploadRequest.PartETags.Add(partETag);
            }

            _client.CompleteMultipartUpload(completeMultipartUploadRequest);
            return Task.FromResult(true);
        }

        /// <summary>
        /// RemoveObjectAsync
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="objectName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Task<bool> RemoveObjectAsync(string bucketName, string objectName)
        {
            if (string.IsNullOrEmpty(bucketName))
            {
                throw new ArgumentNullException(nameof(bucketName));
            }
            objectName = FormatObjectName(objectName);
            var result = _client.DeleteObject(bucketName, objectName);
            if (result != null)
            {
                return Task.FromResult(true);
            }
            else
            {
                return Task.FromResult(false);
            }
        }

        /// <summary>
        /// RemoveObjectAsync
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="objectNames"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="Exception"></exception>
        public Task<bool> RemoveObjectAsync(string bucketName, List<string> objectNames)
        {
            if (string.IsNullOrEmpty(bucketName))
            {
                throw new ArgumentNullException(nameof(bucketName));
            }

            if (objectNames == null || objectNames.Count == 0)
            {
                throw new ArgumentNullException(nameof(objectNames));
            }

            List<string> delObjects = new List<string>();
            foreach (var item in objectNames)
            {
                delObjects.Add(FormatObjectName(item));
            }

            var quietMode = false;

            // DeleteObjectsRequest的第三个参数指定返回模式。
            var request = new DeleteObjectsRequest(bucketName, delObjects, quietMode);

            // 删除多个文件。
            var result = _client.DeleteObjects(request);
            if ((!quietMode) && (result.Keys != null))
            {
                if (result.Keys.Count() == delObjects.Count)
                {
                    return Task.FromResult(true);
                }
                else
                {
                    throw new Exception("Some file delete failed.");
                }
            }
            else
            {
                if (result != null)
                {
                    return Task.FromResult(true);
                }
                else
                {
                    return Task.FromResult(true);
                }
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
        public Task<ItemMeta> GetObjectMetadataAsync(string bucketName, string objectName, string versionID = null, string matchEtag = null, DateTime? modifiedSince = null)
        {
            if (string.IsNullOrEmpty(bucketName))
            {
                throw new ArgumentNullException(nameof(bucketName));
            }
            objectName = FormatObjectName(objectName);
            GetObjectMetadataRequest request = new GetObjectMetadataRequest(bucketName, objectName)
            {
                VersionId = versionID
            };
            var oldMeta = _client.GetObjectMetadata(request);

            // 设置新的文件元信息。
            var newMeta = new ItemMeta()
            {
                ObjectName = objectName,
                ContentType = oldMeta.ContentType,
                Size = oldMeta.ContentLength,
                LastModified = oldMeta.LastModified,
                ETag = oldMeta.ETag,
                IsEnableHttps = Options.IsEnableHttps,
                MetaData = new Dictionary<string, string>(),
            };
            if (oldMeta.UserMetadata != null && oldMeta.UserMetadata.Count > 0)
            {
                foreach (var item in oldMeta.UserMetadata)
                {
                    newMeta.MetaData.Add(item.Key, item.Value);
                }
            }

            return Task.FromResult(newMeta);
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
                AccessModeEnum.Default => CannedAccessControlList.Default,
                AccessModeEnum.Private => CannedAccessControlList.Private,
                AccessModeEnum.PublicRead => CannedAccessControlList.PublicRead,
                AccessModeEnum.PublicReadWrite => CannedAccessControlList.PublicReadWrite,
                _ => CannedAccessControlList.Default,
            };
            _client.SetObjectAcl(bucketName, objectName, canned);
            return await Task.FromResult(true);
        }

        /// <summary>
        /// GetObjectAclAsync
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="objectName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="Exception"></exception>
        public async Task<AccessModeEnum> GetObjectAclAsync(string bucketName, string objectName)
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
            var result = _client.GetObjectAcl(bucketName, objectName);
            var mode = result.ACL switch
            {
                CannedAccessControlList.Default => AccessModeEnum.Default,
                CannedAccessControlList.Private => AccessModeEnum.Private,
                CannedAccessControlList.PublicRead => AccessModeEnum.PublicRead,
                CannedAccessControlList.PublicReadWrite => AccessModeEnum.PublicReadWrite,
                _ => AccessModeEnum.Default,
            };
            if (mode == AccessModeEnum.Default)
            {
                return await GetBucketAclAsync(bucketName);
            }

            return await Task.FromResult(mode);
        }

        /// <summary>
        /// RemoveObjectAclAsync
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="objectName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="Exception"></exception>
        public async Task<AccessModeEnum> RemoveObjectAclAsync(string bucketName, string objectName)
        {
            if (string.IsNullOrEmpty(bucketName))
            {
                throw new ArgumentNullException(nameof(bucketName));
            }

            objectName = FormatObjectName(objectName);
            if (!await SetObjectAclAsync(bucketName, objectName, AccessModeEnum.Default))
            {
                throw new Exception("Save new policy info failed when remove object acl.");
            }

            return await GetObjectAclAsync(bucketName, objectName);
        }
        #endregion
    }
}
