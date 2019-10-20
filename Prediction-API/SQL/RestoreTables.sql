CREATE TABLE IF NOT EXISTS predictions (
  	symbol VARCHAR(6),
  	prediction_date TIMESTAMP,
  	price DECIMAL(60, 30),
  	accuracy_score DECIMAL(60, 30)
);