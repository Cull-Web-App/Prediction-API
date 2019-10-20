CREATE OR REPLACE FUNCTION AddPrediction(symbol VARCHAR(6), prediction_date TIMESTAMP, price DECIMAL(60, 30))
	RETURNS VOID
AS $$
	START TRANSACTION;
        INSERT INTO predictions(symbol, prediction_date, price, accuracy_score)
        VALUES (symbol, prediction_date, price, NULL);
	COMMIT TRANSACTION;
$$ LANGUAGE SQL;
