CREATE PROCEDURE AddPrediction(IN symbol VARCHAR(6), IN prediction_date DATETIME, IN price DECIMAL(60, 30))
BEGIN
	START TRANSACTION;
		INSERT INTO Predictions(symbol, prediction_date, price)
		VALUES (symbol, prediction_date, price);
	COMMIT;
END