CREATE DATABASE  IF NOT EXISTS `tmsdb` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `tmsdb`;
-- MySQL dump 10.13  Distrib 8.0.31, for Win64 (x86_64)
--
-- Host: localhost    Database: tmsdb
-- ------------------------------------------------------
-- Server version	8.0.31

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `carriers`
--

DROP TABLE IF EXISTS `carriers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `carriers` (
  `carrier_ID` int unsigned NOT NULL AUTO_INCREMENT,
  `carrier_name` varchar(50) NOT NULL,
  `depot_city` varchar(50) NOT NULL,
  `ftl_availability` float NOT NULL,
  `ltl_availability` float NOT NULL,
  `ftl_rate` float NOT NULL,
  `ltl_rate` float NOT NULL,
  `reefer_charge` float NOT NULL,
  PRIMARY KEY (`carrier_ID`),
  KEY `order_id` (`depot_city`)
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `carriers`
--

LOCK TABLES `carriers` WRITE;
/*!40000 ALTER TABLE `carriers` DISABLE KEYS */;
INSERT INTO `carriers` VALUES (1,'Planet Express','Windsor',50,640,5.21,0.3621,0.08),(2,'Planet Express','Hamilton',50,640,5.21,0.3621,0.08),(3,'Planet Express','Oshawa',50,640,5.21,0.3621,0.08),(4,'Planet Express','Belleville',50,640,5.21,0.3621,0.08),(5,'Planet Express','Ottawa',50,640,5.21,0.3621,0.08),(6,'Schooner\'s','London',18,98,5.05,0.3434,0.07),(7,'Schooner\'s','Toronto',18,98,5.05,0.3434,0.07),(8,'Schooner\'s','Kingston',18,98,5.05,0.3434,0.07),(9,'Tillman Transport','Windsor',24,35,5.11,0.3012,0.09),(10,'Tillman Transport','London',18,45,5.11,0.3012,0.09),(11,'Tillman Transport','Hamilton',18,45,5.11,0.3012,0.09),(12,'We Haul','Ottawa',11,0,5.2,0,0.065),(13,'We Haul','Toronto',11,0,5.2,0,0.065);
/*!40000 ALTER TABLE `carriers` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `contract`
--

DROP TABLE IF EXISTS `contract`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `contract` (
  `contract_ID` int unsigned NOT NULL AUTO_INCREMENT,
  `customer_ID` int unsigned NOT NULL,
  `job_type` int NOT NULL,
  `quantity` int NOT NULL,
  `van_type` int NOT NULL,
  `origin_city` varchar(25) NOT NULL,
  `destination_city` varchar(25) NOT NULL,
  PRIMARY KEY (`contract_ID`),
  KEY `customer_ID` (`customer_ID`),
  CONSTRAINT `contract_ibfk_1` FOREIGN KEY (`customer_ID`) REFERENCES `customer` (`customer_ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `contract`
--

LOCK TABLES `contract` WRITE;
/*!40000 ALTER TABLE `contract` DISABLE KEYS */;
/*!40000 ALTER TABLE `contract` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `customer`
--

DROP TABLE IF EXISTS `customer`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `customer` (
  `customer_ID` int unsigned NOT NULL AUTO_INCREMENT,
  `customer_name` varchar(50) NOT NULL,
  PRIMARY KEY (`customer_ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `customer`
--

LOCK TABLES `customer` WRITE;
/*!40000 ALTER TABLE `customer` DISABLE KEYS */;
/*!40000 ALTER TABLE `customer` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `invoices`
--

DROP TABLE IF EXISTS `invoices`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `invoices` (
  `invoice_ID` int unsigned NOT NULL AUTO_INCREMENT,
  `customer_ID` int unsigned NOT NULL,
  `date` date NOT NULL,
  `carrier_ID` int unsigned NOT NULL,
  `job_type` int NOT NULL,
  `van_type` int NOT NULL,
  `quantity` int NOT NULL,
  `distance` int NOT NULL,
  `total` decimal(10,2) NOT NULL,
  PRIMARY KEY (`invoice_ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `invoices`
--

LOCK TABLES `invoices` WRITE;
/*!40000 ALTER TABLE `invoices` DISABLE KEYS */;
/*!40000 ALTER TABLE `invoices` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `orders`
--

DROP TABLE IF EXISTS `orders`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `orders` (
  `order_ID` int unsigned NOT NULL AUTO_INCREMENT,
  `contract_ID` int unsigned NOT NULL,
  `carrier_ID` int unsigned DEFAULT NULL,
  `trip_ID` int unsigned DEFAULT NULL,
  `cargo_type` int NOT NULL,
  `origin_city` varchar(15) NOT NULL,
  `trip_duration` double DEFAULT NULL,
  `completed` varchar(5) DEFAULT NULL,
  PRIMARY KEY (`order_ID`),
  KEY `contract_ID` (`contract_ID`),
  KEY `carrier_ID_idx` (`carrier_ID`),
  KEY `trip_ID` (`trip_ID`),
  CONSTRAINT `carrier_ID` FOREIGN KEY (`carrier_ID`) REFERENCES `carriers` (`carrier_ID`),
  CONSTRAINT `orders_ibfk_1` FOREIGN KEY (`contract_ID`) REFERENCES `contract` (`contract_ID`),
  CONSTRAINT `trip_ID` FOREIGN KEY (`trip_ID`) REFERENCES `trips` (`trip_ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `orders`
--

LOCK TABLES `orders` WRITE;
/*!40000 ALTER TABLE `orders` DISABLE KEYS */;
/*!40000 ALTER TABLE `orders` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `trips`
--

DROP TABLE IF EXISTS `trips`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `trips` (
  `trip_ID` int unsigned NOT NULL AUTO_INCREMENT,
  `destination_city` varchar(25) NOT NULL,
  `trip_length` int NOT NULL,
  `trip_duration` double NOT NULL,
  PRIMARY KEY (`trip_ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `trips`
--

LOCK TABLES `trips` WRITE;
/*!40000 ALTER TABLE `trips` DISABLE KEYS */;
/*!40000 ALTER TABLE `trips` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `users` (
  `user_ID` int unsigned NOT NULL AUTO_INCREMENT,
  `order_ID` int unsigned NOT NULL,
  `role_ID` int unsigned NOT NULL,
  `first_name` varchar(20) NOT NULL,
  `last_name` varchar(20) NOT NULL,
  PRIMARY KEY (`user_ID`),
  KEY `order_ID` (`order_ID`),
  CONSTRAINT `users_ibfk_1` FOREIGN KEY (`order_ID`) REFERENCES `orders` (`order_ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
/*!40000 ALTER TABLE `users` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2022-12-05 14:41:27
