-- phpMyAdmin SQL Dump
-- version 5.0.4
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Nov 30, 2020 at 07:11 PM
-- Server version: 10.4.16-MariaDB
-- PHP Version: 7.4.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `khana`
--

-- --------------------------------------------------------

--
-- Table structure for table `admin_roles`
--

CREATE TABLE `admin_roles` (
  `id` bigint(20) UNSIGNED NOT NULL,
  `name` varchar(191) COLLATE utf8mb4_unicode_ci NOT NULL,
  `guard_name` varchar(191) COLLATE utf8mb4_unicode_ci NOT NULL,
  `created_at` timestamp NULL DEFAULT NULL,
  `updated_at` timestamp NULL DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Dumping data for table `admin_roles`
--

INSERT INTO `admin_roles` (`id`, `name`, `guard_name`, `created_at`, `updated_at`) VALUES
(1, 'superadmin', 'web', '2020-11-30 11:59:46', '2020-11-30 11:59:46');

-- --------------------------------------------------------

--
-- Table structure for table `model_has_permissions`
--

CREATE TABLE `model_has_permissions` (
  `permission_id` bigint(20) UNSIGNED NOT NULL,
  `model_type` varchar(191) COLLATE utf8mb4_unicode_ci NOT NULL,
  `model_id` bigint(20) UNSIGNED NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Table structure for table `model_has_roles`
--

CREATE TABLE `model_has_roles` (
  `role_id` bigint(20) UNSIGNED NOT NULL,
  `model_type` varchar(191) COLLATE utf8mb4_unicode_ci NOT NULL,
  `model_id` bigint(20) UNSIGNED NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Dumping data for table `model_has_roles`
--

INSERT INTO `model_has_roles` (`role_id`, `model_type`, `model_id`) VALUES
(1, 'App\\User', 1);

-- --------------------------------------------------------

--
-- Table structure for table `permissions`
--

CREATE TABLE `permissions` (
  `id` bigint(20) UNSIGNED NOT NULL,
  `name` varchar(191) COLLATE utf8mb4_unicode_ci NOT NULL,
  `guard_name` varchar(191) COLLATE utf8mb4_unicode_ci NOT NULL,
  `group_name` varchar(191) COLLATE utf8mb4_unicode_ci NOT NULL,
  `created_at` timestamp NULL DEFAULT NULL,
  `updated_at` timestamp NULL DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Dumping data for table `permissions`
--

INSERT INTO `permissions` (`id`, `name`, `guard_name`, `group_name`, `created_at`, `updated_at`) VALUES
(1, 'dashboard', 'web', 'dashboard', '2020-11-30 11:59:46', '2020-11-30 11:59:46'),
(2, 'update', 'web', 'update', '2020-11-30 11:59:47', '2020-11-30 11:59:47'),
(3, 'admin.create', 'web', 'admin', '2020-11-30 11:59:48', '2020-11-30 11:59:48'),
(4, 'admin.edit', 'web', 'admin', '2020-11-30 11:59:48', '2020-11-30 11:59:48'),
(5, 'admin.update', 'web', 'admin', '2020-11-30 11:59:48', '2020-11-30 11:59:48'),
(6, 'admin.delete', 'web', 'admin', '2020-11-30 11:59:49', '2020-11-30 11:59:49'),
(7, 'admin.list', 'web', 'admin', '2020-11-30 11:59:49', '2020-11-30 11:59:49'),
(8, 'role.create', 'web', 'role', '2020-11-30 11:59:49', '2020-11-30 11:59:49'),
(9, 'role.edit', 'web', 'role', '2020-11-30 11:59:50', '2020-11-30 11:59:50'),
(10, 'role.update', 'web', 'role', '2020-11-30 11:59:50', '2020-11-30 11:59:50'),
(11, 'role.delete', 'web', 'role', '2020-11-30 11:59:51', '2020-11-30 11:59:51'),
(12, 'role.list', 'web', 'role', '2020-11-30 11:59:51', '2020-11-30 11:59:51'),
(13, 'media.list', 'web', 'Media', '2020-11-30 11:59:52', '2020-11-30 11:59:52'),
(14, 'media.upload', 'web', 'Media', '2020-11-30 11:59:52', '2020-11-30 11:59:52'),
(15, 'media.delete', 'web', 'Media', '2020-11-30 11:59:53', '2020-11-30 11:59:53'),
(16, 'page.list', 'web', 'Page', '2020-11-30 11:59:53', '2020-11-30 11:59:53'),
(17, 'page.create', 'web', 'Page', '2020-11-30 11:59:54', '2020-11-30 11:59:54'),
(18, 'page.edit', 'web', 'Page', '2020-11-30 11:59:54', '2020-11-30 11:59:54'),
(19, 'page.delete', 'web', 'Page', '2020-11-30 11:59:55', '2020-11-30 11:59:55'),
(20, 'product.list', 'web', 'Products', '2020-11-30 11:59:56', '2020-11-30 11:59:56'),
(21, 'product.delete', 'web', 'Products', '2020-11-30 11:59:56', '2020-11-30 11:59:56'),
(22, 'product.category', 'web', 'Products', '2020-11-30 11:59:57', '2020-11-30 11:59:57'),
(23, 'resturents.requests', 'web', 'Restaurant', '2020-11-30 11:59:57', '2020-11-30 11:59:57'),
(24, 'resturents.view', 'web', 'Restaurant', '2020-11-30 11:59:58', '2020-11-30 11:59:58'),
(25, 'all.resturents', 'web', 'Restaurant', '2020-11-30 11:59:58', '2020-11-30 11:59:58'),
(26, 'manage.review', 'web', 'Restaurant', '2020-11-30 11:59:58', '2020-11-30 11:59:58'),
(27, 'rider.request', 'web', 'Rider', '2020-11-30 11:59:59', '2020-11-30 11:59:59'),
(28, 'all.rider', 'web', 'Rider', '2020-11-30 11:59:59', '2020-11-30 11:59:59'),
(29, 'customer.list', 'web', 'Customer', '2020-11-30 12:00:00', '2020-11-30 12:00:00'),
(30, 'customer.edit', 'web', 'Customer', '2020-11-30 12:00:00', '2020-11-30 12:00:00'),
(31, 'payout.request', 'web', 'Payout', '2020-11-30 12:00:00', '2020-11-30 12:00:00'),
(32, 'payout.history', 'web', 'Payout', '2020-11-30 12:00:00', '2020-11-30 12:00:00'),
(33, 'payout.account', 'web', 'Payout', '2020-11-30 12:00:01', '2020-11-30 12:00:01'),
(34, 'payout.view', 'web', 'Payout', '2020-11-30 12:00:01', '2020-11-30 12:00:01'),
(35, 'order.list', 'web', 'Orders', '2020-11-30 12:00:01', '2020-11-30 12:00:01'),
(36, 'order.control', 'web', 'Orders', '2020-11-30 12:00:02', '2020-11-30 12:00:02'),
(37, 'plan.create', 'web', 'Plan', '2020-11-30 12:00:02', '2020-11-30 12:00:02'),
(38, 'plan.list', 'web', 'Plan', '2020-11-30 12:00:02', '2020-11-30 12:00:02'),
(39, 'plan.view', 'web', 'Plan', '2020-11-30 12:00:03', '2020-11-30 12:00:03'),
(40, 'plan.edit', 'web', 'Plan', '2020-11-30 12:00:03', '2020-11-30 12:00:03'),
(41, 'plan.delete', 'web', 'Plan', '2020-11-30 12:00:03', '2020-11-30 12:00:03'),
(42, 'payment.request', 'web', 'Plan', '2020-11-30 12:00:04', '2020-11-30 12:00:04'),
(43, 'payment.make', 'web', 'Plan', '2020-11-30 12:00:04', '2020-11-30 12:00:04'),
(44, 'location.create', 'web', 'Location', '2020-11-30 12:00:04', '2020-11-30 12:00:04'),
(45, 'location.list', 'web', 'Location', '2020-11-30 12:00:05', '2020-11-30 12:00:05'),
(46, 'location.edit', 'web', 'Location', '2020-11-30 12:00:05', '2020-11-30 12:00:05'),
(47, 'location.delete', 'web', 'Location', '2020-11-30 12:00:05', '2020-11-30 12:00:05'),
(48, 'badge.control', 'web', 'Badge', '2020-11-30 12:00:05', '2020-11-30 12:00:05'),
(49, 'featured.control', 'web', 'Featured', '2020-11-30 12:00:06', '2020-11-30 12:00:06'),
(50, 'earning.order.report', 'web', 'Reports', '2020-11-30 12:00:06', '2020-11-30 12:00:06'),
(51, 'earning.delivery.report', 'web', 'Reports', '2020-11-30 12:00:06', '2020-11-30 12:00:06'),
(52, 'earning.subscription.report', 'web', 'Reports', '2020-11-30 12:00:06', '2020-11-30 12:00:06'),
(53, 'theme', 'web', 'Appearance', '2020-11-30 12:00:07', '2020-11-30 12:00:07'),
(54, 'menu', 'web', 'Appearance', '2020-11-30 12:00:07', '2020-11-30 12:00:07'),
(55, 'plugin.control', 'web', 'Plugin', '2020-11-30 12:00:07', '2020-11-30 12:00:07'),
(56, 'site.settings', 'web', 'Settings', '2020-11-30 12:00:07', '2020-11-30 12:00:07'),
(57, 'seo', 'web', 'Settings', '2020-11-30 12:00:08', '2020-11-30 12:00:08'),
(58, 'file.system', 'web', 'Settings', '2020-11-30 12:00:08', '2020-11-30 12:00:08'),
(59, 'system.settings', 'web', 'Settings', '2020-11-30 12:00:08', '2020-11-30 12:00:08'),
(60, 'payment.settings', 'web', 'Settings', '2020-11-30 12:00:08', '2020-11-30 12:00:08'),
(61, 'language.control', 'web', 'language', '2020-11-30 12:00:09', '2020-11-30 12:00:09');

-- --------------------------------------------------------

--
-- Table structure for table `role_has_permissions`
--

CREATE TABLE `role_has_permissions` (
  `permission_id` bigint(20) UNSIGNED NOT NULL,
  `role_id` bigint(20) UNSIGNED NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Dumping data for table `role_has_permissions`
--

INSERT INTO `role_has_permissions` (`permission_id`, `role_id`) VALUES
(1, 1),
(2, 1),
(3, 1),
(4, 1),
(5, 1),
(6, 1),
(7, 1),
(8, 1),
(9, 1),
(10, 1),
(11, 1),
(12, 1),
(13, 1),
(14, 1),
(15, 1),
(16, 1),
(17, 1),
(18, 1),
(19, 1),
(20, 1),
(21, 1),
(22, 1),
(23, 1),
(24, 1),
(25, 1),
(26, 1),
(27, 1),
(28, 1),
(29, 1),
(30, 1),
(31, 1),
(32, 1),
(33, 1),
(34, 1),
(35, 1),
(36, 1),
(37, 1),
(38, 1),
(39, 1),
(40, 1),
(41, 1),
(42, 1),
(43, 1),
(44, 1),
(45, 1),
(46, 1),
(47, 1),
(48, 1),
(49, 1),
(50, 1),
(51, 1),
(52, 1),
(53, 1),
(54, 1),
(55, 1),
(56, 1),
(57, 1),
(58, 1),
(59, 1),
(60, 1),
(61, 1);

--
-- Indexes for dumped tables
--

--
-- Indexes for table `admin_roles`
--
ALTER TABLE `admin_roles`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `model_has_permissions`
--
ALTER TABLE `model_has_permissions`
  ADD PRIMARY KEY (`permission_id`,`model_id`,`model_type`),
  ADD KEY `model_has_permissions_model_id_model_type_index` (`model_id`,`model_type`);

--
-- Indexes for table `model_has_roles`
--
ALTER TABLE `model_has_roles`
  ADD PRIMARY KEY (`role_id`,`model_id`,`model_type`),
  ADD KEY `model_has_roles_model_id_model_type_index` (`model_id`,`model_type`);

--
-- Indexes for table `permissions`
--
ALTER TABLE `permissions`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `role_has_permissions`
--
ALTER TABLE `role_has_permissions`
  ADD PRIMARY KEY (`permission_id`,`role_id`),
  ADD KEY `role_has_permissions_role_id_foreign` (`role_id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `admin_roles`
--
ALTER TABLE `admin_roles`
  MODIFY `id` bigint(20) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT for table `permissions`
--
ALTER TABLE `permissions`
  MODIFY `id` bigint(20) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=62;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `model_has_permissions`
--
ALTER TABLE `model_has_permissions`
  ADD CONSTRAINT `model_has_permissions_permission_id_foreign` FOREIGN KEY (`permission_id`) REFERENCES `permissions` (`id`) ON DELETE CASCADE;

--
-- Constraints for table `model_has_roles`
--
ALTER TABLE `model_has_roles`
  ADD CONSTRAINT `model_has_roles_role_id_foreign` FOREIGN KEY (`role_id`) REFERENCES `admin_roles` (`id`) ON DELETE CASCADE;

--
-- Constraints for table `role_has_permissions`
--
ALTER TABLE `role_has_permissions`
  ADD CONSTRAINT `role_has_permissions_permission_id_foreign` FOREIGN KEY (`permission_id`) REFERENCES `permissions` (`id`) ON DELETE CASCADE,
  ADD CONSTRAINT `role_has_permissions_role_id_foreign` FOREIGN KEY (`role_id`) REFERENCES `admin_roles` (`id`) ON DELETE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
