CREATE OR REPLACE FUNCTION GetPredictionsInRange(symbol VARCHAR(6), start_date TIMESTAMP, end_date TIMESTAMP)
    RETURNS SETOF predictions
AS $$
	SELECT *
	FROM
	   predictions as p
	WHERE
        p.symbol = symbol AND
        p.prediction_date >= start_date AND
        p.prediction_date <= end_date;
$$ LANGUAGE SQL;
