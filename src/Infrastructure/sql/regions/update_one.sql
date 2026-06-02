WITH "target_row" AS (SELECT "id",
                             "code",
                             "name",
                             "image_url"
                      FROM "regions"
                      WHERE "id" = @RegionId)
UPDATE "regions" AS "R"
SET "code"      = CASE
                      WHEN @RegionCode IS NULL THEN "T"."code"
                      ELSE @RegionCode
                  END,
    "name"      = CASE
                      WHEN @RegionName IS NULL THEN "T"."name"
                      ELSE @RegionName
                  END,
    "image_url" = @RegionImageUrl
FROM "target_row" AS "T"
WHERE "R"."id" = "T"."id"
RETURNING "R"."id",
    "R"."code",
    "R"."name",
    "R"."image_url";