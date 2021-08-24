insert 
	into statistics
	(
		statistics_id,
		date,
		json_data
	)
	values
	(
		@StatisticsId,
		@Date,
		cast(@JsonData as jsonb)
	)