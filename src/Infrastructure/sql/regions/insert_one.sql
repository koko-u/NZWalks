INSERT INTO "regions" ("code",
                       "name",
                       "image_url")
VALUES (@RegionCode,
        @RegionName,
        @RegionImageUrl)
RETURNING "id",
    "code",
    "name",
    "image_url";