-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 07-05-2025 a las 01:07:20
-- Versión del servidor: 10.4.32-MariaDB
-- Versión de PHP: 8.0.30

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `inmobiliaria`
--

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `contrato`
--

CREATE TABLE `contrato` (
  `ID_Contrato` int(11) NOT NULL,
  `id_inmueble` int(11) NOT NULL,
  `id_inquilino` int(11) NOT NULL,
  `fecha_inicio` date NOT NULL,
  `fecha_fin` date NOT NULL,
  `monto_alquiler` decimal(10,2) NOT NULL CHECK (`monto_alquiler` > 0),
  `multa` decimal(10,2) DEFAULT 0.00 CHECK (`multa` >= 0),
  `estado` enum('Vigente','Terminado','Anulado') NOT NULL,
  `EstadoLogico` tinyint(1) NOT NULL,
  `ID_UsuarioAlta` int(20) NOT NULL,
  `ID_UsuarioBaja` int(2) NOT NULL,
  `ID_UsuarioAnulacion` tinyint(2) DEFAULT NULL,
  `Fecha_FinAnt` date DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `contrato`
--

INSERT INTO `contrato` (`ID_Contrato`, `id_inmueble`, `id_inquilino`, `fecha_inicio`, `fecha_fin`, `monto_alquiler`, `multa`, `estado`, `EstadoLogico`, `ID_UsuarioAlta`, `ID_UsuarioBaja`, `ID_UsuarioAnulacion`, `Fecha_FinAnt`) VALUES
(1, 1, 1, '2025-05-06', '2026-05-06', 1237.00, 2474.00, 'Anulado', 0, 2, 2, 3, '2025-06-12'),
(2, 2, 2, '2026-05-06', '2027-05-06', 50000.00, 0.00, 'Vigente', 1, 3, 0, NULL, NULL);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `imagenes`
--

CREATE TABLE `imagenes` (
  `Id` int(11) NOT NULL,
  `InmuebleId` int(11) NOT NULL,
  `Url` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inmueble`
--

CREATE TABLE `inmueble` (
  `id` int(11) NOT NULL,
  `direccion` varchar(100) NOT NULL,
  `uso` enum('Comercial','Residencial') NOT NULL,
  `cantidad_ambientes` int(11) NOT NULL CHECK (`cantidad_ambientes` > 0),
  `coordenadas` varchar(255) DEFAULT NULL,
  `precio` decimal(10,2) NOT NULL CHECK (`precio` > 0),
  `estado` enum('Disponible','No disponible','Suspendido') NOT NULL,
  `id_propietario` int(11) NOT NULL,
  `Portada` varchar(500) DEFAULT NULL,
  `Imagenes` varchar(500) NOT NULL,
  `id_tipoinmueble` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `inmueble`
--

INSERT INTO `inmueble` (`id`, `direccion`, `uso`, `cantidad_ambientes`, `coordenadas`, `precio`, `estado`, `id_propietario`, `Portada`, `Imagenes`, `id_tipoinmueble`) VALUES
(1, 'pje las alinas', 'Residencial', 2, '3711581', 450000.00, 'Disponible', 1, NULL, '', 1),
(2, 'la punta 456', 'Residencial', 3, '45269787-4593635696', 123.00, 'Disponible', 3, '/Uploads/Portadas/ef54984a-653c-453b-a01f-bdccd1db388a.jpg', '', 3);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inquilino`
--

CREATE TABLE `inquilino` (
  `id` int(11) NOT NULL,
  `nombre` varchar(20) NOT NULL,
  `apellido` varchar(20) NOT NULL,
  `dni` varchar(20) NOT NULL,
  `telefono` varchar(20) NOT NULL,
  `email` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `inquilino`
--

INSERT INTO `inquilino` (`id`, `nombre`, `apellido`, `dni`, `telefono`, `email`) VALUES
(1, 'Gonzalo', 'Noe', '28989745', '265498799', 'g@n.com'),
(2, 'Guadalupe', 'Filgar', '8999665', '2654879898', 'g@f.com');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pago`
--

CREATE TABLE `pago` (
  `id` int(11) NOT NULL,
  `id_contrato` int(11) NOT NULL,
  `fecha_pago` date DEFAULT NULL,
  `Periodo_Pago` date DEFAULT NULL,
  `monto` decimal(10,2) NOT NULL CHECK (`monto` > 0),
  `Pagado` tinyint(1) NOT NULL,
  `EsMulta` tinyint(1) NOT NULL,
  `Descripcion` varchar(200) DEFAULT NULL,
  `Estado` tinyint(1) NOT NULL DEFAULT 1,
  `ID_UsuarioAlta` int(2) NOT NULL,
  `ID_UsuarioBaja` int(2) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `pago`
--

INSERT INTO `pago` (`id`, `id_contrato`, `fecha_pago`, `Periodo_Pago`, `monto`, `Pagado`, `EsMulta`, `Descripcion`, `Estado`, `ID_UsuarioAlta`, `ID_UsuarioBaja`) VALUES
(1, 2, '2026-05-06', '2026-05-01', 50000.00, 1, 0, 'Se genera el pago correspondiente al mes de Mayo,  lo paga en la oficia.', 1, 3, 0),
(2, 1, '2025-05-15', '2025-05-01', 2474.00, 1, 1, 'Se Agrega la multa correspondiente a cancelar el contrato.Lo pago en efectivo en la oficina.', 1, 3, 0),
(3, 2, NULL, '2026-06-01', 50000.00, 0, 0, NULL, 1, 3, 0),
(4, 2, NULL, '2026-07-01', 50000.00, 0, 0, '', 1, 3, 0),
(5, 2, NULL, '2026-08-01', 50000.00, 0, 0, NULL, 1, 3, 0),
(6, 2, NULL, '2026-09-01', 50000.00, 0, 0, NULL, 1, 3, 0),
(7, 2, NULL, '2026-10-01', 50000.00, 0, 0, NULL, 1, 3, 0),
(8, 2, NULL, '2026-11-01', 50000.00, 0, 0, NULL, 1, 3, 0),
(9, 2, NULL, '2026-12-01', 50000.00, 0, 0, NULL, 1, 3, 0),
(10, 2, NULL, '2027-01-01', 50000.00, 0, 0, NULL, 1, 3, 0),
(11, 2, NULL, '2027-02-01', 50000.00, 0, 0, NULL, 1, 3, 0),
(12, 2, NULL, '2027-03-01', 50000.00, 0, 0, NULL, 1, 3, 0),
(13, 2, NULL, '2027-04-01', 50000.00, 0, 0, NULL, 1, 3, 0),
(14, 2, '2025-06-15', '2027-05-01', 50000.00, 1, 0, 'Se abona el mes de Mayo/2027 por delantado. Probando editar pagos, funciona pero hay que cargar siempre el valor', 1, 3, 0);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `propietario`
--

CREATE TABLE `propietario` (
  `id` int(11) NOT NULL,
  `nombre` varchar(20) NOT NULL,
  `apellido` varchar(20) NOT NULL,
  `dni` varchar(20) NOT NULL,
  `telefono` varchar(20) NOT NULL,
  `email` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `propietario`
--

INSERT INTO `propietario` (`id`, `nombre`, `apellido`, `dni`, `telefono`, `email`) VALUES
(1, 'Martin', 'Norton', '25633988', '02665119568', 'martinnorton07@gmail.com'),
(2, 'Jose', 'Juarez', '21555666', '2659659898', 'jj@jj.com'),
(3, 'Juan', 'Juan', '21756984', '2665559998', 'j@j.com');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `tipoinmueble`
--

CREATE TABLE `tipoinmueble` (
  `Id` int(3) NOT NULL,
  `Nombre` varchar(30) NOT NULL,
  `Descripcion` varchar(50) NOT NULL,
  `Estado` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `tipoinmueble`
--

INSERT INTO `tipoinmueble` (`Id`, `Nombre`, `Descripcion`, `Estado`) VALUES
(1, 'Local Comercial', 'Espacio Utilizado con fines comerciales', 1),
(2, 'Edificio de oficinas', 'Espacio Utilizado con fines de oficinas ', 1),
(3, 'Residencia/vivienda', 'Espacio Utilizado con fines de Vivienda', 1),
(4, 'Departamento residencial', 'departamento dstinado a vivienda', 1),
(5, 'Salon de eventos', 'Inmueble destinado a fiestas de hasta 500 personas', 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `usuarios`
--

CREATE TABLE `usuarios` (
  `Id` int(11) NOT NULL,
  `Nombre` varchar(100) NOT NULL,
  `Apellido` varchar(100) NOT NULL,
  `Email` varchar(255) NOT NULL,
  `Clave` varchar(255) NOT NULL,
  `Avatar` varchar(500) DEFAULT NULL,
  `Rol` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `usuarios`
--

INSERT INTO `usuarios` (`Id`, `Nombre`, `Apellido`, `Email`, `Clave`, `Avatar`, `Rol`) VALUES
(2, 'Administrador', 'Sistema', 'admin@admin.com', '4pca3VVRbpiLnTP88XuZ1vUJnwqwLXPVyogt5itCYgM=', NULL, 1),
(3, 'Inmobiliaria', 'Las Rosas', 'inmob@lasrosas.com', 'NKyNEXvljeuHjx1Ozew6AZalvCHVOD69CKNJq46z6D8=', '/Uploads/Avatars\\avatar_3.jpg', 2);

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `contrato`
--
ALTER TABLE `contrato`
  ADD PRIMARY KEY (`ID_Contrato`),
  ADD KEY `id_inmueble` (`id_inmueble`),
  ADD KEY `id_inquilino` (`id_inquilino`);

--
-- Indices de la tabla `imagenes`
--
ALTER TABLE `imagenes`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `InmuebleId` (`InmuebleId`);

--
-- Indices de la tabla `inmueble`
--
ALTER TABLE `inmueble`
  ADD PRIMARY KEY (`id`),
  ADD KEY `id_propietario` (`id_propietario`),
  ADD KEY `FK_Inmueble_TipoInmueble` (`id_tipoinmueble`);

--
-- Indices de la tabla `inquilino`
--
ALTER TABLE `inquilino`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `dni` (`dni`);

--
-- Indices de la tabla `pago`
--
ALTER TABLE `pago`
  ADD PRIMARY KEY (`id`),
  ADD KEY `id_contrato` (`id_contrato`);

--
-- Indices de la tabla `propietario`
--
ALTER TABLE `propietario`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `dni` (`dni`);

--
-- Indices de la tabla `tipoinmueble`
--
ALTER TABLE `tipoinmueble`
  ADD PRIMARY KEY (`Id`);

--
-- Indices de la tabla `usuarios`
--
ALTER TABLE `usuarios`
  ADD PRIMARY KEY (`Id`),
  ADD UNIQUE KEY `Email_UNIQUE` (`Email`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `contrato`
--
ALTER TABLE `contrato`
  MODIFY `ID_Contrato` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT de la tabla `imagenes`
--
ALTER TABLE `imagenes`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `inmueble`
--
ALTER TABLE `inmueble`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT de la tabla `inquilino`
--
ALTER TABLE `inquilino`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT de la tabla `pago`
--
ALTER TABLE `pago`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=15;

--
-- AUTO_INCREMENT de la tabla `propietario`
--
ALTER TABLE `propietario`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT de la tabla `tipoinmueble`
--
ALTER TABLE `tipoinmueble`
  MODIFY `Id` int(3) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT de la tabla `usuarios`
--
ALTER TABLE `usuarios`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `contrato`
--
ALTER TABLE `contrato`
  ADD CONSTRAINT `contrato_ibfk_1` FOREIGN KEY (`id_inmueble`) REFERENCES `inmueble` (`id`) ON DELETE CASCADE,
  ADD CONSTRAINT `contrato_ibfk_2` FOREIGN KEY (`id_inquilino`) REFERENCES `inquilino` (`id`) ON DELETE CASCADE;

--
-- Filtros para la tabla `imagenes`
--
ALTER TABLE `imagenes`
  ADD CONSTRAINT `imagenes_ibfk_1` FOREIGN KEY (`InmuebleId`) REFERENCES `inmueble` (`id`) ON DELETE CASCADE;

--
-- Filtros para la tabla `inmueble`
--
ALTER TABLE `inmueble`
  ADD CONSTRAINT `FK_Inmueble_TipoInmueble` FOREIGN KEY (`id_tipoinmueble`) REFERENCES `tipoinmueble` (`Id`),
  ADD CONSTRAINT `inmueble_ibfk_1` FOREIGN KEY (`id_propietario`) REFERENCES `propietario` (`id`) ON DELETE CASCADE;

--
-- Filtros para la tabla `pago`
--
ALTER TABLE `pago`
  ADD CONSTRAINT `pago_ibfk_1` FOREIGN KEY (`id_contrato`) REFERENCES `contrato` (`ID_Contrato`) ON DELETE CASCADE ON UPDATE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
