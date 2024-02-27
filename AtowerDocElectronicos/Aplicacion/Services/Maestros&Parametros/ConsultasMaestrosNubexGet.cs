using AtowerDocElectronico.Aplicacion.Dtos.Maestros_Parametros;
using AtowerDocElectronico.Aplicacion.Interfaces;
using AtowerDocElectronico.Infraestructura.Interfaces;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace AtowerDocElectronico.Aplicacion.Services.Maestros_Parametros
{
    public class ConsultasMaestrosNubexGet: IConsultasMaestrosNubexGet
    {
        private readonly INubexDbContext _nubexDbContext;

        public ConsultasMaestrosNubexGet(INubexDbContext nubexDbContext)
        {
            _nubexDbContext = nubexDbContext;
        }
        public async Task<(IEnumerable<ImpuestosDTOs>, int)> ObtenerImpuestos(int page = 1, int pageSize = 10, string? codigo = null, string? nombre = null, string? descripcion = null)
        {
            if (page < 1)
            {
                page = 1;
            }

            if (pageSize < 1)
            {
                pageSize = 10;
            }

            // Acceder a la conexión de base de datos a través de Entity Framework Core
            using IDbConnection dbConnection = _nubexDbContext.Database.GetDbConnection();

            // Construir la consulta SQL base
            var sql = "SELECT Id , Code as Codigo, Name as Descripcion, Description as Nombre FROM taxes";

            var whereClauses = new List<string>();

            // Condición para el filtro de código
            if (!string.IsNullOrEmpty(codigo))
            {
                whereClauses.Add("Code LIKE @Codigo");
            }

            // Condición para el filtro de nombre
            if (!string.IsNullOrEmpty(nombre))
            {
                whereClauses.Add("Name LIKE @Nombre");
            }

            // Condición para el filtro de descripción
            if (!string.IsNullOrEmpty(descripcion))
            {
                whereClauses.Add("Description LIKE @Descripcion");
            }

            if (whereClauses.Any())
            {
                sql += " WHERE " + string.Join(" AND ", whereClauses);
            }

            // Consulta para obtener el recuento total de documentos
            var countQuery = "SELECT COUNT(Id) FROM taxes";
            if (whereClauses.Any())
            {
                countQuery += " WHERE " + string.Join(" AND ", whereClauses);
            }

            // Obtener el recuento total de documentos
            var totalDocumentos = await dbConnection.ExecuteScalarAsync<int>(countQuery, new { Codigo = $"%{codigo}%", Nombre = $"%{nombre}%", Descripcion = $"%{descripcion}%" });

            // Calcular el número total de páginas
            var totalPages = (int)Math.Ceiling((double)totalDocumentos / pageSize);

            // Calcular el desplazamiento
            var offset = (page - 1) * pageSize;

            // Ejecutar la consulta
            var documentos = await dbConnection.QueryAsync<ImpuestosDTOs>(sql + " ORDER BY Id LIMIT @PageSize OFFSET @Offset", new { Codigo = $"%{codigo}%", Nombre = $"%{nombre}%", Descripcion = $"%{descripcion}%", PageSize = pageSize, Offset = offset });

            return (documentos, totalPages);
        }
        public async Task<(IEnumerable<TipoRegimenDTOs>, int)> ObtenerTipoRegimenPaginados(int page = 1, int pageSize = 10, string? codigo = null, string? nombre = null)
        {
            if (page < 1)
            {
                page = 1;
            }

            if (pageSize < 1)
            {
                pageSize = 10;
            }

            // Acceder a la conexión de base de datos a través de Entity Framework Core
            using IDbConnection dbConnection = _nubexDbContext.Database.GetDbConnection();

            // Construir la consulta SQL base
            var sql = "SELECT Id, Code, Name FROM type_regimes";

            // Condición para el filtro de código
            if (!string.IsNullOrEmpty(codigo))
            {
                sql += " WHERE Code LIKE @Codigo";
            }

            // Condición para el filtro de nombre
            if (!string.IsNullOrEmpty(nombre))
            {
                sql += string.IsNullOrEmpty(codigo) ? " WHERE" : " AND";
                sql += " Name LIKE @Nombre";
            }

            // Agregar la cláusula de paginación (MySQL utiliza LIMIT y OFFSET en lugar de OFFSET y FETCH NEXT)
            sql += " ORDER BY Id LIMIT @PageSize OFFSET @Offset";

            // Consulta para obtener el recuento total de documentos
            var countQuery = "SELECT COUNT(Id) FROM type_regimes";
            if (!string.IsNullOrEmpty(codigo))
            {
                countQuery += " WHERE Code LIKE @Codigo";
            }
            if (!string.IsNullOrEmpty(nombre))
            {
                countQuery += string.IsNullOrEmpty(codigo) ? " WHERE" : " AND";
                countQuery += " Name LIKE @Nombre";
            }

            // Obtener el recuento total de documentos
            var totalDocumentos = await dbConnection.ExecuteScalarAsync<int>(countQuery, new { Codigo = $"%{codigo}%", Nombre = $"%{nombre}%" });

            // Calcular el número total de páginas
            var totalPages = (int)Math.Ceiling((double)totalDocumentos / pageSize);

            // Calcular el desplazamiento
            var offset = (page - 1) * pageSize;

            // Ejecutar la consulta
            var documentos = await dbConnection.QueryAsync(sql, new { Codigo = $"%{codigo}%", Nombre = $"%{nombre}%", PageSize = pageSize, Offset = offset });


            var documentoDTOs = documentos.Select(tipoDoc => new TipoRegimenDTOs
            {
                Id = tipoDoc.Id,
                Codigo = tipoDoc.Code,
                Nombre = tipoDoc.Name
            });

            return (documentoDTOs, totalPages);

        }
        public async Task<(IEnumerable<DescuentoDTOs>, int)> ObtenerDescuentos(int page = 1, int pageSize = 10, string? codigo = null, string? nombre = null)
        {
            if (page < 1)
            {
                page = 1;
            }

            if (pageSize < 1)
            {
                pageSize = 10;
            }

            // Acceder a la conexión de base de datos a través de Entity Framework Core
            using IDbConnection dbConnection = _nubexDbContext.Database.GetDbConnection();

            // Construir la consulta SQL base
            var sql = "SELECT Id, Code, Name FROM discounts";

            // Condición para el filtro de código
            if (!string.IsNullOrEmpty(codigo))
            {
                sql += " WHERE Code LIKE @Codigo";
            }

            // Condición para el filtro de nombre
            if (!string.IsNullOrEmpty(nombre))
            {
                sql += string.IsNullOrEmpty(codigo) ? " WHERE" : " AND";
                sql += " Name LIKE @Nombre";
            }

            // Agregar la cláusula de paginación (MySQL utiliza LIMIT y OFFSET en lugar de OFFSET y FETCH NEXT)
            sql += " ORDER BY Id LIMIT @PageSize OFFSET @Offset";

            // Consulta para obtener el recuento total de documentos
            var countQuery = "SELECT COUNT(Id) FROM discounts";
            if (!string.IsNullOrEmpty(codigo))
            {
                countQuery += " WHERE Code LIKE @Codigo";
            }
            if (!string.IsNullOrEmpty(nombre))
            {
                countQuery += string.IsNullOrEmpty(codigo) ? " WHERE" : " AND";
                countQuery += " Name LIKE @Nombre";
            }

            // Obtener el recuento total de documentos
            var totalDocumentos = await dbConnection.ExecuteScalarAsync<int>(countQuery, new { Codigo = $"%{codigo}%", Nombre = $"%{nombre}%" });

            // Calcular el número total de páginas
            var totalPages = (int)Math.Ceiling((double)totalDocumentos / pageSize);

            // Calcular el desplazamiento
            var offset = (page - 1) * pageSize;

            // Ejecutar la consulta
            var documentos = await dbConnection.QueryAsync(sql, new { Codigo = $"%{codigo}%", Nombre = $"%{nombre}%", PageSize = pageSize, Offset = offset });


            var documentoDTOs = documentos.Select(tipoDoc => new DescuentoDTOs
            {
                Id = tipoDoc.Id,
                Codigo = tipoDoc.Code,
                Nombre = tipoDoc.Name
            });

            return (documentoDTOs, totalPages);

        }
        public async Task<(IEnumerable<MetodoPagoDTOs>, int)> ObtenerMetodoPago(int page = 1, int pageSize = 10, string? codigo = null, string? nombre = null)
        {
            if (page < 1)
            {
                page = 1;
            }

            if (pageSize < 1)
            {
                pageSize = 10;
            }

            // Acceder a la conexión de base de datos a través de Entity Framework Core
            using IDbConnection dbConnection = _nubexDbContext.Database.GetDbConnection();

            // Construir la consulta SQL base
            var sql = "SELECT Id, Code, Name FROM payment_methods";

            // Condición para el filtro de código
            if (!string.IsNullOrEmpty(codigo))
            {
                sql += " WHERE Code LIKE @Codigo";
            }

            // Condición para el filtro de nombre
            if (!string.IsNullOrEmpty(nombre))
            {
                sql += string.IsNullOrEmpty(codigo) ? " WHERE" : " AND";
                sql += " Name LIKE @Nombre";
            }

            // Agregar la cláusula de paginación (MySQL utiliza LIMIT y OFFSET en lugar de OFFSET y FETCH NEXT)
            sql += " ORDER BY Id LIMIT @PageSize OFFSET @Offset";

            // Consulta para obtener el recuento total de documentos
            var countQuery = "SELECT COUNT(Id) FROM payment_methods";
            if (!string.IsNullOrEmpty(codigo))
            {
                countQuery += " WHERE Code LIKE @Codigo";
            }
            if (!string.IsNullOrEmpty(nombre))
            {
                countQuery += string.IsNullOrEmpty(codigo) ? " WHERE" : " AND";
                countQuery += " Name LIKE @Nombre";
            }

            // Obtener el recuento total de documentos
            var totalDocumentos = await dbConnection.ExecuteScalarAsync<int>(countQuery, new { Codigo = $"%{codigo}%", Nombre = $"%{nombre}%" });

            // Calcular el número total de páginas
            var totalPages = (int)Math.Ceiling((double)totalDocumentos / pageSize);

            // Calcular el desplazamiento
            var offset = (page - 1) * pageSize;

            // Ejecutar la consulta
            var documentos = await dbConnection.QueryAsync(sql, new { Codigo = $"%{codigo}%", Nombre = $"%{nombre}%", PageSize = pageSize, Offset = offset });


            var documentoDTOs = documentos.Select(tipoDoc => new MetodoPagoDTOs
            {
                Id = tipoDoc.Id,
                Codigo = tipoDoc.Code,
                Nombre = tipoDoc.Name
            });

            return (documentoDTOs, totalPages);

        }
        public async Task<(IEnumerable<FormaPagoDTOs>, int)> ObtenerFormasPago(int page = 1, int pageSize = 10, string? codigo = null, string? nombre = null)
        {
            if (page < 1)
            {
                page = 1;
            }

            if (pageSize < 1)
            {
                pageSize = 10;
            }

            // Acceder a la conexión de base de datos a través de Entity Framework Core
            using IDbConnection dbConnection = _nubexDbContext.Database.GetDbConnection();

            // Construir la consulta SQL base
            var sql = "SELECT Id, Code, Name FROM type_documents";

            // Condición para el filtro de código
            if (!string.IsNullOrEmpty(codigo))
            {
                sql += " WHERE Code LIKE @Codigo";
            }

            // Condición para el filtro de nombre
            if (!string.IsNullOrEmpty(nombre))
            {
                sql += string.IsNullOrEmpty(codigo) ? " WHERE" : " AND";
                sql += " Name LIKE @Nombre";
            }

            // Agregar la cláusula de paginación (MySQL utiliza LIMIT y OFFSET en lugar de OFFSET y FETCH NEXT)
            sql += " ORDER BY Id LIMIT @PageSize OFFSET @Offset";

            // Consulta para obtener el recuento total de documentos
            var countQuery = "SELECT COUNT(Id) FROM type_documents";
            if (!string.IsNullOrEmpty(codigo))
            {
                countQuery += " WHERE Code LIKE @Codigo";
            }
            if (!string.IsNullOrEmpty(nombre))
            {
                countQuery += string.IsNullOrEmpty(codigo) ? " WHERE" : " AND";
                countQuery += " Name LIKE @Nombre";
            }

            // Obtener el recuento total de documentos
            var totalDocumentos = await dbConnection.ExecuteScalarAsync<int>(countQuery, new { Codigo = $"%{codigo}%", Nombre = $"%{nombre}%" });

            // Calcular el número total de páginas
            var totalPages = (int)Math.Ceiling((double)totalDocumentos / pageSize);

            // Calcular el desplazamiento
            var offset = (page - 1) * pageSize;

            // Ejecutar la consulta
            var documentos = await dbConnection.QueryAsync(sql, new { Codigo = $"%{codigo}%", Nombre = $"%{nombre}%", PageSize = pageSize, Offset = offset });


            var documentoDTOs = documentos.Select(tipoDoc => new FormaPagoDTOs
            {
                Id = tipoDoc.Id,
                Codigo = tipoDoc.Code,
                Nombre = tipoDoc.Name
            });

            return (documentoDTOs, totalPages);

        }
        public async Task<(IEnumerable<TipoDocumentoDTOs>, int)> ObtenerTipoDocumentosPaginados(int page = 1, int pageSize = 10, string? codigo = null, string? nombre = null)
        {
            if (page < 1)
            {
                page = 1;
            }

            if (pageSize < 1)
            {
                pageSize = 10;
            }

            // Acceder a la conexión de base de datos a través de Entity Framework Core
            using IDbConnection dbConnection = _nubexDbContext.Database.GetDbConnection();

            // Construir la consulta SQL base
            var sql = "SELECT Id, Code, Name FROM type_document_identifications";

            // Condición para el filtro de código
            if (!string.IsNullOrEmpty(codigo))
            {
                sql += " WHERE Code LIKE @Codigo";
            }

            // Condición para el filtro de nombre
            if (!string.IsNullOrEmpty(nombre))
            {
                sql += string.IsNullOrEmpty(codigo) ? " WHERE" : " AND";
                sql += " Name LIKE @Nombre";
            }

            // Agregar la cláusula de paginación (MySQL utiliza LIMIT y OFFSET en lugar de OFFSET y FETCH NEXT)
            sql += " ORDER BY Id LIMIT @PageSize OFFSET @Offset";

            // Consulta para obtener el recuento total de documentos
            var countQuery = "SELECT COUNT(Id) FROM type_document_identifications";
            if (!string.IsNullOrEmpty(codigo))
            {
                countQuery += " WHERE Code LIKE @Codigo";
            }
            if (!string.IsNullOrEmpty(nombre))
            {
                countQuery += string.IsNullOrEmpty(codigo) ? " WHERE" : " AND";
                countQuery += " Name LIKE @Nombre";
            }

            // Obtener el recuento total de documentos
            var totalDocumentos = await dbConnection.ExecuteScalarAsync<int>(countQuery, new { Codigo = $"%{codigo}%", Nombre = $"%{nombre}%" });

            // Calcular el número total de páginas
            var totalPages = (int)Math.Ceiling((double)totalDocumentos / pageSize);

            // Calcular el desplazamiento
            var offset = (page - 1) * pageSize;

            // Ejecutar la consulta
            var documentos = await dbConnection.QueryAsync(sql, new { Codigo = $"%{codigo}%", Nombre = $"%{nombre}%", PageSize = pageSize, Offset = offset });


            var documentoDTOs = documentos.Select(tipoDoc => new TipoDocumentoDTOs
            {
                Id = tipoDoc.Id,
                Codigo = tipoDoc.Code,
                Nombre = tipoDoc.Name
            });

            return (documentoDTOs, totalPages);

        }
        
        public async Task<(IEnumerable<TipoOrganizacionDTOs>, int)> ObtenerTipoOrganizacionPaginados(int page = 1, int pageSize = 10, string? codigo = null, string? nombre = null)
        {
            if (page < 1)
            {
                page = 1;
            }

            if (pageSize < 1)
            {
                pageSize = 10;
            }

            // Acceder a la conexión de base de datos a través de Entity Framework Core
            using IDbConnection dbConnection = _nubexDbContext.Database.GetDbConnection();

            // Construir la consulta SQL base
            var sql = "SELECT Id, Code, Name FROM type_organizations";

            // Condición para el filtro de código
            if (!string.IsNullOrEmpty(codigo))
            {
                sql += " WHERE Code LIKE @Codigo";
            }

            // Condición para el filtro de nombre
            if (!string.IsNullOrEmpty(nombre))
            {
                sql += string.IsNullOrEmpty(codigo) ? " WHERE" : " AND";
                sql += " Name LIKE @Nombre";
            }

            // Agregar la cláusula de paginación (MySQL utiliza LIMIT y OFFSET en lugar de OFFSET y FETCH NEXT)
            sql += " ORDER BY Id LIMIT @PageSize OFFSET @Offset";

            // Consulta para obtener el recuento total de documentos
            var countQuery = "SELECT COUNT(Id) FROM type_organizations";
            if (!string.IsNullOrEmpty(codigo))
            {
                countQuery += " WHERE Code LIKE @Codigo";
            }
            if (!string.IsNullOrEmpty(nombre))
            {
                countQuery += string.IsNullOrEmpty(codigo) ? " WHERE" : " AND";
                countQuery += " Name LIKE @Nombre";
            }

            // Obtener el recuento total de documentos
            var totalDocumentos = await dbConnection.ExecuteScalarAsync<int>(countQuery, new { Codigo = $"%{codigo}%", Nombre = $"%{nombre}%" });

            // Calcular el número total de páginas
            var totalPages = (int)Math.Ceiling((double)totalDocumentos / pageSize);

            // Calcular el desplazamiento
            var offset = (page - 1) * pageSize;

            // Ejecutar la consulta
            var documentos = await dbConnection.QueryAsync(sql, new { Codigo = $"%{codigo}%", Nombre = $"%{nombre}%", PageSize = pageSize, Offset = offset });


            var documentoDTOs = documentos.Select(tipoDoc => new TipoOrganizacionDTOs
            {
                Id = tipoDoc.Id,
                Codigo = tipoDoc.Code,
                Nombre = tipoDoc.Name
            });

            return (documentoDTOs, totalPages);

        }

        public async Task<(IEnumerable<TipoResponsabilidadDTOs>, int)> ObtenerTipoResponsabilidadPaginados(int page = 1, int pageSize = 10, string? codigo = null, string? nombre = null)
        {
            if (page < 1)
            {
                page = 1;
            }

            if (pageSize < 1)
            {
                pageSize = 10;
            }

            // Acceder a la conexión de base de datos a través de Entity Framework Core
            using IDbConnection dbConnection = _nubexDbContext.Database.GetDbConnection();

            // Construir la consulta SQL base
            var sql = "SELECT Id, Code, Name FROM type_liabilities";

            // Condición para el filtro de código
            if (!string.IsNullOrEmpty(codigo))
            {
                sql += " WHERE Code LIKE @Codigo";
            }

            // Condición para el filtro de nombre
            if (!string.IsNullOrEmpty(nombre))
            {
                sql += string.IsNullOrEmpty(codigo) ? " WHERE" : " AND";
                sql += " Name LIKE @Nombre";
            }

            // Agregar la cláusula de paginación (MySQL utiliza LIMIT y OFFSET en lugar de OFFSET y FETCH NEXT)
            sql += " ORDER BY Id LIMIT @PageSize OFFSET @Offset";

            // Consulta para obtener el recuento total de documentos
            var countQuery = "SELECT COUNT(Id) FROM type_liabilities";
            if (!string.IsNullOrEmpty(codigo))
            {
                countQuery += " WHERE Code LIKE @Codigo";
            }
            if (!string.IsNullOrEmpty(nombre))
            {
                countQuery += string.IsNullOrEmpty(codigo) ? " WHERE" : " AND";
                countQuery += " Name LIKE @Nombre";
            }

            // Obtener el recuento total de documentos
            var totalDocumentos = await dbConnection.ExecuteScalarAsync<int>(countQuery, new { Codigo = $"%{codigo}%", Nombre = $"%{nombre}%" });

            // Calcular el número total de páginas
            var totalPages = (int)Math.Ceiling((double)totalDocumentos / pageSize);

            // Calcular el desplazamiento
            var offset = (page - 1) * pageSize;

            // Ejecutar la consulta
            var documentos = await dbConnection.QueryAsync(sql, new { Codigo = $"%{codigo}%", Nombre = $"%{nombre}%", PageSize = pageSize, Offset = offset });


            var documentoDTOs = documentos.Select(tipoDoc => new TipoResponsabilidadDTOs
            {
                Id = tipoDoc.Id,
                Codigo = tipoDoc.Code,
                Nombre = tipoDoc.Name
            });

            return (documentoDTOs, totalPages);
            
        }
        public async Task<(IEnumerable<CiudadDepartamentoDTOs>, int)> ObtenerCiudadPaginados(int page = 1, int pageSize = 10, string? codigoCiudad = null, string? nombreCiudad = null, string? nombreDepartamento = null)
        {
            if (page < 1)
            {
                page = 1;
            }

            if (pageSize < 1)
            {
                pageSize = 10;
            }

            // Acceder a la conexión de base de datos a través de Entity Framework Core
            using IDbConnection dbConnection = _nubexDbContext.Database.GetDbConnection();

            // Construir la consulta SQL base
            var sql = @"SELECT m.id, m.code AS CodigoCiudad, m.name AS NombreCiudad, d.name AS NombreDepartamento
                FROM municipalities m
                INNER JOIN departments d ON d.id = m.department_id";

            var whereClauses = new List<string>();

            if (!string.IsNullOrEmpty(codigoCiudad))
            {
                whereClauses.Add("m.code LIKE @CodigoCiudad");
            }

            if (!string.IsNullOrEmpty(nombreCiudad))
            {
                whereClauses.Add("m.name LIKE @NombreCiudad");
            }

            if (!string.IsNullOrEmpty(nombreDepartamento))
            {
                whereClauses.Add("d.name LIKE @NombreDepartamento");
            }

            if (whereClauses.Any())
            {
                sql += " WHERE " + string.Join(" AND ", whereClauses);
            }

            // Consulta para obtener el recuento total de documentos
            var countQuery = "SELECT COUNT(m.id) FROM municipalities m INNER JOIN departments d ON d.id = m.department_id";
            if (whereClauses.Any())
            {
                countQuery += " WHERE " + string.Join(" AND ", whereClauses);
            }

            // Obtener el recuento total de documentos
            var totalDocumentos = await dbConnection.ExecuteScalarAsync<int>(countQuery, new { CodigoCiudad = $"%{codigoCiudad}%", NombreCiudad = $"%{nombreCiudad}%", NombreDepartamento = $"%{nombreDepartamento}%" });

            // Calcular el número total de páginas
            var totalPages = (int)Math.Ceiling((double)totalDocumentos / pageSize);

            // Calcular el desplazamiento
            var offset = (page - 1) * pageSize;

            // Ejecutar la consulta
            var documentos = await dbConnection.QueryAsync<CiudadDepartamentoDTOs>(sql + " ORDER BY m.id LIMIT @PageSize OFFSET @Offset", new { CodigoCiudad = $"%{codigoCiudad}%", NombreCiudad = $"%{nombreCiudad}%", NombreDepartamento = $"%{nombreDepartamento}%", PageSize = pageSize, Offset = offset });

            return (documentos, totalPages);
        }
    }
}
