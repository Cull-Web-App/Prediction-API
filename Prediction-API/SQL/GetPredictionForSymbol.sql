CREATE OR REPLACE FUNCTION GetPredictionForSymbol(symbol VARCHAR(6), prediction_date TIMESTAMP)
    RETURNS SETOF DECIMAL(28, 14)
AS $$
    SELECT
       p.price
    FROM
       predictions AS p
    WHERE
       p.symbol = GetPredictionForSymbol.symbol AND
       EXTRACT(DAY FROM p.prediction_date - GetPredictionForSymbol.prediction_date) = 0;
$$ LANGUAGE SQL;
