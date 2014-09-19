SELECT	unitinfo.lease_sq_foot,
	unitinfo.unit_id,
	unitinfo.floor,
	unitinfo.unit_type,
	unitinfo.dealid,
	unitinfo.charge_start_date,
	unitinfo.charge_code,
	unitinfo.actual_sq_foot,
	(CASE   WHEN    ( unitinfo.charge_code='BPR' )
		THEN  '2'
		ELSE '1'
		END ) AS chargecodecateg ,
	unitinfo.budgetorforecast,
	ROUND(annrentforecast,0) AS annrentforecast,
	ROUND(annrentbudget,0) AS annrentbudget,
	ROUND(ytdrentbudget,0) AS ytdrentbudget,
	ROUND(ytdrentforecast,0) AS ytdrentforecast,
	variance.ytdcode,
	variance.ytdcomment,
	variance.anncode,
	variance.anncomment,
	(ytdrentforecast-ytdrentbudget) AS ytdrentdiff,
	(annrentforecast-annrentbudget) AS annrentdiff

FROM
	(SELECT
		CASE WHEN unitlease.dealid IS NULL
			THEN budgetrents.lease_sq_foot
			ELSE unitlease.lease_sq_foot
			END lease_sq_foot,
		CASE WHEN unitlease.unit_id IS NULL
			THEN budgetrents.unit_type
			ELSE unitlease.unit_type
			END unit_type,
		CASE WHEN unitlease.unit_id IS NULL
			THEN 2
			ELSE 1
			END budgetorforecast,
		CASE WHEN unitlease.unit_id IS NULL
			THEN budgetrents.actual_sq_foot
			ELSE unitlease.actual_sq_foot
			END actual_sq_foot,
		CASE WHEN unitlease.unit_id IS NULL
			THEN budgetrents.unit_id
			ELSE unitlease.unit_id
			END unit_id,
		CASE WHEN unitlease.unit_id IS NULL
			THEN budgetrents.floor
			ELSE unitlease.floor
			END floor,
		unitlease.charge_start_date,
		CASE WHEN unitlease.unit_id IS NULL
			THEN budgetrents.dealid
			ELSE unitlease.dealid
			END dealid,
		CASE WHEN unitlease.charge_code IS NULL
			THEN budgetrents.charge_code
			ELSE unitlease.charge_code
			END charge_code,
		forecastrents.annrentforecast,
		forecastrents.ytdrentforecast,
		budgetrents.annrentbudget,
		budgetrents.ytdrentbudget
	FROM
		(SELECT	u.unit_id,
			u.floor,
			u.unit_type,
			u.actual_sq_foot ,
			l.lease_id,
			l.dealid,
			l.lease_sq_foot,
			r.space_seq_num,
			r.charge_start_date,
			r.charge_code
			
		 FROM	(Unit_Budget u
			 INNER JOIN Month_Unit_Status mus ON
				u.budget_guid='CURRENTBUDGET' AND
				u.project_id='0110' AND
				u.budget_guid = mus.budget_guid AND
				u.project_id=mus.project_id AND
				u.unit_id=mus.unit_id AND u.floor=mus.floor
			)
		LEFT OUTER JOIN Lease_Budget l ON
				u.budget_guid = l.budget_guid AND
				u.project_id=l.project_id AND
				u.unit_id=l.unit_id AND
				u.floor=l.floor

        	LEFT OUTER JOIN Rent_Budget r ON
				l.budget_guid=r.budget_guid AND
				l.lease_id=r.lease_id AND
				l.unit_id=r.unit_id  AND
				l.project_id=r.project_id AND
				l.floor=r.floor AND
				l.dealID=r.dealID AND
				l.space_seq_num=r.space_seq_num AND
				r.month + (INTEGER(r.year) * 100) <= 200912 AND
				r.month + (INTEGER(r.year) * 100)  >= 200901 AND
				r.charge_code NOT IN ('BOV', 'BOX', 'AOV', 'AOX', 'COV', 'CZX', 'OVR', 'OVX')
		WHERE ( (u.end_date >= '20090101' or u.end_date = '' OR u.end_date IS NULL) AND u.effective_date <= '20091231' )
                   OR ( r.month + (INTEGER(r.year) * 100) <= 20091231 AND r.month + (INTEGER(r.year) * 100)  >= 20090101 )
		GROUP BY u.unit_id, u.floor, u.unit_type, u.actual_sq_foot,
				l.lease_sq_foot, l.lease_id, l.dealid,
				r.charge_start_date, r.charge_code,  r.space_seq_num
		) AS unitlease

	LEFT OUTER JOIN  
			(SELECT u.unit_id, 
				u.floor,
				r.charge_code,   
				r.charge_start_date,   
				r.space_seq_num,    
				l.lease_id,   
				l.dealid,   
				SUM(r.rent_month) AS annrentforecast,   
				SUM(CASE WHEN ( (  r.month + (INTEGER(r.year) * 100) <=  8 + (2009 * 100))   AND
						 (r.month + (INTEGER(r.year) * 100)  >= 20090101 )) 
					 THEN r.rent_month   
					 ELSE 0   
					 END) AS ytdrentforecast   
			 FROM Unit_Budget u 
			 INNER JOIN   
				(SELECT   
					m.budget_guid,   
					m.project_id,   
					m.unit_id,   
					m.floor   

				FROM 	Month_Unit_Status m   

				WHERE 	m.budget_guid = 'CURRENTBUDGET' AND   
					m.project_id = '0110' 
				GROUP BY   m.budget_guid, m.project_id, m.unit_id, m.floor   
				) AS mus   
					ON  
					u.budget_guid = mus.budget_guid AND   
					u.project_id=mus.project_id AND   
					u.unit_id=mus.unit_id AND   
					u.floor=mus.floor   
			INNER JOIN Lease_Budget l ON   
					u.budget_guid=l.budget_guid AND   
					u.project_id=l.project_id AND   
					u.unit_id=l.unit_id AND    
					u.floor=l.floor    

			LEFT OUTER JOIN Rent_Budget r ON
					l.budget_guid=r.budget_guid AND  
					l.project_id=r.project_id AND   
					l.unit_id=r.unit_id AND   
					l.floor=r.floor AND   
					l.lease_id=r.lease_id AND   
					l.dealID=r.dealID AND   
					l.space_seq_num=r.space_seq_num   AND 
					r.month + (INTEGER(r.year) * 100) <= 20091231 AND 
					r.month + (INTEGER(r.year) * 100)  >= 20090101 AND 
					r.charge_code NOT IN ('BOV', 'BOX', 'AOV', 'AOX', 'COV', 'CZX', 'OVR', 'OVX')  
			GROUP BY u.unit_id, u.floor, r.charge_code, r.charge_start_date, l.dealid, l.lease_id, r.space_seq_num   
			) AS forecastrents    
			ON    
			forecastrents.unit_id=unitlease.unit_id AND   
			forecastrents.floor=unitlease.floor AND   
			forecastrents.charge_code=unitlease.charge_code AND   
			forecastrents.charge_start_date=unitlease.charge_start_date AND   
			forecastrents.dealid=unitlease.dealid AND   
			forecastrents.lease_id=unitlease.lease_id AND   
			forecastrents.space_seq_num=unitlease.space_seq_num    

	FULL OUTER JOIN   
			( SELECT u.unit_id,   
				u.floor,   
				u.unit_Type,   
				u.actual_sq_foot,   
				l.lease_sq_foot,    
				r.charge_code,   
				r.charge_start_date,   
				l.lease_id,   
				l.dealid,   
				l.space_seq_num,   
				SUM(r.rent_month) AS annrentbudget,    
				SUM( CASE WHEN (  ( r.month + (INTEGER(r.year) * 100) <= 8 + ( 2009 * 100)) AND  
						  ( r.month + (INTEGER(r.year) * 100) >= 20090101 )) 
					THEN r.rent_month   
					ELSE 0   
					END) AS ytdrentbudget   
			FROM Unit_Budget u 
			INNER JOIN Month_Unit_Status mus ON   
				u.budget_guid='COMPARISONBUDGET' AND 
				u.project_id='0110' AND 
				u.budget_guid = mus.budget_guid AND   
				u.project_id=mus.project_id AND   
				u.unit_id=mus.unit_id AND   
				u.floor=mus.floor  
			INNER JOIN Lease_Budget l ON   
				u.budget_guid=l.budget_guid AND   
				u.project_id=l.project_id AND   
				u.unit_id=l.unit_id AND    
				u.floor=l.floor   
			LEFT OUTER JOIN Rent_Budget r ON   
				l.budget_guid=r.budget_guid AND   
				l.project_id=r.project_id AND   
				l.unit_id=r.unit_id  AND   
				l.floor=r.floor AND   
				l.lease_id=r.lease_id AND   
				l.dealID=r.dealID AND   
				l.space_seq_num=r.space_seq_num AND  
				r.year = mus.year AND  
				r.month = mus.month  AND  
				r.month + (INTEGER(r.year) * 100) <= 20091231 AND 
				r.month + (INTEGER(r.year) * 100)  >= 20090101 AND 
				r.charge_code NOT IN ('BOV', 'BOX', 'AOV', 'AOX', 'COV', 'CZX', 'OVR', 'OVX')   
			WHERE 
				r.charge_code IS NOT NULL  
			GROUP BY     
				u.unit_Type,   
				u.actual_sq_foot,   
				l.lease_sq_foot,    
				u.unit_id,   
				u.floor,   
				l.lease_id,   
				l.dealid,   
				l.space_seq_num,   
				r.charge_code,   
				r.charge_start_date   
			) AS budgetrents  
			ON    
			budgetrents.unit_id=unitlease.unit_id AND   
			budgetrents.floor=unitlease.floor AND    
			budgetrents.lease_id = unitlease.lease_id AND    
			budgetrents.dealid = unitlease.dealid AND    
			budgetrents.space_seq_num = unitlease.space_seq_num AND    
			budgetrents.charge_code = unitlease.charge_code AND    
			budgetrents.charge_Start_date = unitlease.charge_start_date    
		) AS unitinfo     
	LEFT OUTER JOIN   
			(SELECT unit_id,   
				CASE WHEN MAX(ytdcode) = chr(0)   
					THEN ''   
					ELSE MAX(ytdcode)   
					END AS ytdcode,   
				CASE WHEN MAX(ytdcomment) = chr(0)   
					THEN ''   
					ELSE MAX(ytdcomment)   
					END AS ytdcomment,    
				CASE WHEN MAX(anncode) = chr(0)   
					THEN ''   
					ELSE MAX(anncode)   
					END AS anncode,   
				CASE WHEN MAX(anncomment) = chr(0)   
					THEN ''   
					ELSE MAX(anncomment)   
					END AS anncomment   
			FROM     
				(SELECT unit_id,      
					CASE WHEN variance_period <> 'YTD'   
							OR variance_code IS NULL   
						THEN chr(0)   
						ELSE  variance_code   
						END ytdcode,     
					CASE WHEN variance_period <> 'YTD'   
							OR variance_comment IS NULL   
						THEN chr(0)   
						ELSE  variance_comment   
						END ytdcomment,     
					CASE WHEN variance_period <> 'ANN'   
							OR variance_code IS NULL   
						THEN chr(0)   
						ELSE  variance_code   
						END anncode,    
					CASE WHEN variance_period <> 'ANN'   
							OR variance_comment IS NULL   
						THEN chr(0)   
						ELSE  variance_comment   
						END anncomment     
				FROM   	Variance  
				WHERE 	budget_guid= 'CURRENTBUDGET' AND   
					project_id='0110' AND  
					variance_type = 'BMR' AND  
					comp_guid='COMPARISONGUID' AND 
					start_date='20090101' AND 
					end_date='20091231'
				) AS vars

			GROUP BY unit_id) AS variance
			ON
			variance.unit_id=unitinfo.unit_id

ORDER BY unit_id, budgetorforecast, chargecodecateg, charge_code
