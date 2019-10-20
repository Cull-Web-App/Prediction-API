CREATE PROCEDURE GetPredictionForSymbol(IN symbol VARCHAR(6), IN prediction_date DATETIME)
BEGIN
	SELECT p.price
	FROM Predictions as p
	WHERE p.symbol = symbol AND DATEDIFF(p.prediction_date, prediction_date) = 0;
END