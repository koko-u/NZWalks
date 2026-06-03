WITH "region_row" AS (SELECT "id",
                             "code",
                             "name",
                             "image_url"
                      FROM "regions"
                      WHERE "code" = @RegionCode),
     "difficulty_row" AS (SELECT "id",
                                 "name"
                          FROM "difficulties"
                          WHERE "name" = @DifficultyName)
INSERT
INTO "walks" ("name",
              "description",
              "length_km",
              "image_url",
              "region_id",
              "difficulty_id")
VALUES (@Name,
        @Description,
        @LengthKm,
        @ImageUrl,
        (SELECT "id" FROM "region_row"),
        (SELECT "id" FROM "difficulty_row"))
RETURNING "id",
    "name",
    "description",
    "length_km",
    "image_url",
    "region_id",
        (SELECT "code" FROM "region_row") AS "region_code",
        (SELECT "name" FROM "region_row") AS "region_name",
        (SELECT "image_url" FROM "region_row") AS "region_image_url",
    "difficulty_id",
        (SELECT "name" FROM "difficulty_row") AS "difficulty_name";
