CREATE TABLE IF NOT EXISTS predictions (
  	symbol VARCHAR(6),
  	prediction_date TIMESTAMP,
  	price DECIMAL(28, 14),
  	accuracy_score DECIMAL(28, 14),
	predicted_on_time TIMESTAMP
);