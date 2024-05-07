--alter database kanban set single_user with rollback immediate
--GO
--drop database kanban
--GO


create database kanban;
go
use kanban;
go

create table kbn_usuario(
    id int not null primary key identity(1,1),
    nombre nvarchar(max) not null,
    correo nvarchar(max) not null
);

create table kbn_usuarioPassword(
    id int not null primary key identity(1,1),
    usuarioId int not null,
    pass nvarchar(max) not null,
    
    foreign key (usuarioId) references kbn_usuario(id)
);

create table kbn_proyecto(
    id int not null primary key identity(1,1),
    nombre nvarchar(max) not null,
    codigoProyecto nvarchar(max) not null,
);

create table kbn_tipoIncidencia(
    id int not null primary key identity(1,1),
    nombre nvarchar(max) not null,
);
create table kbn_tipoTarea(
    id int not null primary key identity(1,1),
    nombre nvarchar(max) not null,
);
create table kbn_estado(
    id int not null primary key identity(1,1),
    nombre nvarchar(max) not null,
);
create table kbn_incidencia(
    id int not null primary key identity(1,1),
    nombre nvarchar(max) not null,
    tipoIncidenciaId int not null,
    estadoId int not null,
    usuarioCreadorId int not null,
    usuarioResponsableId int not null,
    fechaCreacion datetime not null,
    proyectoId int not null,
    descripcion nvarchar(max),
    -- estimacion decimal, -- se calculara sumando las estimaciones de las tareas
    -- ejecucion decimal,  -- se calculara sumando las ejecuciones de las tareas

    foreign key (proyectoId) references kbn_proyecto(id),
    foreign key (tipoIncidenciaId) references kbn_tipoIncidencia(id),
    foreign key (estadoId) references kbn_estado(id),
    foreign key (usuarioCreadorId) references kbn_usuario(id),
    foreign key (usuarioResponsableId) references kbn_usuario(id)
);
create table kbn_tarea(
    id int not null primary key identity(1,1),
    nombre nvarchar(max) not null,
    tipoTareaId int not null,
    estadoId int not null,
    usuarioCreadorId int not null,
    usuarioResponsableId int ,
    incidenciaId int not null,
    fechaCreacion datetime not null,
    estimacion decimal default 0,
    ejecucion decimal default 0,
    semanaDeEjecucionPlanificada nvarchar(max),
    semanaDeEjecucionReal nvarchar(max),
    descripcion nvarchar,

    foreign key (tipoTareaId) references kbn_tipoTarea(id),
    foreign key (estadoId) references kbn_estado(id),
    foreign key (usuarioCreadorId) references kbn_usuario(id),
    foreign key (usuarioResponsableId) references kbn_usuario(id),
    foreign key (incidenciaId) references kbn_incidencia(id)
);

create table kbn_registroTiempo(
    id int not null primary key identity(1,1),
    usuarioId int not null,
    tareaId int not null,
    estadoTareaId int not null,
    ejecucion decimal default 0,
    descripcion nvarchar(max),
    fechaEjecucion datetime not null,

    fechaRegistro datetime not null
);


create table kbn_log(
    id int not null primary key identity(1,1),
    usuarioId int not null,
    entidad nvarchar(max) not null,
    entidadId int not null,
    accion nvarchar(max) not null,
    fechaHora datetime not null,
    detalles nvarchar(max)
);

USE [kanban]
GO

SET IDENTITY_INSERT kbn_usuario ON 
GO
INSERT kbn_usuario (id,nombre,correo) VALUES (1,N'ADMIN_1','PDV1')
GO
SET IDENTITY_INSERT kbn_usuario OFF 
GO

SET IDENTITY_INSERT kbn_usuarioPassword ON 
GO
INSERT kbn_usuarioPassword (id,usuarioId,pass) VALUES (1,1,'Pablo01+')
GO
SET IDENTITY_INSERT kbn_usuarioPassword OFF 
GO

SET IDENTITY_INSERT kbn_tipoIncidencia ON 
GO
INSERT kbn_tipoIncidencia (id,nombre) VALUES (1, N'Incidencia')
GO
INSERT kbn_tipoIncidencia (id,nombre) VALUES (2, N'Bug')
GO
INSERT kbn_tipoIncidencia (id,nombre) VALUES (3, N'Evolutivo')
GO
INSERT kbn_tipoIncidencia (id,nombre) VALUES (4, N'Nueva Funcionalidad')
GO
INSERT kbn_tipoIncidencia (id,nombre) VALUES (5, N'Investigacion/Factibilidad')
GO
SET IDENTITY_INSERT kbn_tipoIncidencia OFF
GO

GO
SET IDENTITY_INSERT kbn_tipoTarea ON 
GO
INSERT kbn_tipoTarea (id,nombre) VALUES (1, N'Analisis Funcional')
GO
INSERT kbn_tipoTarea (id,nombre) VALUES (2, N'Desarrollo')
GO
INSERT kbn_tipoTarea (id,nombre) VALUES (3, N'Testing')
GO
INSERT kbn_tipoTarea (id,nombre) VALUES (4, N'Deploy')
GO
INSERT kbn_tipoTarea (id,nombre) VALUES (5, N'Prueba de Usuario (PU)')
GO
INSERT kbn_tipoTarea (id,nombre) VALUES (6, N'Reuniones/Dailies')
GO
SET IDENTITY_INSERT kbn_tipoTarea OFF
GO

GO
SET IDENTITY_INSERT kbn_estado ON 
GO
INSERT kbn_estado (id,nombre) VALUES (1, N'Backlog')
GO
INSERT kbn_estado (id,nombre) VALUES (2, N'Por Hacer')
GO
INSERT kbn_estado (id,nombre) VALUES (3, N'En Curso')
GO
INSERT kbn_estado (id,nombre) VALUES (4, N'Finalizado')
GO
INSERT kbn_estado (id,nombre) VALUES (5, N'Bloqueado')
GO
INSERT kbn_estado (id,nombre) VALUES (6, N'Cerrado')
GO

SET IDENTITY_INSERT kbn_estado OFF
GO