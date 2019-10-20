CREATE OR REPLACE FUNCTION GetPredictionForSymbol(symbol VARCHAR(6), prediction_date TIMESTAMP)
    RETURNS SETOF DECIMAL(60, 30)
AS $$
	SELECT
	   p.price
	FROM
	   predictions as p
	WHERE
	   p.symbol = symbol AND
       EXTRACT(DAY FROM p.prediction_date - prediction_date) = 0;
$$ LANGUAGE SQL;
