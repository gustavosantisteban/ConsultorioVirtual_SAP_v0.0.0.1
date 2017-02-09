using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SeguridadWebv2.Models.Aplicacion;
using System.IO;
using CsvHelper;

namespace SeguridadWebv2.Patterns.Adapter.Files
{
    public class CSVUploader : FilesUpload<StreamReader>
    {
        public override ICollection<Models.Aplicacion.File> UploadFile(StreamReader sr)
        {
            CsvReader reader = new CsvReader(sr);
            List<Models.Aplicacion.File> FilesList = new List<Models.Aplicacion.File>();

            while (reader.Read())
            {
                FilesList.Add(new Models.Aplicacion.File
                {
                    Fecha = Convert.ToDateTime(reader.GetField<string>("Fecha")),
                    //Descripcion = reader.GetField<string>("Descripcion")
                });
            }

            return FilesList;
        }
    }
}