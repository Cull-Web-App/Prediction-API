CREATE DATABASE IF NOT EXISTS `Prediction-Store`;
USE `Prediction-Store`;

CREATE TABLE IF NOT EXISTS `Predictions` (
  	`symbol` VARCHAR(6),
  	`prediction_date` DATETIME,
  	`price` DECIMAL(60, 30)
);