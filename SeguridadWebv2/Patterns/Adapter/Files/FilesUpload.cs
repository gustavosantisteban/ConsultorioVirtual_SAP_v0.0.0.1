using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeguridadWebv2.Patterns.Adapter.Files
{
    public abstract class FilesUpload<T> where T : class
    {
        public abstract ICollection<Models.Aplicacion.File> UploadFile(T sr);
    }
}