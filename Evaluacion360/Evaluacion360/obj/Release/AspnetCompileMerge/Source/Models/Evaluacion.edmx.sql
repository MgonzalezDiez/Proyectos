
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 10/17/2019 00:14:01
-- Generated from EDMX file: D:\Desarrollo C#\Evaluacion 360 v3\Evaluacion360\Evaluacion360\Models\Evaluacion.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [BD_Evaluacion];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Cargos]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Cargos];
GO
IF OBJECT_ID(N'[dbo].[Cargos_Evaluadores]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Cargos_Evaluadores];
GO
IF OBJECT_ID(N'[dbo].[Datos_Usuarios]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Datos_Usuarios];
GO
IF OBJECT_ID(N'[dbo].[Evaluacion_Preguntas]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Evaluacion_Preguntas];
GO
IF OBJECT_ID(N'[dbo].[Evaluacion_Preguntas_Cargos]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Evaluacion_Preguntas_Cargos];
GO
IF OBJECT_ID(N'[dbo].[Evaluacion_Resultado]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Evaluacion_Resultado];
GO
IF OBJECT_ID(N'[dbo].[Evaluacion_Resultado_Cargos]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Evaluacion_Resultado_Cargos];
GO
IF OBJECT_ID(N'[dbo].[Evaluaciones]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Evaluaciones];
GO
IF OBJECT_ID(N'[dbo].[Preguntas_Aleatorias]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Preguntas_Aleatorias];
GO
IF OBJECT_ID(N'[dbo].[Preguntas_Cargos]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Preguntas_Cargos];
GO
IF OBJECT_ID(N'[dbo].[Procesos_Evaluacion]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Procesos_Evaluacion];
GO
IF OBJECT_ID(N'[dbo].[Secciones]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Secciones];
GO
IF OBJECT_ID(N'[dbo].[Rol_Operacion]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Rol_Operacion];
GO
IF OBJECT_ID(N'[dbo].[Operacion]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Operacion];
GO
IF OBJECT_ID(N'[dbo].[Rol]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Rol];
GO
IF OBJECT_ID(N'[dbo].[Estado_Usuarios]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Estado_Usuarios];
GO
IF OBJECT_ID(N'[dbo].[Usuarios]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Usuarios];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Cargos'
CREATE TABLE [dbo].[Cargos] (
    [Codigo_Cargo] nvarchar(20)  NOT NULL,
    [Nombre_Cargo] nvarchar(50)  NOT NULL,
    [Fondo] nvarchar(50)  NOT NULL,
    [Ciclo] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'Cargos_Evaluadores'
CREATE TABLE [dbo].[Cargos_Evaluadores] (
    [Codigo_Cargo] nvarchar(20)  NOT NULL,
    [Cod_Cargo_Evaluado] nvarchar(20)  NOT NULL,
    [Ponderacion] decimal(18,1)  NOT NULL
);
GO

-- Creating table 'Datos_Usuarios'
CREATE TABLE [dbo].[Datos_Usuarios] (
    [Codigo_Usuario] nvarchar(20)  NOT NULL,
    [Nombre_Completo] nvarchar(50)  NOT NULL,
    [Fecha_Nacimiento] datetime  NOT NULL,
    [Rut] decimal(8,0)  NOT NULL,
    [Fondo] nvarchar(50)  NOT NULL,
    [Fecha_Ingreso] datetime  NOT NULL,
    [Fecha_Termino_Contrato] datetime  NOT NULL,
    [Calidad_Contrato] nvarchar(50)  NOT NULL,
    [Tipo_Contrato] nvarchar(50)  NOT NULL,
    [Codigo_Contrato] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'Evaluacion_Preguntas'
CREATE TABLE [dbo].[Evaluacion_Preguntas] (
    [Numero_Evaluacion] decimal(18,0)  NOT NULL,
    [Codigo_Proceso] decimal(18,0)  NOT NULL,
    [Codigo_seccion] nchar(10)  NOT NULL,
    [Numero_Pregunta] decimal(18,0)  NOT NULL,
    [Nota] decimal(3,1)  NOT NULL
);
GO

-- Creating table 'Evaluacion_Preguntas_Cargos'
CREATE TABLE [dbo].[Evaluacion_Preguntas_Cargos] (
    [Numero_Evaluacion] decimal(18,0)  NOT NULL,
    [Codigo_Proceso] decimal(18,0)  NOT NULL,
    [Codigo_seccion] nchar(10)  NOT NULL,
    [Cod_Cargo_Evaluado] nvarchar(20)  NOT NULL,
    [Cod_Usuario_Evaluado] nvarchar(20)  NOT NULL,
    [Numero_Pregunta] decimal(18,0)  NOT NULL,
    [Nota] decimal(3,1)  NOT NULL
);
GO

-- Creating table 'Evaluacion_Resultado'
CREATE TABLE [dbo].[Evaluacion_Resultado] (
    [Numero_Evaluacion] decimal(18,0)  NOT NULL,
    [Codigo_Proceso] decimal(18,0)  NOT NULL,
    [Codigo_Seccion] nchar(10)  NOT NULL,
    [Codigo_Usuario] nvarchar(20)  NOT NULL,
    [Nota_F_Seccion] decimal(3,1)  NULL
);
GO

-- Creating table 'Evaluacion_Resultado_Cargos'
CREATE TABLE [dbo].[Evaluacion_Resultado_Cargos] (
    [Numero_Evaluacion] decimal(18,0)  NOT NULL,
    [Codigo_Proceso] decimal(18,0)  NOT NULL,
    [Codigo_Seccion] nchar(10)  NOT NULL,
    [Cod_Cargo_Evaluado] nvarchar(20)  NOT NULL,
    [Cod_Usuario_Evaluado] nvarchar(20)  NOT NULL,
    [Nota_F_Seccion] nchar(10)  NULL
);
GO

-- Creating table 'Evaluaciones'
CREATE TABLE [dbo].[Evaluaciones] (
    [Numero_Evaluacion] decimal(18,0)  NOT NULL,
    [Codigo_Proceso] decimal(18,0)  NOT NULL,
    [Codigo_Cargo] nvarchar(20)  NOT NULL,
    [Fecha] datetime  NULL,
    [Logros] nvarchar(max)  NOT NULL,
    [Metas] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Preguntas_Aleatorias'
CREATE TABLE [dbo].[Preguntas_Aleatorias] (
    [Codigo_Seccion] nchar(10)  NOT NULL,
    [Numero_Pregunta] decimal(18,0)  NOT NULL,
    [Texto_Pregunta] nvarchar(50)  NOT NULL,
    [Ponderacion_P] decimal(18,1)  NOT NULL
);
GO

-- Creating table 'Preguntas_Cargos'
CREATE TABLE [dbo].[Preguntas_Cargos] (
    [Codigo_Cargo] nvarchar(20)  NOT NULL,
    [Cod_Cargo_Evaluado] nvarchar(20)  NOT NULL,
    [Codigo_seccion] nchar(10)  NOT NULL,
    [Nunero_Pregunta] decimal(18,0)  NOT NULL
);
GO

-- Creating table 'Procesos_Evaluacion'
CREATE TABLE [dbo].[Procesos_Evaluacion] (
    [Codigo_Proceso] decimal(18,0)  NOT NULL,
    [Ano_Proceso] decimal(4,0)  NOT NULL,
    [Nombre_Proceso] nvarchar(50)  NOT NULL,
    [Mes_Proceso] decimal(2,0)  NOT NULL,
    [Retroalimentacion] nvarchar(1000)  NULL
);
GO

-- Creating table 'Secciones'
CREATE TABLE [dbo].[Secciones] (
    [Codigo_Seccion] nchar(10)  NOT NULL,
    [Nombre_Seccion] nvarchar(50)  NOT NULL,
    [Ponderacion_S] decimal(18,1)  NOT NULL
);
GO

-- Creating table 'Rol_Operacion'
CREATE TABLE [dbo].[Rol_Operacion] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [IdRol] int  NOT NULL,
    [IdOperacion] int  NOT NULL
);
GO

-- Creating table 'Operacion'
CREATE TABLE [dbo].[Operacion] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Nombre] nvarchar(100)  NOT NULL
);
GO

-- Creating table 'Rol'
CREATE TABLE [dbo].[Rol] (
    [Tipo_Usuario] int IDENTITY(1,1) NOT NULL,
    [Nombre] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'Estado_Usuarios'
CREATE TABLE [dbo].[Estado_Usuarios] (
    [IdState] int  NOT NULL,
    [StateDescription] nvarchar(30)  NOT NULL
);
GO

-- Creating table 'Usuarios'
CREATE TABLE [dbo].[Usuarios] (
    [Codigo_Usuario] nvarchar(20)  NOT NULL,
    [Nombre_Usuario] nvarchar(50)  NOT NULL,
    [Tipo_Usuario] nchar(10)  NOT NULL,
    [Codigo_Cargo] nvarchar(20)  NULL,
    [PASS] nvarchar(20)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Codigo_Cargo] in table 'Cargos'
ALTER TABLE [dbo].[Cargos]
ADD CONSTRAINT [PK_Cargos]
    PRIMARY KEY CLUSTERED ([Codigo_Cargo] ASC);
GO

-- Creating primary key on [Codigo_Cargo], [Cod_Cargo_Evaluado] in table 'Cargos_Evaluadores'
ALTER TABLE [dbo].[Cargos_Evaluadores]
ADD CONSTRAINT [PK_Cargos_Evaluadores]
    PRIMARY KEY CLUSTERED ([Codigo_Cargo], [Cod_Cargo_Evaluado] ASC);
GO

-- Creating primary key on [Codigo_Usuario] in table 'Datos_Usuarios'
ALTER TABLE [dbo].[Datos_Usuarios]
ADD CONSTRAINT [PK_Datos_Usuarios]
    PRIMARY KEY CLUSTERED ([Codigo_Usuario] ASC);
GO

-- Creating primary key on [Numero_Evaluacion], [Codigo_Proceso], [Codigo_seccion], [Numero_Pregunta] in table 'Evaluacion_Preguntas'
ALTER TABLE [dbo].[Evaluacion_Preguntas]
ADD CONSTRAINT [PK_Evaluacion_Preguntas]
    PRIMARY KEY CLUSTERED ([Numero_Evaluacion], [Codigo_Proceso], [Codigo_seccion], [Numero_Pregunta] ASC);
GO

-- Creating primary key on [Numero_Evaluacion], [Codigo_Proceso], [Codigo_seccion], [Cod_Cargo_Evaluado], [Cod_Usuario_Evaluado], [Numero_Pregunta] in table 'Evaluacion_Preguntas_Cargos'
ALTER TABLE [dbo].[Evaluacion_Preguntas_Cargos]
ADD CONSTRAINT [PK_Evaluacion_Preguntas_Cargos]
    PRIMARY KEY CLUSTERED ([Numero_Evaluacion], [Codigo_Proceso], [Codigo_seccion], [Cod_Cargo_Evaluado], [Cod_Usuario_Evaluado], [Numero_Pregunta] ASC);
GO

-- Creating primary key on [Numero_Evaluacion], [Codigo_Proceso], [Codigo_Seccion], [Codigo_Usuario] in table 'Evaluacion_Resultado'
ALTER TABLE [dbo].[Evaluacion_Resultado]
ADD CONSTRAINT [PK_Evaluacion_Resultado]
    PRIMARY KEY CLUSTERED ([Numero_Evaluacion], [Codigo_Proceso], [Codigo_Seccion], [Codigo_Usuario] ASC);
GO

-- Creating primary key on [Numero_Evaluacion], [Codigo_Proceso], [Codigo_Seccion], [Cod_Cargo_Evaluado], [Cod_Usuario_Evaluado] in table 'Evaluacion_Resultado_Cargos'
ALTER TABLE [dbo].[Evaluacion_Resultado_Cargos]
ADD CONSTRAINT [PK_Evaluacion_Resultado_Cargos]
    PRIMARY KEY CLUSTERED ([Numero_Evaluacion], [Codigo_Proceso], [Codigo_Seccion], [Cod_Cargo_Evaluado], [Cod_Usuario_Evaluado] ASC);
GO

-- Creating primary key on [Numero_Evaluacion], [Codigo_Proceso], [Codigo_Cargo] in table 'Evaluaciones'
ALTER TABLE [dbo].[Evaluaciones]
ADD CONSTRAINT [PK_Evaluaciones]
    PRIMARY KEY CLUSTERED ([Numero_Evaluacion], [Codigo_Proceso], [Codigo_Cargo] ASC);
GO

-- Creating primary key on [Codigo_Seccion], [Numero_Pregunta] in table 'Preguntas_Aleatorias'
ALTER TABLE [dbo].[Preguntas_Aleatorias]
ADD CONSTRAINT [PK_Preguntas_Aleatorias]
    PRIMARY KEY CLUSTERED ([Codigo_Seccion], [Numero_Pregunta] ASC);
GO

-- Creating primary key on [Codigo_Cargo], [Cod_Cargo_Evaluado], [Codigo_seccion], [Nunero_Pregunta] in table 'Preguntas_Cargos'
ALTER TABLE [dbo].[Preguntas_Cargos]
ADD CONSTRAINT [PK_Preguntas_Cargos]
    PRIMARY KEY CLUSTERED ([Codigo_Cargo], [Cod_Cargo_Evaluado], [Codigo_seccion], [Nunero_Pregunta] ASC);
GO

-- Creating primary key on [Codigo_Proceso] in table 'Procesos_Evaluacion'
ALTER TABLE [dbo].[Procesos_Evaluacion]
ADD CONSTRAINT [PK_Procesos_Evaluacion]
    PRIMARY KEY CLUSTERED ([Codigo_Proceso] ASC);
GO

-- Creating primary key on [Codigo_Seccion] in table 'Secciones'
ALTER TABLE [dbo].[Secciones]
ADD CONSTRAINT [PK_Secciones]
    PRIMARY KEY CLUSTERED ([Codigo_Seccion] ASC);
GO

-- Creating primary key on [Id] in table 'Rol_Operacion'
ALTER TABLE [dbo].[Rol_Operacion]
ADD CONSTRAINT [PK_Rol_Operacion]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Operacion'
ALTER TABLE [dbo].[Operacion]
ADD CONSTRAINT [PK_Operacion]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Tipo_Usuario] in table 'Rol'
ALTER TABLE [dbo].[Rol]
ADD CONSTRAINT [PK_Rol]
    PRIMARY KEY CLUSTERED ([Tipo_Usuario] ASC);
GO

-- Creating primary key on [IdState] in table 'Estado_Usuarios'
ALTER TABLE [dbo].[Estado_Usuarios]
ADD CONSTRAINT [PK_Estado_Usuarios]
    PRIMARY KEY CLUSTERED ([IdState] ASC);
GO

-- Creating primary key on [Codigo_Usuario] in table 'Usuarios'
ALTER TABLE [dbo].[Usuarios]
ADD CONSTRAINT [PK_Usuarios]
    PRIMARY KEY CLUSTERED ([Codigo_Usuario] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------