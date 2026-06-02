DELETE
FROM "regions"
WHERE "id" = @RegionId
RETURNING "id",
    "code",
    "name",
    "image_url";