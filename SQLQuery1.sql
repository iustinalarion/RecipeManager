﻿SELECT UserID, COUNT(*) 
FROM Member
GROUP BY UserID
HAVING COUNT(*) > 1;
