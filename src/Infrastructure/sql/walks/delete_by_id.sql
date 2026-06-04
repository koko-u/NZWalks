WITH "deleted_row" AS (
    DELETE
        FROM "walks"
            WHERE "id" = @Id
            RETURNING "id",
                "name",
                "description",
                "length_km",
                "image_url",
                "region_id",
                "difficulty_id")
SELECT "W"."id",
       "W"."name",
       "W"."description",
       "W"."length_km",
       "W"."image_url",
       "W"."region_id",
       "R"."name" AS "region_name",
       "W"."difficulty_id",
       "D"."name" AS "difficulty_name"
FROM "deleted_row" AS "W"
         INNER JOIN
     "regions" AS "R"
     ON "R"."id" = "W"."region_id"
         INNER JOIN
     "difficulties" AS "D"
     ON "D"."id" = "W"."difficulty_id"
