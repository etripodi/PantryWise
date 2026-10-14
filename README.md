# PantryWise

App de despensa inteligente que integra la API de [Open Food Facts](https://world.openfoodfacts.org/). Trabajo final de Desarrollo de Software (UTN FRCU).

Es una solución en capas generada con la [plantilla de ABP](https://abp.io/docs/latest/solution-templates/layered-web-application) (ABP 9.3, .NET 9, EF Core, SQL Server).

## Requisitos

* [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet)
* SQL Server (la configuración por defecto usa la instancia `.\MSSQLSERVER2022` con autenticación de Windows)
* [Node.js](https://nodejs.org/en) y [Yarn](https://yarnpkg.com/)
* ABP CLI: `dotnet tool install -g Volo.Abp.Studio.Cli`
* Opcional, solo para crear migraciones nuevas: `dotnet tool install -g dotnet-ef`

## Configuración

La cadena de conexión está en dos archivos y tiene que ser la misma en ambos:

* `src/PantryWise.HttpApi.Host/appsettings.json`
* `src/PantryWise.DbMigrator/appsettings.json`

```json
"Default": "Server=.\\MSSQLSERVER2022;Database=PantryWise;Trusted_Connection=True;TrustServerCertificate=True"
```

Si tu instancia de SQL Server tiene otro nombre, cambiala en los dos. En JSON la barra invertida se escribe doble (`.\\`).

### Secrets (opcional en desarrollo)

El Host lee `src/PantryWise.HttpApi.Host/appsettings.secrets.json`, que está ignorado por git. En desarrollo no hace falta: si no existe, ABP usa sus valores por defecto. Para fijar valores propios:

```json
{
  "AuthServer": {
    "CertificatePassPhrase": "<cert-password>"
  },
  "StringEncryption": {
    "DefaultPassPhrase": "<passphrase-de-16-caracteres>"
  }
}
```

## Cómo correrlo

### 1. Instalar las librerías del cliente

Desde la raíz de la solución:

```bash
abp install-libs
```

Si falla con `The engine "node" is incompatible with this module` (select2 pide Node 24 o superior), ignorá el chequeo de versión y volvé a correrlo. En PowerShell:

```powershell
$env:YARN_IGNORE_ENGINES = 'true'
```

```powershell
abp install-libs
```

### 2. Crear la base de datos

El DbMigrator aplica las migraciones y siembra los datos iniciales: el usuario `admin`, los clientes de OpenIddict y los datos de ejemplo de la despensa. Hay que correrlo la primera vez y cada vez que se agregue una migración.

**Tiene que ejecutarse desde la carpeta del proyecto**, porque lee `appsettings.json` del directorio actual:

```bash
cd src/PantryWise.DbMigrator
```

```bash
dotnet run
```

Si lo corrés desde la raíz con `--project`, falla con `The ConnectionString property has not been initialized`.

### 3. Levantar la API

También desde la carpeta del proyecto:

```bash
cd src/PantryWise.HttpApi.Host
```

```bash
dotnet run
```

En Visual Studio: marcá `PantryWise.HttpApi.Host` como proyecto de inicio y F5.

La API queda en `https://localhost:44345` y Swagger en `https://localhost:44345/swagger`.

### 4. Probar en Swagger

1. Abrí `https://localhost:44345/swagger`.
2. Tocá **Authorize**, tildá el scope `PantryWise` y tocá **Authorize** de nuevo.
3. Iniciá sesión con **`admin`** / **`1q2w3E*`**.
4. En las secciones *Pantry*, *Pantry Query*, *Pantry Detail* y *Pantry Basic*, usá **Try it out** con un `beforeDate`, por ejemplo la fecha de dentro de una semana.

### 5. Levantar la UI de Angular

Con la API corriendo y el DbMigrator ya ejecutado (crea el cliente OpenIddict `PantryWise_App` con `http://localhost:4200`):

```bash
cd angular
```

```bash
yarn install
```

```bash
yarn start
```

La UI queda en `http://localhost:4200` y hace login contra `https://localhost:44345` (`admin` / `1q2w3E*`). Si `yarn install` se queja de la versión de Node, definí `YARN_IGNORE_ENGINES=true`.

## Ejemplos del patrón Repository

El mismo caso de uso ("ítems de despensa del usuario actual que vencen antes de una fecha") está resuelto de cuatro formas. Todo está en el namespace `PantryWise.Pantry`.

| Ruta | Servicio | Repositorio |
|---|---|---|
| `GET /api/app/pantry/expiring-items` | `PantryAppService` | `IRepository<PantryItem, Guid>` (genérico) |
| `GET /api/app/pantry-query/expiring-items` | `PantryQueryAppService` | `IReadOnlyRepository<PantryItem, Guid>` |
| `GET /api/app/pantry-detail/expiring-with-product` | `PantryDetailAppService` | `IPantryItemRepository` (repositorio propio, incluye `Product`) |
| `GET /api/app/pantry-basic/expiring-items` | `PantryBasicAppService` | `IBasicRepository<PantryItem, Guid>` (filtra en memoria, a propósito) |

Las cuatro requieren autenticación y devuelven los mismos ítems. Solo `pantry-detail` trae el `Product` completo; en las demás viene en `null`.

### Datos de ejemplo

`PantryDataSeedContributor` (`src/PantryWise.Domain/Pantry`) carga 8 productos y 10 ítems de despensa. Las fechas de vencimiento son relativas a la ejecución del seed:

* **`admin`:** 8 ítems. Uno ya vencido, varios que vencen en 1, 3, 5 y 10 días, y otros a meses.
* **Usuario ficticio:** 2 ítems que vencen pronto y nunca deben aparecer para `admin`. Sirven para comprobar el filtro por `UserId`.

El seed solo corre si no hay productos cargados. Para volver a sembrar con fechas nuevas, vaciá `AppPantryItems` y `AppProducts` y corré el DbMigrator otra vez.

## Migraciones

Para agregar una migración después de cambiar el modelo:

```bash
dotnet ef migrations add <Nombre> --project src/PantryWise.EntityFrameworkCore
```

Después aplicala corriendo el DbMigrator (paso 2).

## Estructura de la solución

* `PantryWise.Domain.Shared`: constantes, enums y localización.
* `PantryWise.Domain`: entidades, interfaces de repositorio y seeders.
* `PantryWise.Application.Contracts`: interfaces de servicios de aplicación y DTOs.
* `PantryWise.Application`: implementación de los servicios de aplicación.
* `PantryWise.EntityFrameworkCore`: `DbContext`, repositorios propios y migraciones.
* `PantryWise.HttpApi` / `PantryWise.HttpApi.Host`: API REST, autenticación (OpenIddict) y Swagger.
* `PantryWise.HttpApi.Client`: proxies de cliente C#.
* `PantryWise.DbMigrator`: aplica las migraciones y siembra los datos iniciales.

## Certificado de firma (producción)

En producción ABP espera un archivo `openiddict.pfx` en el Host para firmar y cifrar los tokens. En desarrollo no hace falta. Para generarlo:

```bash
dotnet dev-certs https -v -ep openiddict.pfx -p <cert-password>
```

Usá el mismo `<cert-password>` en `AuthServer:CertificatePassPhrase` de `appsettings.secrets.json`. Más información en [Configuring OpenIddict](https://abp.io/docs/latest/Deployment/Configuring-OpenIddict#production-environment).

## Recursos

* [Documentación de ABP](https://abp.io/docs/latest)
* [Repositorios en ABP](https://abp.io/docs/latest/framework/architecture/domain-driven-design/repositories)
* [Tutorial Web Application Development (BookStore)](https://abp.io/docs/latest/tutorials/book-store/part-1)
