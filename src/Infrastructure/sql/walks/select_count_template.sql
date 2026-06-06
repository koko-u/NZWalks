SELECT COUNT(*)
FROM "walks" AS "W"
         INNER JOIN
     "regions" AS "R"
     ON "W"."region_id" = "R"."id"
         INNER JOIN
     "difficulties" AS "D"
     ON "W"."difficulty_id" = "D"."id"
/**where**/;