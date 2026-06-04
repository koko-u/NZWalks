UPDATE "walks" AS "W"
SET "name"          = COALESCE(@Name, "W"."name"),
    "description"   = COALESCE(@Description, "W"."description"),
    "length_km"     = COALESCE(@LengthKm, "W"."length_km"),
    "image_url"     = COALESCE(@ImageUrl, "W"."image_url"),
    "region_id"     = COALESCE(
            (SELECT "id" FROM "regions" WHERE "code" = @RegionCode),
            "W"."region_id"
                      ),
    "difficulty_id" = COALESCE(
            (SELECT "id" FROM "difficulties" WHERE "name" = @DifficultyName),
            "W"."difficulty_id"
                      )
WHERE "W"."id" = @Id
RETURNING "W"."id",
    "W"."name",
    "W"."description",
    "W"."length_km",
    "W"."image_url",
    "W"."region_id",
        (SELECT "code" FROM "regions" WHERE "id" = "W"."region_id") AS "region_code",
        (SELECT "name" FROM "regions" WHERE "id" = "W"."region_id") AS "region_name",
        (SELECT "image_url" FROM "regions" WHERE "id" = "W"."region_id") AS "region_image_url",
    "W"."difficulty_id",
        (SELECT "name" FROM "difficulties" WHERE "id" = "W"."difficulty_id")