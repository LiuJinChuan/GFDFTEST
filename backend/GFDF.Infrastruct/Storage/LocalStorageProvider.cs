using GFDF.Infrastruct.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;

namespace GFDF.Infrastruct.Storage
{

    /// <summary>
    /// 本地存储提供程序
    /// </summary>
    public class LocalStorageProvider : IStorageProvider
    {
        /// <summary>
        /// Defines the _rootPath
        /// </summary>
        private readonly string _rootPath;

        /// <summary>
        /// Defines the _rootUrl
        /// </summary>
        private readonly string _rootUrl;

        /// <summary>
        /// Gets the ProviderName
        /// </summary>
        public string ProviderName => "Local";

        /// <summary>
        /// Gets or sets the AllowExtensionList
        /// 允许的扩展列表
        /// </summary>
        public IList<string> AllowExtensionList { get; set; }

        /// <summary>
        /// The ExceptionHandling
        /// </summary>
        /// <param name="ioAction">The ioAction<see cref="Action"/></param>
        private void ExceptionHandling(Action ioAction)
        {
            ioAction();
        }

        /// <summary>
        /// The ExceptionHandling
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ioFunc">The ioFunc<see cref="Func{T}"/></param>
        /// <returns>The <see cref="T"/></returns>
        private T ExceptionHandling<T>(Func<T> ioFunc)
        {
            return ioFunc();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalStorageProvider"/> class.
        /// </summary>
        /// <param name="rootPath">文件根路径</param>
        /// <param name="rootUrl">根Url</param>
        public LocalStorageProvider(string rootPath, string rootUrl)
        {
            _rootPath = rootPath;
            _rootUrl = rootUrl;
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="containerName"></param>
        /// <param name="blobName"></param>
        /// <returns>The <see cref="Task"/></returns>
        public async Task DeleteBlob(string containerName, string blobName)
        {
            await Task.Run(() =>
            {
                ExceptionHandling(() =>
                {
                    var path = Path.Combine(_rootPath, containerName, blobName);
                    File.Delete(path);
                });
            });
        }

        /// <summary>
        /// 删除容器
        /// </summary>
        /// <param name="containerName">容器名称</param>
        /// <returns>The <see cref="Task"/></returns>
        public async Task DeleteContainer(string containerName)
        {
            await Task.Run(() =>
            {
                ExceptionHandling(() =>
                {
                    var path = Path.Combine(_rootPath, containerName);
                    Directory.Delete(path, true);
                });
            });
        }

#pragma warning disable CS1998 // 此异步方法缺少 "await" 运算符，将以同步方式运行。请考虑使用 "await" 运算符等待非阻止的 API 调用，或者使用 "await Task.Run(...)" 在后台线程上执行占用大量 CPU 的工作。
        /// <summary>
        /// 获取文件信息
        /// </summary>
        /// <param name="containerName"></param>
        /// <param name="blobName"></param>
        /// <returns></returns>
        public async Task<BlobFileInfo> GetBlobFileInfo(string containerName, string blobName)
#pragma warning restore CS1998 // 此异步方法缺少 "await" 运算符，将以同步方式运行。请考虑使用 "await" 运算符等待非阻止的 API 调用，或者使用 "await Task.Run(...)" 在后台线程上执行占用大量 CPU 的工作。
        {
            //return await Task.Run(() => ExceptionHandling(() =>
            // {
            var path = Path.Combine(_rootPath, containerName, blobName);
            var info = new FileInfo(path);

            return new BlobFileInfo
            {
                Container = containerName,
                ContentMD5 = "",
                ContentType = info.Extension.GetMimeType(),
                ETag = "",
                LastModified = info.LastWriteTimeUtc,
                Length = info.Length,
                Name = info.Name,
                Url = GetUrl(containerName, blobName)
            };
            //}));
        }

        /// <summary>
        /// The GetBlobStream
        /// </summary>
        /// <param name="containerName">The containerName<see cref="string"/></param>
        /// <param name="blobName">The blobName<see cref="string"/></param>
        /// <returns>The <see cref="Task{Stream}"/></returns>
        public async Task<Stream> GetBlobStream(string containerName, string blobName)
        {
            return await Task.Run(() =>
            {
                return ExceptionHandling(() =>
                {
                    var path = Path.Combine(_rootPath, containerName, blobName);
                    return (Stream)File.OpenRead(path);
                });
            });
        }

        /// <summary>
        /// The GetBlobUrl
        /// </summary>
        /// <param name="containerName">The containerName<see cref="string"/></param>
        /// <param name="blobName">The blobName<see cref="string"/></param>
        /// <returns>The <see cref="Task{string}"/></returns>
        public Task<string> GetBlobUrl(string containerName, string blobName)
        {
            var path = Path.Combine(_rootPath, containerName, blobName);
            if (!Directory.Exists(Path.Combine(_rootPath, containerName)))
            {
                throw new BaseException(ExEnum.Other, "没有找到该容器");
            }

            if (!File.Exists(path))
            {
                throw new BaseException(ExEnum.Other, "没有找到该文件");
            }
            var url = GetUrl(containerName, blobName);
            return Task.FromResult(url);
        }

        public string GetUrl(string containerName, string blobName) => string.Format("{0}/{1}/{2}", _rootUrl.TrimEnd('/'), containerName, blobName);
        /// <summary>
        /// The ListBlobs
        /// </summary>
        /// <param name="containerName">The containerName<see cref="string"/></param>
        /// <returns>The <see cref="Task{IList{BlobFileInfo}}"/></returns>
        public async Task<IList<BlobFileInfo>> ListBlobs(string containerName)
        {
            return await Task.Run(() =>
            {
                return ExceptionHandling(() =>
                {
                    var localFilesInfo = new List<BlobFileInfo>();
                    var dir = Path.Combine(_rootPath, containerName);
                    var dirInfo = new DirectoryInfo(dir);
                    var fileInfo = dirInfo.GetFiles();

                    foreach (var f in fileInfo)
                        localFilesInfo.Add(new BlobFileInfo
                        {
                            ContentMD5 = "",
                            ETag = "",
                            ContentType = f.Extension.GetMimeType(),
                            Container = containerName,
                            LastModified = f.LastWriteTime,
                            Length = f.Length,
                            Name = f.Name,
                            Url = f.FullName,
                        });

                    return localFilesInfo;
                });
            });
        }

#pragma warning disable CS1998 // 此异步方法缺少 "await" 运算符，将以同步方式运行。请考虑使用 "await" 运算符等待非阻止的 API 调用，或者使用 "await Task.Run(...)" 在后台线程上执行占用大量 CPU 的工作。
        /// <summary>
        /// The SaveBlobStream
        /// </summary>
        /// <param name="containerName">The containerName<see cref="string"/></param>
        /// <param name="blobName">The blobName<see cref="string"/></param>
        /// <param name="source">The source<see cref="Stream"/></param>
        /// <returns>The <see cref="Task"/></returns>
        public async Task SaveBlobStream(string containerName, string blobName, Stream source)
#pragma warning restore CS1998 // 此异步方法缺少 "await" 运算符，将以同步方式运行。请考虑使用 "await" 运算符等待非阻止的 API 调用，或者使用 "await Task.Run(...)" 在后台线程上执行占用大量 CPU 的工作。
        {
            //await Task.Run(() =>
            //{
            //ExceptionHandling(() =>
            //{
            var dir = Path.Combine(_rootPath, containerName);
            Directory.CreateDirectory(dir);
            using (var file = File.Create(Path.Combine(dir, blobName)))
            {
                if (AllowExtensionList != null && !AllowExtensionList.Contains((Path.GetExtension(blobName) ?? "".ToLower())))
                {
                    throw new BaseException(ExEnum.Other, $"不支持{Path.GetExtension(blobName)}类型的文件上传，请查看允许的扩展名设置！");
                }
                source.CopyTo(file);
            }
            //});
            //});
        }

#pragma warning disable CS1998 // 此异步方法缺少 "await" 运算符，将以同步方式运行。请考虑使用 "await" 运算符等待非阻止的 API 调用，或者使用 "await Task.Run(...)" 在后台线程上执行占用大量 CPU 的工作。
        /// <summary>
        /// The SaveBlobStream
        /// </summary>
        /// <param name="containerName">The containerName<see cref="string"/></param>
        /// <param name="blobName">The blobName<see cref="string"/></param>
        /// <param name="source">The source<see cref="Stream"/></param>
        /// <returns>The <see cref="Task"/></returns>
        public async Task SaveBlobByte(string containerName, string blobName, byte[] source)
#pragma warning restore CS1998 // 此异步方法缺少 "await" 运算符，将以同步方式运行。请考虑使用 "await" 运算符等待非阻止的 API 调用，或者使用 "await Task.Run(...)" 在后台线程上执行占用大量 CPU 的工作。
        {
            //await Task.Run(() =>
            //{
            //ExceptionHandling(() =>
            //{
            var dir = Path.Combine(_rootPath, containerName);
            Directory.CreateDirectory(dir);
            //using (var file = File.Create(Path.Combine(dir, blobName)))
            //{
            if (AllowExtensionList != null && !AllowExtensionList.Contains((Path.GetExtension(blobName) ?? "".ToLower())))
            {
                throw new BaseException(ExEnum.Other, $"不支持{Path.GetExtension(blobName)}类型的文件上传，请查看允许的扩展名设置！");
            }
            File.WriteAllBytes(Path.Combine(dir, blobName), source);
            //}
            //});
            //});
        }

#pragma warning disable CS1998 // 此异步方法缺少 "await" 运算符，将以同步方式运行。请考虑使用 "await" 运算符等待非阻止的 API 调用，或者使用 "await Task.Run(...)" 在后台线程上执行占用大量 CPU 的工作。
        /// <summary>
        /// The SaveBlobStream
        /// </summary>
        /// <param name="containerName">The containerName<see cref="string"/></param>
        /// <param name="blobName">The blobName<see cref="string"/></param>
        /// <param name="source">The source<see cref="Stream"/></param>
        /// <returns>The <see cref="Task"/></returns>
        public async Task SaveBlobBitmap(string containerName, string blobName, Bitmap source)
#pragma warning restore CS1998 // 此异步方法缺少 "await" 运算符，将以同步方式运行。请考虑使用 "await" 运算符等待非阻止的 API 调用，或者使用 "await Task.Run(...)" 在后台线程上执行占用大量 CPU 的工作。
        {
            //await Task.Run(() =>
            //{
            //ExceptionHandling(() =>
            //{
            var dir = Path.Combine(_rootPath, containerName);
            Directory.CreateDirectory(dir);
            //using (var file = File.Create(Path.Combine(dir, blobName)))
            //{
            if (AllowExtensionList != null && !AllowExtensionList.Contains((Path.GetExtension(blobName) ?? "".ToLower())))
            {
                throw new BaseException(ExEnum.Other, $"不支持{Path.GetExtension(blobName)}类型的文件上传，请查看允许的扩展名设置！");
            }
            source.Save(Path.Combine(dir, blobName), ImageFormat.Png);
            //}
            //});
            //});
        }

        /// <summary>
        /// The GetBlobUrl
        /// </summary>
        /// <param name="containerName">The containerName<see cref="string"/></param>
        /// <param name="blobName">The blobName<see cref="string"/></param>
        /// <param name="expiry">The expiry<see cref="DateTime"/></param>
        /// <param name="isDownload">The isDownload<see cref="bool"/></param>
        /// <param name="fileName">The fileName<see cref="string"/></param>
        /// <param name="contentType">The contentType<see cref="string"/></param>
        /// <param name="access">The access<see cref="BlobUrlAccess"/></param>
        /// <returns>The <see cref="Task{string}"/></returns>
        public Task<string> GetBlobUrl(string containerName, string blobName, DateTime expiry, bool isDownload = false, string fileName = null, string contentType = null, BlobUrlAccess access = BlobUrlAccess.Read) => throw new NotSupportedException();
    }
}
