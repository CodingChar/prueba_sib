# SB.Management — Gestión de Pagos de Empleados

API en .NET 8 / C# y frontend en React + TypeScript + Tailwind CSS, genuninamente uno de los stacks mas interesantes que he usado, aunque estandar en la industria de RD.

Calcula los pagos semanales de empleados según su tipo de contrato, gestiona
usuarios con roles (Admin/Usuario) vía JWT, genera reportes por periodo, y
mantiene un catálogo CRUD de entidades gubernamentales de la República
Dominicana.

Se usaron arquitecturas que facilitan el desarrollo agil de la aplicacion, siendo estas la arquitectura de cebolla o onion  en ingles!


##Reflexion 

Anteriormente he trabajado con la arquitectura en cebolla y la hexagonal, implemnentandola con Java SpringBoot; es ligeramente diferente a C#, se me hizo mas facil porque estaba mas acostumbrado a usar hibernate en vez de Entity Framework, pero la verstailidad es importante y por eso me logre desevolver basstante bien en el proyecto.

Me auxilie de la inteligencia artificial con la documentacion y con partes tediosas del proyecto que entendia que se podian hacer rapido.

Por ejemplo en los DTOs y Servicios. 


## Contenido

- [Requisitos previos](#requisitos-previos)
- [Cómo ejecutar el backend](#cómo-ejecutar-el-backend)
- [Cómo ejecutar el frontend](#cómo-ejecutar-el-frontend)
- [Arquitectura](#arquitectura)
- [Decisiones de diseño y sus justificaciones](#decisiones-de-diseño-y-sus-justificaciones)
- [Endpoints principales](#endpoints-principales)
- [Pruebas unitarias](#pruebas-unitarias)
- [Estructura del repositorio](#estructura-del-repositorio)

## Requisitos previos

- .NET 8 SDK
- Node.js 18+ y npm
- SQL Server (local o remoto) — usado para `Empleado`, `Pago`, `Usuario`, `Rol`
- (Opcional) `dotnet-ef` CLI si prefieres generar migraciones desde terminal
  en vez de la Package Manager Console de Visual Studio

## Cómo ejecutar el backend

1. **Configura la cadena de conexión** en
   `sib_backend/SIB.API/appsettings.json`, sección `ConnectionStrings:DefaultConnection`,
   apuntando a tu instancia de SQL Server. Ajusta también `Jwt:Key` por un
   secreto propio de al menos 32 caracteres si vas a desplegar esto fuera de
   un entorno de prueba.

2. **Aplica las migraciones de EF Core** para crear la base de datos y sus
   tablas (usando la Package Manager Console de Visual Studio, con
   `Default project` = `SB.Management.Infrastructure`):
   ```
   Add-Migration InitialCreate -StartupProject SB.Management.API
   Update-Database -StartupProject SB.Management.API
   ```
   Esto crea la base `SbGestionPagosDb` con 8 tablas: `Empleado` y sus 4
   tablas hijas (mapeo TPT), `Usuarios`, `Roles`, `Pagos`.

3. **Siembra los roles** (necesarios para poder registrar el primer usuario),
   ejecutando en SQL Server Management Studio contra `SbGestionPagosDb`:
   ```sql
   INSERT INTO [Roles] ([Nombre]) VALUES ('Admin');
   INSERT INTO [Roles] ([Nombre]) VALUES ('Usuario');
   ```

4. **Coloca el catálogo de entidades gubernamentales.** El repositorio de
   `EntidadGubernamental` lee/escribe un archivo de texto plano (JSON) en
   `SIB.API/App_Data/entidades-gubernamentales.json`. Este archivo ya viene
   incluido en el repositorio, generado a partir del Excel oficial
   (`ListaEntidadesGubernamentales.xlsx`, 181 registros). Si el archivo no
   existe, el repositorio lo crea vacío automáticamente en el primer arranque.

5. **Ejecuta la API:**
   - Desde Visual Studio: F5 (perfil `https`)
   - Desde terminal:
     ```
     cd sib_backend/SIB.API
     dotnet run --launch-profile https
     ```
   Swagger disponible en `https://localhost:7293/swagger` (ajusta el puerto
   si tu `launchSettings.json` usa uno distinto).

6. **Registra un usuario administrador** vía `POST /api/auth/registro`
   (`rolId: 1` para Admin), luego `POST /api/auth/login` para obtener un
   token JWT, y autoriza en Swagger con el botón "Authorize" (formato:
   `Bearer {token}`, incluyendo la palabra "Bearer" y el espacio).

## Cómo ejecutar el frontend

1. Configura la URL del backend en `sib_frontend/src/services/api.ts`
   (`baseURL`), si tu API corre en un puerto distinto a `7293`.
2. Instala dependencias y corre el servidor de desarrollo:
   ```
   cd sib_frontend
   npm install
   npm run dev
   ```
3. Abre `http://localhost:5173`. Se te redirige a `/login` si no hay sesión
   activa. Ingresa con el usuario administrador que registraste en el paso
   anterior.

**Nota de CORS:** el backend solo permite peticiones desde
`http://localhost:5173` (configurado en `Program.cs`). Si corres el
frontend en otro puerto, ajusta la política de CORS en el backend.

## Arquitectura

```
Domain/            Entidades puras (Empleado y sus 4 subtipos, Usuario, Rol,
                    Pago, EntidadGubernamental). Sin dependencias externas.
Application/       Casos de uso (EmpleadoService, EntidadGubernamentalService),
                    interfaces de repositorio, DTOs. Depende solo de Domain.
Infrastructure/     EF Core (DbContext con mapeo TPT), repositorios SQL Server,
                    repositorio de archivo plano (EntidadGubernamental),
                    autenticación JWT + BCrypt. Depende de Application y Domain.
API/                Controllers REST, Program.cs (inyección de dependencias,
                    JWT, Swagger, Serilog, CORS). Depende de todo lo anterior.
```

La regla de dependencia va siempre hacia adentro: `Domain` no referencia a
ningún otro proyecto; `API` referencia a todos. Esto permite cambiar el
motor de base de datos, el framework web, o el mecanismo de autenticación
sin tocar la lógica de negocio central.

## Decisiones de diseño y sus justificaciones

### Persistencia dual: SQL Server + archivo de texto plano

SQL Server u Oracle para la aplicación de gestión de pagos. Un documento adicional añadió el
requisito de un mantenimiento de entidades gubernamentales con "base de
datos" en archivo de texto plano, ubicado dentro del proyecto. Dado que el
objetivo de ese segundo documento liga explícitamente el archivo plano "a
este propósito" (el catálogo de entidades gubernamentales), se optó por:

- `Empleado`, `Pago`, `Usuario`, `Rol` → SQL Server vía EF Core
- `EntidadGubernamental` → archivo JSON en `App_Data/`, leído/escrito
  directamente por `EntidadGubernamentalFileRepository`

Esto permite satisfacer los enunciados de las instrucciones y documentacion dadas; 
Aunque anteriormente se considero ingresarlos a la base de datos SQLServer, esta idea se descarto porque podria no cumplir los requerimientos. 

### Mapeo TPT (Table Per Type) para la jerarquía de Empleado

`Empleado` es una clase abstracta con 4 subtipos (`EmpleadoAsalariado`,
`EmpleadoPorHora`, `EmpleadoPorComision`, `EmpleadoAsalariadoComision`), cada
uno con campos y fórmula de cálculo propios. Se usó TPT en vez de TPH (una
sola tabla con columnas NULL según el tipo) porque:

- Permite constraints `NOT NULL` reales por tipo a nivel de base de datos,
  no solo validación en la capa de aplicación
- Agregar un tipo nuevo de empleado no requiere alterar las tablas
  existentes — solo agregar una tabla hija nueva (cumple el requisito de
  "escalabilidad sin modificar código existente" del enunciado)

**No existe una columna discriminadora `Tipo`** en la tabla `Empleado`: el
tipo real de cada empleado se determina por en cuál tabla hija existe la
fila con el mismo `Id`. Guardar el tipo como columna aparte sería
información redundante (derivable de la estructura relacional) y
permitiría una anomalía de actualización si ambos datos se desincronizan.

**Limitación conocida:** el modelo relacional no puede forzar nativamente
"esta fila de `Empleado` tiene exactamente una fila coincidente en una — y
solo una — de las 4 tablas hijas" mediante constraints estándar de SQL
Server (no puede referenciar múltiples tablas en un `CHECK`). Esta
exclusividad se garantiza por diseño en `EmpleadoService`, que siempre
inserta en una única tabla hija por operación de creación.

### Cálculo de pago en el dominio, no en SQL

Cada subtipo de `Empleado` implementa `CalcularPago()` con polimorfismo, en
C# puro, sin acceso a base de datos. Se evitó deliberadamente duplicar esta
lógica en un stored procedure o trigger de SQL, para no mantener la misma
regla de negocio en dos lugares distintos, y para permitir pruebas
unitarias rápidas sin depender de una base de datos real.

### Departamento como atributo simple, no como entidad; Estado como atributo de Activo/Inactivo 

Departamento:

El enunciado pide poder filtrar empleados por departamento, pero no lo
incluye entre los campos a capturar según el tipo de empleado (sección de
"Requisitos funcionales"). Se optó por modelarlo como un atributo de texto
simple en `Empleado`. 

Una decision hecha meramente por ahorro de tiempo para el desarrollo de la prueba tecnica, sin embargo, se reconoce que es un error garrafal dado a que podria afectar la consistencia de los datos. 

Estado: 

El atributo estado de empleado, era evidente que podria referirse al eestado como lugar fisico del empleado, es decir la provincia donde pertenece; sin embargo, considere que usarlo como un atributo para indicar si se encuentra activo o no ahorraba mas tiempo.

Aun asi, se comprende perfectamente como se hubiese aplicado de la otra manera, pues hubiese requerido un atributo con clave foranea a una tabla Estados. 



### Reportería por rango de fechas

`Pago` se consulta por `FechaInicio`/`FechaFin` en vez de una fecha puntual,
para que un mismo endpoint sirva tanto para reportes semanales como para
cualquier periodo arbitrario que se necesite, sin duplicar lógica.

Sin embargo, en el frontend no se facilita de forma intuitiva 

### Números mágicos como constantes nombradas

Siguiendo la regla de nomenclatura del documento de especificaciones
técnicas ("No usar números mágicos"), valores como las 40 horas semanales
regulares, el factor 1.5 de horas extra, y el 10% de bonificación sobre
salario base están declarados como constantes en mayúscula
(`HORAS_SEMANALES_REGULARES`, `FACTOR_HORAS_EXTRA`,
`FACTOR_BONIFICACION_SALARIO_BASE`) en vez de literales sueltos en el
código.

## Endpoints principales

| Método | Ruta | Rol requerido |
|---|---|---|
| POST | `/api/auth/registro` | — |
| POST | `/api/auth/login` | — |
| GET | `/api/empleados?nombre=&departamento=&estado=` | Autenticado |
| POST | `/api/empleados/asalariado` | Admin |
| POST | `/api/empleados/por-hora` | Admin |
| POST | `/api/empleados/por-comision` | Admin |
| POST | `/api/empleados/asalariado-comision` | Admin |
| GET | `/api/empleados/reporte?fechaInicio=&fechaFin=` | Autenticado |
| GET | `/api/entidadesgubernamentales` | Autenticado |
| GET | `/api/entidadesgubernamentales/{id}` | Autenticado |
| POST | `/api/entidadesgubernamentales` | Admin |
| PUT | `/api/entidadesgubernamentales/{id}` | Admin |
| DELETE | `/api/entidadesgubernamentales/{id}` | Admin |

Documentación interactiva completa disponible en `/swagger` en entorno de
desarrollo.

## Pruebas unitarias

Proyecto `SB.Management.Domain.Tests` (xUnit), con 7 pruebas:

- 6 pruebas sobre `CalcularPago()` de cada uno de los 4 subtipos de
  `Empleado`, incluyendo el caso límite exacto de 40 horas trabajadas (sin
  recargo) y un caso con horas extra (con recargo de 1.5x)
- 1 prueba de rendimiento que valida el requisito no funcional explícito
  del enunciado: el cálculo de pago para 1,000 empleados debe completarse
  en menos de 2 segundos

Ejecutar desde Visual Studio: menú `Test` → `Run All Tests`.

## Estructura del repositorio

```
prueba_sib/
  sib_backend/
    SIB.API/                      (namespace/ensamblado: SB.Management.API)
      App_Data/
        entidades-gubernamentales.json
      Controllers/
      Program.cs
      appsettings.json
    SIB.Domain/                   (SB.Management.Domain)
      Entities/
    SIB.Application/              (SB.Management.Application)
      Interfaces/
      DTOs/
      Services/
    SIB.Infrastructure/           (SB.Management.Infrastructure)
      Persistence/
      Repositories/
      FileStorage/
      Security/
    SB.Management.Domain.Tests/
    SB.MANAGEMENT.API.slnx
  sib_frontend/
    src/
      components/                 Sidebar, TopBar, Layout
      context/                    AuthContext
      pages/                      Login, Empleados (consulta/crear),
                                   EntidadesGubernamentales (consulta/crear)
      services/                   Cliente API (axios + interceptor JWT)
      types/
```

**Nota sobre nombres de carpeta:** las carpetas de proyecto en disco
conservan el prefijo `SIB.` por razones históricas del desarrollo, mientras
que los archivos `.csproj`, namespaces y ensamblados usan el prefijo
`SB.Management.`, siguiendo la convención `[SB].[NombreProyecto].[Capa]`
del documento de especificaciones técnicas. Esto no afecta la compilación
ni el funcionamiento del proyecto.

// -> habia nombrado mal el proyecto inicialmente, esa es la razon historica. 

