SELECT "id", 
       "code", 
       "name", 
       "image_url"
FROM "regions"
WHERE "code" = @RegionCode;