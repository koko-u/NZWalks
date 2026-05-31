SELECT "W"."id",
       "W"."name",
       "W"."description",
       "W"."length_km",
       "W"."image_url",
       "W"."region_id",
       "R"."name" AS "region_name",
       "W"."difficulty_id",
       "D"."name" AS "difficulty_name"
FROM "walks" AS "W"
         INNER JOIN
     "regions" AS "R"
     ON "W"."region_id" = "R"."id"
         INNER JOIN
     "difficulties" AS "D"
     ON "W"."difficulty_id" = "D"."id"
ORDER BY "W"."id";