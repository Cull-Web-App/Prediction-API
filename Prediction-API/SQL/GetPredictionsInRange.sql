CREATE PROCEDURE GetPredictionsInRange(IN symbol VARCHAR(6), IN start_date DATETIME, IN end_date DATETIME)
BEGIN
	SELECT *
	FROM Predictions as p
	WHERE p.symbol = symbol AND p.prediction_date >= start_date AND p.prediction_date <= end_date;
END