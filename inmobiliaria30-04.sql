-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 30-04-2025 a las 16:35:38
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
  `ID_UsuarioBaja` int(2) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `contrato`
--

INSERT INTO `contrato` (`ID_Contrato`, `id_inmueble`, `id_inquilino`, `fecha_inicio`, `fecha_fin`, `monto_alquiler`, `multa`, `estado`, `EstadoLogico`, `ID_UsuarioAlta`, `ID_UsuarioBaja`) VALUES
(4, 1, 1, '2025-04-05', '2025-04-29', 330000.00, 495000.00, '', 0, 0, 1),
(5, 3, 3, '2025-04-15', '2028-04-15', 449000.00, 0.00, 'Vigente', 0, 0, 1),
(6, 2, 1, '2025-04-15', '2027-06-06', 450000.00, 0.00, 'Vigente', 0, 0, 1),
(8, 6, 1, '2025-05-03', '2025-06-03', 500000.00, 0.00, 'Vigente', 1, 0, 0),
(9, 1, 2, '2030-06-05', '2031-06-05', 789.00, 0.00, 'Vigente', 1, 0, 0),
(10, 3, 1, '2030-01-01', '2030-02-01', 567.00, 0.00, 'Vigente', 1, 0, 0),
(11, 6, 1, '2030-03-30', '2031-03-30', 123.00, 0.00, 'Vigente', 1, 3, 0),
(12, 2, 1, '2030-01-01', '2025-04-29', 895.00, 0.00, 'Vigente', 0, 1, 1),
(13, 2, 1, '2031-01-01', '2032-01-01', 564.00, 0.00, 'Terminado', 1, 1, 0),
(14, 2, 1, '2032-01-02', '2033-01-02', 1230.00, 0.00, 'Vigente', 1, 1, 0);

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
  `tipo` varchar(50) NOT NULL,
  `cantidad_ambientes` int(11) NOT NULL CHECK (`cantidad_ambientes` > 0),
  `coordenadas` varchar(255) DEFAULT NULL,
  `precio` decimal(10,2) NOT NULL CHECK (`precio` > 0),
  `estado` enum('Disponible','No disponible','Suspendido') NOT NULL,
  `id_propietario` int(11) NOT NULL,
  `Portada` varchar(500) DEFAULT NULL,
  `Imagenes` varchar(500) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `inmueble`
--

INSERT INTO `inmueble` (`id`, `direccion`, `uso`, `tipo`, `cantidad_ambientes`, `coordenadas`, `precio`, `estado`, `id_propietario`, `Portada`, `Imagenes`) VALUES
(1, '867 Pasaje Las Salinas', 'Residencial', 'departamento', 2, '44569865', 70000.00, 'Disponible', 2, '/Uploads/Portadas/ba4debcf-d6b8-451d-9a4c-9acb892c0f70.jpg', ''),
(2, 'Las Lomas 356, bº las lomas, San Luis', 'Residencial', 'casa', 5, '45269787-4593635696', 80000000.00, 'Disponible', 3, '/Uploads/Portadas/3b9c084d-fff6-40ff-9f13-7208462eb6dc.avif', ''),
(3, 'Pringles 1445, Ciudad de San Luis, San Luis', 'Comercial', 'Local', 3, '45269787-4593635696', 20000000.00, 'Disponible', 2, '/Uploads/Portadas/b99d6883-13d8-457f-b78b-e84c9b3a6058.jpg', ''),
(4, 'la punta 456', 'Comercial', 'Local', 3, '45269787-4593635696', 6.00, 'Disponible', 2, NULL, ''),
(5, 'suyuque 45', 'Residencial', 'departamento', 7, '45269787-4593635696', 8.00, 'Disponible', 3, '/Uploads/Portadas/71e5e66c-f846-4141-bf4f-2f297d46f0aa.png', ''),
(6, 'chacabuco 1416', 'Residencial', 'casa', 5, '44569865', 89888989.00, 'No disponible', 3, '/Uploads/Portadas/ca113d6d-0fcc-4203-890d-c72cdfdafab6.jpg', '');

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
(1, 'Guadalupe', 'Figar', '48596232', '7775654', 'guada@figar.com.ar'),
(2, 'Gonzalo', 'Noe', '21412312', '02664629466', 'gonza2002noe@gmail.com'),
(3, 'roberto', 'juarez', '45689752', '2615449235', 'juarez@roberto.com');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pago`
--

CREATE TABLE `pago` (
  `id` int(11) NOT NULL,
  `id_contrato` int(11) NOT NULL,
  `fecha_pago` date NOT NULL,
  `monto` decimal(10,2) NOT NULL CHECK (`monto` > 0),
  `Estado` tinyint(1) NOT NULL,
  `ID_UsuarioAlta` int(2) NOT NULL,
  `ID_UsuarioBaja` int(2) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `pago`
--

INSERT INTO `pago` (`id`, `id_contrato`, `fecha_pago`, `monto`, `Estado`, `ID_UsuarioAlta`, `ID_UsuarioBaja`) VALUES
(58, 4, '2025-04-15', 50000.00, 0, 0, 0),
(59, 4, '2025-02-09', 50000.00, 0, 0, 0),
(60, 4, '2030-01-01', 123.00, 0, 3, 0),
(61, 4, '2025-01-01', 150.00, 0, 3, 3),
(62, 12, '2030-01-01', 895.00, 0, 1, 1),
(63, 4, '0001-01-01', 1203.00, 1, 1, 0),
(64, 12, '2025-04-29', 1342.50, 1, 0, 0);

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
(2, 'Martin', 'Norton', '45669332', '2665119568', 'martinnorto@gmail.com'),
(3, 'julian', 'diaz', '25633988', '2659886475', 'julian@diaz.com');

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
(1, 'Admin', 'Admin', 'admin@admin.com', 'mBZPIhgElj0rSVwipR/X0M7+4wW2vGjVhlvKm+EP00g=', '/Uploads/Avatars\\avatar_1.jpg', 1),
(2, 'Inmobiliaria', 'Las Rosas', 'inmob@lasrosas.com', 'm/v4mphGaXqhYgv0fdh4YYUQq1fPWvx1GX3ArU78pSI=', '/Uploads/Avatars\\avatar_2.jpg', 2),
(3, 'Juan', 'perez', 'juanperez@gmail.com', 'M6yoBgMOpIokxwMC1N9GTPmL1IYlLo+lM/6PkBM0nwc=', '/Uploads/Avatars\\avatar_3.jpg', 2);

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
  ADD KEY `id_propietario` (`id_propietario`);

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
  MODIFY `ID_Contrato` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=15;

--
-- AUTO_INCREMENT de la tabla `imagenes`
--
ALTER TABLE `imagenes`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `inmueble`
--
ALTER TABLE `inmueble`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT de la tabla `inquilino`
--
ALTER TABLE `inquilino`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT de la tabla `pago`
--
ALTER TABLE `pago`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=65;

--
-- AUTO_INCREMENT de la tabla `propietario`
--
ALTER TABLE `propietario`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

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
