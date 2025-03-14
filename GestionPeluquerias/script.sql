/****** Object:  Table [dbo].[Servicios]    Script Date: 12/03/2025 13:48:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Servicios](
	[IdServicio] [int] NOT NULL,
	[Nombre] [nvarchar](100) NOT NULL,
	[Descripcion] [nvarchar](255) NULL,
	[Precio] [decimal](10, 2) NOT NULL,
	[Duracion] [int] NOT NULL,
	[IdPeluqueria] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdServicio] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Peluqueros]    Script Date: 12/03/2025 13:48:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Peluqueros](
	[IdPeluquero] [int] NOT NULL,
	[IdUsuario] [int] NOT NULL,
	[IdPeluqueria] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdPeluquero] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[vw_ServiciosPeluquerosPeluquerias]    Script Date: 12/03/2025 13:48:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_ServiciosPeluquerosPeluquerias] AS
SELECT 
    s.Nombre AS ServicioNombre,
    s.Descripcion AS ServicioDescripcion,
    s.Precio AS ServicioPrecio,
    s.Duracion AS ServicioDuracion,
    u.Nombre AS PeluqueroNombre,
    u.Apellido AS PeluqueroApellido,
    p.Nombre AS PeluqueriaNombre
FROM Servicios s
JOIN Peluquerias p ON s.IdPeluqueria = p.IdPeluqueria
JOIN Peluqueros pe ON p.IdPeluqueria = pe.IdPeluqueria
JOIN Usuarios u ON pe.IdUsuario = u.IdUsuario;
GO
/****** Object:  View [dbo].[vw_DetallesPeluqueria]    Script Date: 12/03/2025 13:48:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_DetallesPeluqueria] AS
SELECT 
    CAST(ROW_NUMBER() OVER (ORDER BY p.IdPeluqueria) AS INT) AS Id, -- 🔹 Convertimos BIGINT a INT
    CAST(p.IdPeluqueria AS INT) AS IdPeluqueria,
    p.Nombre AS NombrePeluqueria,
    ISNULL(p.Direccion, 'No disponible') AS Direccion, -- 🔹 Evita valores NULL
    ISNULL(p.Telefono, 'No disponible') AS Telefono, 
    p.HorarioApertura,
    p.HorarioCierre,
    ISNULL(u.Nombre, 'No asignado') AS NombreAdministrador, 

    CAST(ISNULL(pe.IdPeluquero, 0) AS INT) AS IdPeluquero, -- 🔹 Si no hay peluquero, pone 0
    CAST(ISNULL(pe.IdUsuario, 0) AS INT) AS IdUsuarioPeluquero,
    ISNULL(up.Nombre, 'Sin Peluquero') AS NombrePeluquero,

    CAST(ISNULL(s.IdServicio, 0) AS INT) AS IdServicio,
    ISNULL(s.Nombre, 'Sin Servicio') AS NombreServicio,
    ISNULL(s.Descripcion, 'Sin descripción') AS Descripcion,
    CAST(ISNULL(s.Precio, 0) AS DECIMAL(10,2)) AS PrecioServicio, -- 🔹 Si no hay precio, pone 0
    CAST(ISNULL(s.Duracion, 0) AS INT) AS Duracion -- 🔹 Si no hay duración, pone 0
FROM Peluquerias p
LEFT JOIN Usuarios u ON p.IdUsuario = u.IdUsuario
LEFT JOIN Peluqueros pe ON p.IdPeluqueria = pe.IdPeluqueria
LEFT JOIN Usuarios up ON pe.IdUsuario = up.IdUsuario
LEFT JOIN Servicios s ON p.IdPeluqueria = s.IdPeluqueria;
GO
/****** Object:  Table [dbo].[Citas]    Script Date: 12/03/2025 13:48:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Citas](
	[IdCita] [int] NOT NULL,
	[IdUsuario] [int] NOT NULL,
	[IdPeluqueria] [int] NOT NULL,
	[IdPeluquero] [int] NOT NULL,
	[IdServicio] [int] NOT NULL,
	[FechaCita] [date] NOT NULL,
	[HoraCita] [time](7) NOT NULL,
	[Estado] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdCita] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 12/03/2025 13:48:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[IdRol] [int] NOT NULL,
	[Nombre] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdRol] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[Citas] ([IdCita], [IdUsuario], [IdPeluqueria], [IdPeluquero], [IdServicio], [FechaCita], [HoraCita], [Estado]) VALUES (1, 7, 1, 1, 1, CAST(N'2025-03-12' AS Date), CAST(N'10:00:00' AS Time), 1)
INSERT [dbo].[Citas] ([IdCita], [IdUsuario], [IdPeluqueria], [IdPeluquero], [IdServicio], [FechaCita], [HoraCita], [Estado]) VALUES (2, 8, 1, 2, 2, CAST(N'2025-03-12' AS Date), CAST(N'11:30:00' AS Time), 1)
INSERT [dbo].[Citas] ([IdCita], [IdUsuario], [IdPeluqueria], [IdPeluquero], [IdServicio], [FechaCita], [HoraCita], [Estado]) VALUES (3, 9, 1, 1, 3, CAST(N'2025-03-13' AS Date), CAST(N'16:00:00' AS Time), 1)
INSERT [dbo].[Citas] ([IdCita], [IdUsuario], [IdPeluqueria], [IdPeluquero], [IdServicio], [FechaCita], [HoraCita], [Estado]) VALUES (4, 7, 1, 2, 4, CAST(N'2025-03-15' AS Date), CAST(N'09:30:00' AS Time), 0)
INSERT [dbo].[Citas] ([IdCita], [IdUsuario], [IdPeluqueria], [IdPeluquero], [IdServicio], [FechaCita], [HoraCita], [Estado]) VALUES (5, 10, 2, 3, 5, CAST(N'2025-03-12' AS Date), CAST(N'12:00:00' AS Time), 1)
INSERT [dbo].[Citas] ([IdCita], [IdUsuario], [IdPeluqueria], [IdPeluquero], [IdServicio], [FechaCita], [HoraCita], [Estado]) VALUES (6, 8, 2, 4, 6, CAST(N'2025-03-14' AS Date), CAST(N'17:30:00' AS Time), 1)
INSERT [dbo].[Citas] ([IdCita], [IdUsuario], [IdPeluqueria], [IdPeluquero], [IdServicio], [FechaCita], [HoraCita], [Estado]) VALUES (7, 9, 2, 4, 7, CAST(N'2025-03-16' AS Date), CAST(N'10:30:00' AS Time), 1)
INSERT [dbo].[Citas] ([IdCita], [IdUsuario], [IdPeluqueria], [IdPeluquero], [IdServicio], [FechaCita], [HoraCita], [Estado]) VALUES (8, 10, 2, 3, 8, CAST(N'2025-03-17' AS Date), CAST(N'15:00:00' AS Time), 0)
INSERT [dbo].[Citas] ([IdCita], [IdUsuario], [IdPeluqueria], [IdPeluquero], [IdServicio], [FechaCita], [HoraCita], [Estado]) VALUES (9, 10, 1, 2, 2, CAST(N'2025-03-12' AS Date), CAST(N'10:00:00' AS Time), 1)
GO
INSERT [dbo].[Peluquerias] ([IdPeluqueria], [Nombre], [Direccion], [Latitud], [Longitud], [Telefono], [HorarioApertura], [HorarioCierre], [IdUsuario]) VALUES (1, N'Estilo Perfecto', N'Calle Mayor 15, Madrid', 40.416775, -3.70379, N'911234567', CAST(N'09:00:00' AS Time), CAST(N'20:00:00' AS Time), 1)
INSERT [dbo].[Peluquerias] ([IdPeluqueria], [Nombre], [Direccion], [Latitud], [Longitud], [Telefono], [HorarioApertura], [HorarioCierre], [IdUsuario]) VALUES (2, N'Cortes Modernos', N'Avenida Diagonal 250, Barcelona', 41.385063, 2.173404, N'932345678', CAST(N'09:30:00' AS Time), CAST(N'20:30:00' AS Time), 2)
GO
INSERT [dbo].[Peluqueros] ([IdPeluquero], [IdUsuario], [IdPeluqueria]) VALUES (1, 3, 1)
INSERT [dbo].[Peluqueros] ([IdPeluquero], [IdUsuario], [IdPeluqueria]) VALUES (2, 4, 1)
INSERT [dbo].[Peluqueros] ([IdPeluquero], [IdUsuario], [IdPeluqueria]) VALUES (3, 5, 2)
INSERT [dbo].[Peluqueros] ([IdPeluquero], [IdUsuario], [IdPeluqueria]) VALUES (4, 6, 2)
GO
INSERT [dbo].[Roles] ([IdRol], [Nombre]) VALUES (1, N'Admin')
INSERT [dbo].[Roles] ([IdRol], [Nombre]) VALUES (3, N'Cliente')
INSERT [dbo].[Roles] ([IdRol], [Nombre]) VALUES (2, N'Peluquero')
GO
INSERT [dbo].[Servicios] ([IdServicio], [Nombre], [Descripcion], [Precio], [Duracion], [IdPeluqueria]) VALUES (1, N'Corte Caballero', N'Corte de pelo para hombre', CAST(15.00 AS Decimal(10, 2)), 30, 1)
INSERT [dbo].[Servicios] ([IdServicio], [Nombre], [Descripcion], [Precio], [Duracion], [IdPeluqueria]) VALUES (2, N'Corte Señora', N'Corte y peinado para mujer', CAST(25.00 AS Decimal(10, 2)), 45, 1)
INSERT [dbo].[Servicios] ([IdServicio], [Nombre], [Descripcion], [Precio], [Duracion], [IdPeluqueria]) VALUES (3, N'Tinte', N'Aplicación de color', CAST(35.00 AS Decimal(10, 2)), 90, 1)
INSERT [dbo].[Servicios] ([IdServicio], [Nombre], [Descripcion], [Precio], [Duracion], [IdPeluqueria]) VALUES (4, N'Mechas', N'Mechas con papel o gorro', CAST(45.00 AS Decimal(10, 2)), 120, 1)
INSERT [dbo].[Servicios] ([IdServicio], [Nombre], [Descripcion], [Precio], [Duracion], [IdPeluqueria]) VALUES (5, N'Corte Clásico', N'Corte tradicional para hombre', CAST(12.00 AS Decimal(10, 2)), 25, 2)
INSERT [dbo].[Servicios] ([IdServicio], [Nombre], [Descripcion], [Precio], [Duracion], [IdPeluqueria]) VALUES (6, N'Corte y Lavado', N'Corte, lavado y peinado', CAST(28.00 AS Decimal(10, 2)), 50, 2)
INSERT [dbo].[Servicios] ([IdServicio], [Nombre], [Descripcion], [Precio], [Duracion], [IdPeluqueria]) VALUES (7, N'Coloración', N'Tinte completo', CAST(40.00 AS Decimal(10, 2)), 100, 2)
INSERT [dbo].[Servicios] ([IdServicio], [Nombre], [Descripcion], [Precio], [Duracion], [IdPeluqueria]) VALUES (8, N'Tratamiento Capilar', N'Hidratación y nutrición profunda', CAST(30.00 AS Decimal(10, 2)), 60, 2)
GO
INSERT [dbo].[Usuarios] ([IdUsuario], [Nombre], [Apellido], [Email], [Password], [Telefono], [IdRol]) VALUES (1, N'Carlos', N'Rodríguez', N'carlos.admin@peluapp.com', N'Admin123!', N'612345678', 1)
INSERT [dbo].[Usuarios] ([IdUsuario], [Nombre], [Apellido], [Email], [Password], [Telefono], [IdRol]) VALUES (2, N'Ana', N'Martínez', N'ana.admin@peluapp.com', N'Admin456!', N'623456789', 1)
INSERT [dbo].[Usuarios] ([IdUsuario], [Nombre], [Apellido], [Email], [Password], [Telefono], [IdRol]) VALUES (3, N'Miguel', N'García', N'miguel.peluquero@peluapp.com', N'Pelu123!', N'634567890', 2)
INSERT [dbo].[Usuarios] ([IdUsuario], [Nombre], [Apellido], [Email], [Password], [Telefono], [IdRol]) VALUES (4, N'Laura', N'Fernández', N'laura.peluquera@peluapp.com', N'Pelu456!', N'645678901', 2)
INSERT [dbo].[Usuarios] ([IdUsuario], [Nombre], [Apellido], [Email], [Password], [Telefono], [IdRol]) VALUES (5, N'David', N'López', N'david.peluquero@peluapp.com', N'Pelu789!', N'656789012', 2)
INSERT [dbo].[Usuarios] ([IdUsuario], [Nombre], [Apellido], [Email], [Password], [Telefono], [IdRol]) VALUES (6, N'Sofía', N'Sánchez', N'sofia.peluquera@peluapp.com', N'Pelu012!', N'667890123', 2)
INSERT [dbo].[Usuarios] ([IdUsuario], [Nombre], [Apellido], [Email], [Password], [Telefono], [IdRol]) VALUES (7, N'Juan', N'Pérez', N'juan.perez@email.com', N'Cliente123!', N'678901234', 3)
INSERT [dbo].[Usuarios] ([IdUsuario], [Nombre], [Apellido], [Email], [Password], [Telefono], [IdRol]) VALUES (8, N'María', N'González', N'maria.gonzalez@email.com', N'Cliente456!', N'689012345', 3)
INSERT [dbo].[Usuarios] ([IdUsuario], [Nombre], [Apellido], [Email], [Password], [Telefono], [IdRol]) VALUES (9, N'Pedro', N'Díaz', N'pedro.diaz@email.com', N'Cliente789!', N'690123456', 3)
INSERT [dbo].[Usuarios] ([IdUsuario], [Nombre], [Apellido], [Email], [Password], [Telefono], [IdRol]) VALUES (10, N'Elena', N'Ruiz', N'elena.ruiz@email.com', N'Cliente012!', N'601234567', 3)
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Roles__75E3EFCFE1DDC3A5]    Script Date: 12/03/2025 13:48:21 ******/
ALTER TABLE [dbo].[Roles] ADD UNIQUE NONCLUSTERED 
(
	[Nombre] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Usuarios__A9D105340AA88177]    Script Date: 12/03/2025 13:48:21 ******/
ALTER TABLE [dbo].[Usuarios] ADD UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Citas] ADD  DEFAULT ((1)) FOR [Estado]
GO
ALTER TABLE [dbo].[Citas]  WITH CHECK ADD FOREIGN KEY([IdPeluqueria])
REFERENCES [dbo].[Peluquerias] ([IdPeluqueria])
GO
ALTER TABLE [dbo].[Citas]  WITH CHECK ADD FOREIGN KEY([IdPeluquero])
REFERENCES [dbo].[Peluqueros] ([IdPeluquero])
GO
ALTER TABLE [dbo].[Citas]  WITH CHECK ADD FOREIGN KEY([IdServicio])
REFERENCES [dbo].[Servicios] ([IdServicio])
GO
ALTER TABLE [dbo].[Citas]  WITH CHECK ADD FOREIGN KEY([IdUsuario])
REFERENCES [dbo].[Usuarios] ([IdUsuario])
GO
ALTER TABLE [dbo].[Peluquerias]  WITH CHECK ADD FOREIGN KEY([IdUsuario])
REFERENCES [dbo].[Usuarios] ([IdUsuario])
GO
ALTER TABLE [dbo].[Peluqueros]  WITH CHECK ADD FOREIGN KEY([IdPeluqueria])
REFERENCES [dbo].[Peluquerias] ([IdPeluqueria])
GO
ALTER TABLE [dbo].[Peluqueros]  WITH CHECK ADD FOREIGN KEY([IdUsuario])
REFERENCES [dbo].[Usuarios] ([IdUsuario])
GO
ALTER TABLE [dbo].[Servicios]  WITH CHECK ADD FOREIGN KEY([IdPeluqueria])
REFERENCES [dbo].[Peluquerias] ([IdPeluqueria])
GO
ALTER TABLE [dbo].[Usuarios]  WITH CHECK ADD FOREIGN KEY([IdRol])
REFERENCES [dbo].[Roles] ([IdRol])
GO
